using Godot;
using System;
using System.IO;
using Kaitai;

public class Sr2GpuChunkLoader
{
	public Mesh[] LoadMeshesFromChunk(ref Sr2ChunkPc chunk, string filepath)
	{
		if (!System.IO.File.Exists(filepath))
		{
			GD.PushWarning("ChunkLoader.LoadGPUChunk(): File doesn't exist! " + filepath);
			return null;
		}

		Mesh[] meshes = new Mesh[chunk.NumRendermodels];

		using (FileStream fs = System.IO.File.OpenRead(filepath))
		{
			BinaryReader br = new BinaryReader(fs);


			// Find correct Modelheader. Usually index zero, but sometimes not.
			// Never more than one of them.
			int mhidx = 0;
			for (int i = 0; i < chunk.ModelCount; i++)
			{
				if (chunk.IndexBuffers[i].Type == 0)
				{
					mhidx = i;
					break;
				}
			}

			// Find Buffer Offsets
			uint iBufOffset = 0;
			uint[] vBufOffsets = new uint[chunk.IndexBuffers[mhidx].NumBlocks];
			for (int i = 0; i < chunk.IndexBuffers[mhidx].NumBlocks; i++)
			{
				vBufOffsets[i] = iBufOffset;
				uint vertCount = chunk.VertexBuffers[mhidx].Blocks[i].NumVertices;
				uint vertSize = chunk.VertexBuffers[mhidx].Blocks[i].VertStride0;
				iBufOffset += vertCount * vertSize;
				// byte align
				while ((iBufOffset & 0xf) != 0) { iBufOffset += 1; }
			}

			for (int i = 0; i < chunk.NumRendermodels; i++)
			{
				Sr2ChunkPc.Rendermodel model = chunk.Rendermodels[i];
				SurfaceTool st = new SurfaceTool();
				st.Begin(Mesh.PrimitiveType.Triangles);

				// Buffers
				uint totalVertCount = 0;
				for (int ii = 0; ii < model.NumSubmeshes; ii++)
				{
					// Skip models that use unk_2
					if (model.NumSubmesh2s != null) continue;

					int vertBufID = (int)model.Submeshes[ii].VertBufferId;
					uint vertSize = chunk.VertexBuffers[mhidx].Blocks[vertBufID].VertStride0;

					int indexOffset = (int)(iBufOffset + model.Submeshes[ii].OffIndices * 2);
					uint vertexOffset = vBufOffsets[vertBufID] + model.Submeshes[ii].OffVertices * vertSize;


					// Get VertCount (it isn't stored explicitly so gotta pull it if from indices)
					fs.Seek(indexOffset, SeekOrigin.Begin);
					uint tempVertCount = 0;
					for (int iii = 0; iii < model.Submeshes[ii].NumIndices; iii++)
					{
						tempVertCount = (uint)Math.Max(br.ReadInt16() + 1, tempVertCount);
					}
					//model.Submeshes[ii].vertCount = tempVertCount;


					// Get Vertices
					fs.Seek(vertexOffset, SeekOrigin.Begin);
					for (int iii = 0; iii < tempVertCount; iii++)
					{
						Vector3 pos = new Vector3(-br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
						st.AddVertex(pos);
						fs.Seek(vertSize - 12, SeekOrigin.Current);
					}

					// Get Indices
					fs.Seek(indexOffset, SeekOrigin.Begin);
					for (int iii = 0; iii < model.Submeshes[ii].NumIndices - 2; iii++)
					{
						uint i0 = totalVertCount + br.ReadUInt16();
						uint i1 = totalVertCount + br.ReadUInt16();
						uint i2 = totalVertCount + br.ReadUInt16();
						fs.Seek(-4, SeekOrigin.Current);

						//GD.Print(i0, " ", i1, " ", i2);

						// degen tri
						if (i0 == i1 || i0 == i2 || i1 == i2)
						{
							continue;
						}

						// Odd faces are flipped
						if ((iii % 2) == 0)
						{
							st.AddIndex((int)i0);
							st.AddIndex((int)i1);
							st.AddIndex((int)i2);
						}
						else
						{
							st.AddIndex((int)i2);
							st.AddIndex((int)i1);
							st.AddIndex((int)i0);
						}
					}
					totalVertCount += tempVertCount;
				}

				meshes[i] = st.Commit();
				//MeshInstance meshInstance = new MeshInstance();
				//meshInstance.Mesh = mesh;
				//meshInstance.Name = "RenderModel" + i.ToString();
				//GetNode("/root/main/chunk/rendermodels").AddChild(meshInstance);
			}
		}
		return meshes;
	}
}
