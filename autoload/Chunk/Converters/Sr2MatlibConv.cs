using System;
using System.IO;
using System.Text.Json;
using System.Text;
using static Sr2ChunkMatlib;
using static Sr2Generic;
using static Sr2ChunkMatlibJSON;
using static Sr2GenericJSON;

public class Sr2MatlibConv
{
	public void BinToJSON(string path_matlib, string path_texlist, string path_json)
	{
		using (StreamWriter sw = File.CreateText(path_json))
		{
			var json = new Sr2ChunkMatlibJSONRoot();

			// Texture names
			using (FileStream fs = System.IO.File.OpenRead(path_texlist))
			{
				BinaryReader br = new BinaryReader(fs);
				uint numTextures = br.ReadUInt32();
				json.Textures = new string[numTextures];
				fs.Seek(numTextures * 4, SeekOrigin.Current);
				for (int i = 0; i < numTextures; i++)
					json.Textures[i] = new NullTerminatedString(fs).Str;
			}

			using (FileStream fs = System.IO.File.OpenRead(path_matlib))
			{
				Sr2ChunkMatlibHeader header = new Sr2ChunkMatlibHeader(fs);
				BinaryReader br = new BinaryReader(fs);
				json.Materials = new Sr2ChunkMatlibJSONMaterial[header.NumMaterials];

				// Materials
				for (int i = 0; i < header.NumMaterials; i++)
				{
					Sr2ChunkMatlibMaterial mat = new Sr2ChunkMatlibMaterial(fs);
					json.Materials[i] = new Sr2ChunkMatlibJSONMaterial
					{
						Name = "material_" + i.ToString(),
						Unknown0x00 = mat.Unknown0x00,
						Shader = mat.Shader,
						Unknown0x08 = mat.Unknown0x08,
						Unknown2s = new Sr2ChunkMatlibJSONUnknown2[mat.NumUnknown2],
						Textures = new Sr2ChunkMatlibJSONTexture[mat.NumTextures],
						Flags = mat.Flags,
						Unknown0x14 = mat.Unknown0x14,
					};

				}

				// Material Unknown2s
				for (int i = 0; i < header.NumMaterials; i++)
				{
					for (int ii = 0; ii < json.Materials[i].Unknown2s.Length; ii++)
					{
						json.Materials[i].Unknown2s[ii] = new Sr2ChunkMatlibJSONUnknown2(new Sr2ChunkMatlibUnknown2(fs));
					}
					fs.Seek(alignsize(fs.Position, 4), SeekOrigin.Current);
				}

				// Shader constants
				for (int i = 0; i < header.NumMaterials; i++)
				{
					Sr2ChunkMatlibConstData constdata = new Sr2ChunkMatlibConstData(fs);
					json.Materials[i].Constants0 = new Sr2Vector4JSON[constdata.NumConstants0 / 4];
					json.Materials[i].IdxConstants0 = constdata.IdxConstants0;
					json.Materials[i].Constants1 = new Sr2Vector4JSON[constdata.NumConstants1 / 4];
					json.Materials[i].IdxConstants1 = constdata.IdxConstants1;
				}

				float[] consts = new float[header.NumShaderConstants];
				for (int i = 0; i < header.NumShaderConstants; i++)
				{
					consts[i] = br.ReadSingle();
				}

				fs.Seek(alignsize(fs.Position, 0x10), SeekOrigin.Current);
				for (int i = 0; i < header.NumMaterials; i++)
				{
					for (int ii = 0; ii < json.Materials[i].Constants0.Length; ii++)
					{

						json.Materials[i].Constants0[ii] = new Sr2Vector4JSON
						{
							X = consts[json.Materials[i].IdxConstants0 + ii * 4],
							Y = consts[json.Materials[i].IdxConstants0 + ii * 4 + 1],
							Z = consts[json.Materials[i].IdxConstants0 + ii * 4 + 2],
							W = consts[json.Materials[i].IdxConstants0 + ii * 4 + 3]
						};
					}
					for (int ii = 0; ii < json.Materials[i].Constants1.Length; ii++)
					{
						json.Materials[i].Constants1[ii] = new Sr2Vector4JSON
						{
							X = consts[json.Materials[i].IdxConstants1 + ii * 4],
							Y = consts[json.Materials[i].IdxConstants1 + ii * 4 + 1],
							Z = consts[json.Materials[i].IdxConstants1 + ii * 4 + 2],
							W = consts[json.Materials[i].IdxConstants1 + ii * 4 + 3]
						};
					}
				}

				// Textures
				for (int i = 0; i < header.NumMaterials; i++)
				{
					for (int ii = 0; ii < 16; ii++)
					{
						Sr2ChunkMatlibTexture tex = new Sr2ChunkMatlibTexture(fs);
						if (ii >= json.Materials[i].Textures.Length)
							continue;
						json.Materials[i].Textures[ii] = new Sr2ChunkMatlibJSONTexture
						{
							Name = json.Textures[tex.Idx],
							Flags = tex.Flags
						};
					}
				}

				// Shader Texture Properties?
				json.ShaderTextureProps = new Sr2ChunkMatlibJSONShaderTextureProp[header.NumShaderTextureProp];
				for (int i = 0; i < header.NumShaderTextureProp; i++)
				{
					Sr2ChunkMatlibShaderTextureProp prop = new Sr2ChunkMatlibShaderTextureProp(fs);
					json.ShaderTextureProps[i] = new Sr2ChunkMatlibJSONShaderTextureProp
					{
						Unknown0x00 = prop.Unknown0x00,
						Shader = prop.Shader,
						ShaderTexturePropData = new UInt32[prop.NumShaderPropData],
						Unknown0x0A = prop.Unknown0x0A,
					};
				}

				// Shader Texture Prop Data
				for (int i = 0; i < header.NumShaderTextureProp; i++)
				{
					for (int ii = 0; ii < json.ShaderTextureProps[i].ShaderTexturePropData.Length; ii++)
					{
						json.ShaderTextureProps[i].ShaderTexturePropData[ii] = br.ReadUInt32();
					}
				}

				sw.Write(JsonSerializer.Serialize(json));
			}
		}
	}

	public void JSONToBin(string path_matlib, string path_texlist, string path_json)
	{
		Sr2ChunkMatlibJSONRoot matStruct = JsonSerializer.Deserialize<Sr2ChunkMatlibJSONRoot>(File.ReadAllText(path_json));

		// Create Texlist
		File.Delete(path_texlist);
		using (FileStream fs = File.OpenWrite(path_texlist))
		{
			BinaryWriter bw = new BinaryWriter(fs);
			// NumTextures
			bw.Write((UInt32)matStruct.Textures.Length);
			// Padding
			for (int i = 0; i < matStruct.Textures.Length; i++)
				bw.Write((UInt32)0);
			// TextureNames
			foreach (String tex in matStruct.Textures)
				bw.Write(ASCIIEncoding.ASCII.GetBytes(tex + char.MinValue));

			bw.Write(align(fs, 16));
		}

		// Create Matlib
		File.Delete(path_matlib + "_new");
		using (FileStream fs = File.OpenWrite(path_matlib + "_new"))
		{
			BinaryWriter bw = new BinaryWriter(fs);

			int NumShaderConstants = 0;
			foreach (Sr2ChunkMatlibJSONMaterial mat in matStruct.Materials)
				NumShaderConstants += mat.Constants0.Length * 4 + mat.Constants1.Length * 4;

			// Header
			bw.Write((UInt32)matStruct.Materials.Length);
			bw.Write(new byte[12]);
			bw.Write((UInt32)NumShaderConstants);
			bw.Write(new byte[8]);
			bw.Write((UInt32)matStruct.ShaderTextureProps.Length);
			bw.Write(new byte[4]);

			// Materials
			foreach (Sr2ChunkMatlibJSONMaterial mat in matStruct.Materials)
			{
				bw.Write((UInt32)mat.Unknown0x00);
				bw.Write((UInt32)mat.Shader);
				bw.Write((UInt32)mat.Unknown0x08);
				bw.Write((UInt16)mat.Unknown2s.Length);
				bw.Write((UInt16)mat.Textures.Length);
				bw.Write(new byte[2]);
				bw.Write((UInt16)mat.Flags);
				bw.Write((UInt32)mat.Unknown0x14);
			}

			// Mat Unk1s
			foreach (Sr2ChunkMatlibJSONMaterial mat in matStruct.Materials)
			{
				if (mat.Unknown2s.Length == 0) continue; // ??? Otherwise this still writes stuff ???
				foreach (Sr2ChunkMatlibJSONUnknown2 unk1 in mat.Unknown2s)
				{
					bw.Write(unk1.X);
					bw.Write(unk1.Y);
					bw.Write(unk1.Z);
				}
				bw.Write(align(fs, 4));
			}
			// Shader Constants
			foreach (Sr2ChunkMatlibJSONMaterial mat in matStruct.Materials)
			{
				bw.Write((UInt32)mat.Constants0.Length * 4);
				bw.Write((UInt32)mat.IdxConstants0);
				bw.Write((UInt32)mat.Constants1.Length * 4);
				bw.Write((UInt32)mat.IdxConstants1);
			}
			bw.Write(align(fs, 16));

			// Shader Constants Block
			long off_consts = fs.Position;
			int num_contst = 0;
			foreach (Sr2ChunkMatlibJSONMaterial mat in matStruct.Materials)
				num_contst += mat.Constants0.Length + mat.Constants1.Length;

			// Allocate space
			bw.Write(new byte[num_contst * 4]);

			foreach (Sr2ChunkMatlibJSONMaterial mat in matStruct.Materials)
			{
				fs.Seek(off_consts + mat.IdxConstants0 * 4, SeekOrigin.Begin);
				for (int i = 0; i < mat.Constants0.Length; i++)
				{
					bw.Write((Single)mat.Constants0[i].X);
					bw.Write((Single)mat.Constants0[i].Y);
					bw.Write((Single)mat.Constants0[i].Z);
					bw.Write((Single)mat.Constants0[i].W);
				}
				fs.Seek(off_consts + mat.IdxConstants1 * 4, SeekOrigin.Begin);
				for (int i = 0; i < mat.Constants1.Length; i++)
				{
					bw.Write((Single)mat.Constants1[i].X);
					bw.Write((Single)mat.Constants1[i].Y);
					bw.Write((Single)mat.Constants1[i].Z);
					bw.Write((Single)mat.Constants1[i].W);
				}
			}

			// Textures
			foreach (Sr2ChunkMatlibJSONMaterial mat in matStruct.Materials)
			{
				for (int i = 0; i < 16; i++)
				{
					if (i < mat.Textures.Length)
					{
						UInt16 index = (UInt16)Array.IndexOf(matStruct.Textures, mat.Textures[i].Name);
						bw.Write(index);
						bw.Write((UInt16)mat.Textures[i].Flags);
					}
					else
					{
						bw.Write((Int16)(-1));
						bw.Write((Int16)(-1));
					}
				}
			}

			// Unknown3s
			foreach (Sr2ChunkMatlibJSONShaderTextureProp prop in matStruct.ShaderTextureProps)
			{
				bw.Write((UInt32)prop.Unknown0x00);
				bw.Write((UInt32)prop.Shader);
				bw.Write((UInt16)prop.ShaderTexturePropData.Length);
				bw.Write((UInt16)prop.Unknown0x0A);
				bw.Write((Int32)(-1));
			}

			// Unknown3unk2s
			foreach (Sr2ChunkMatlibJSONShaderTextureProp prop in matStruct.ShaderTextureProps)
			{
				for (int i = 0; i < prop.ShaderTexturePropData.Length; i++)
				{
					bw.Write((UInt32)prop.ShaderTexturePropData[i]);
				}
			}

		}
	}
	int alignsize(long pos, int size)
	{
		return (size - (int)pos % size) % size;
	}

	byte[] align(FileStream fs, int size)
	{
		return new byte[(size - (int)fs.Position % size) % size];
	}
}
