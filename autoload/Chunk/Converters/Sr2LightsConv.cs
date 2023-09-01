using System;
using System.IO;
using System.Text.Json;
using System.Text;
using static Sr2ChunkLights;
using static Sr2ChunkLightsJSON;
using static Sr2Generic;


public class Sr2LightsConv
{
    public void BinToJSON(string path_bin, string path_json)
    {
        using (FileStream fs = System.IO.File.OpenRead(path_bin))
        {
            using (StreamWriter sw = File.CreateText(path_json))
            {
                Sr2ChunkLightsJSONRoot json = new Sr2ChunkLightsJSONRoot();

                Sr2ChunkLightHeader header = new Sr2ChunkLightHeader(fs);
                json.Unknown0x04 = header.Unknown0x04;
                json.Lights = new Sr2ChunkLightDataJSON[header.NumLights];

                Sr2ChunkLightData[] datas = new Sr2ChunkLightData[header.NumLights];
                string[] names = new string[header.NumLights];
                
                // Light Data
                for (int i = 0; i < header.NumLights; i++)
                    datas[i] = new Sr2ChunkLightData(fs);

                // Light Names
                for (int i = 0; i < header.NumLights; i++)
                    names[i] = new NullTerminatedString(fs).Str;

                // Light Floats & Put them in
                for (int i = 0; i < header.NumLights; i++)
                {
                    float[] floats = new float[datas[i].NumUnknownFloats];
                    BinaryReader br = new BinaryReader(fs);

                    for (int ii = 0; ii < datas[i].NumUnknownFloats; ii++)
                        floats[ii] = br.ReadSingle();

                    json.Lights[i] = new Sr2ChunkLightDataJSON(datas[i], names[i], floats);
                }
                sw.Write(JsonSerializer.Serialize(json));
            }
        }

    }
}