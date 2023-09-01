using System.IO;
using System.Text.Json;
using static Sr2ChunkObjectData;
using static Sr2ChunkObjectDataJSON;
using static Sr2Generic;


public class Sr2ObjectDataConv
{
    public void BinToJSON(string path_bin_header, string path_bin_data0, string path_bin_data1, string path_json)
    {
        using (StreamWriter sw = File.CreateText(path_json))
        {
            Sr2ChunkObjectDataJSONRoot json;
            using (FileStream fs = System.IO.File.OpenRead(path_bin_data0))
            {
                Sr2ChunkObjectData0Header header = new Sr2ChunkObjectData0Header(fs);
                json = new Sr2ChunkObjectDataJSONRoot(header);

                // RendermodelUnknowns
                for (int i = 0; i < header.NumRendermodels; i++)
                    json.RendermodelUnknowns[i] = new Sr2ChunkRendermodelUnknownJSON(new Sr2ChunkRendermodelUnknown(fs));

                // Align 16
                fs.Seek((16 - (int)fs.Position % 16) % 16, SeekOrigin.Current);

                // Object Transforms
                for (int i = 0; i < header.NumObjectTransforms; i++)
                    json.ObjectTransforms[i] = new Sr2ChunkObjectTransformJSON(new Sr2ChunkObjectTransform(fs));

                // Align 16
                fs.Seek((16 - (int)fs.Position % 16) % 16, SeekOrigin.Current);

                // Unknown3s
                for (int i = 0; i < header.NumUnknown3s; i++)
                    json.Unknown3s[i] = new Sr2ChunkObjectUnknown3JSON(new Sr2ChunkObjectUnknown3(fs));

                // Align 16
                fs.Seek((16 - (int)fs.Position % 16) % 16, SeekOrigin.Current);

                // Unknown4s
                for (int i = 0; i < header.NumUnknown4s; i++)
                    json.Unknown4s[i] = new Sr2ChunkObjectUnknown4JSON(new Sr2ChunkObjectUnknown4(fs));

                // Align 16
                fs.Seek((16 - (int)fs.Position % 16) % 16, SeekOrigin.Current);
            }

            uint NumCityobjects;
            using (FileStream fs = System.IO.File.OpenRead(path_bin_header))
            {
                BinaryReader br = new BinaryReader(fs);
                fs.Seek(0x94, SeekOrigin.Begin);
                NumCityobjects = br.ReadUInt32();
            }

            using (FileStream fs = System.IO.File.OpenRead(path_bin_data1))
            {
                json.Cityobjects = new Sr2ChunkCityobjectJSON[NumCityobjects];
                for (int i = 0; i < NumCityobjects; i++)
                    json.Cityobjects[i] = new Sr2ChunkCityobjectJSON(new Sr2ChunkCityobject(fs));

                for (int i = 0; i < NumCityobjects; i++)
                    json.Cityobjects[i].Name = new NullTerminatedString(fs).Str;
            }

            sw.Write(JsonSerializer.Serialize(json));
        }

    }
}