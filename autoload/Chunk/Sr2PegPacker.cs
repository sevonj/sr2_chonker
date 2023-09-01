using System;
using System.IO;
using System.Runtime.InteropServices;
//using Godot;
using static DDSTexture;
using static Sr2PegPc;

// Sr2PegPacker
// Unlike what the name suggests, this cannot actually pack pegfiles, only unpack, which it doesn't
// do particularly well either.
//
// Masamaru's pegfile doc:
// https://www.saintsrowmods.com/forum/threads/saints-row-2-peg-texture-tool.8687/
//
// Minimaul's code for later version?:
// https://github.com/saintsrowmods/ThomasJepp.SaintsRow/blob/master/Bitmaps/Version13/PegHeader.cs
//
// Knobby's official document for SRTT (why did I find this last)
// https://www.saintsrowmods.com/forum/threads/peg-file-format.2908/
//
// It appears that num_bitmaps is always equal to total_entries in chunk pegs.
//

public class Sr2PegPacker
{
    public void Unpack(string path_peg, string dir)
    {
        string path_g_peg = Path.ChangeExtension(path_peg, ".g_peg_pc");

        if (!File.Exists(path_peg))
        {
            return;
        }
        if (!File.Exists(path_g_peg))
        {
            return;
        }
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        using (FileStream fs = File.OpenRead(path_peg))
        {
            // Setup g_peg fs
            FileStream g_fs = File.OpenRead(path_g_peg);
            BinaryReader g_br = new BinaryReader(g_fs);
            //GD.Print("pegging");

            // Read Peg Header
            PegHeader header = ReadPegHeader(fs);

            // Read Peg Frames
            PegFrame[] frames = new PegFrame[header.TotalEntries];
            for (int i = 0; i < header.TotalEntries; i++)
            {
                frames[i] = ReadPegFrame(fs);
            }
            // Read Peg Names
            string[] names = new string[header.NumBitmaps];
            BinaryReader br = new BinaryReader(fs);
            for (int i = 0; i < header.NumBitmaps; i++)
            {
                string str = "";
                while (true)
                {
                    char c = br.ReadChar();
                    if ((int)c == 0) break;
                    str = str + c;
                }
                names[i] = str;
            }

            for (int i = 0; i < header.TotalEntries; i++)
            {
                DDSTextureHeader ddhead = pegframe2ddsheader(frames[i]);
                string filepath = Path.Combine(dir, names[i] + ".dds");
                using (FileStream out_fs = File.OpenWrite(filepath))
                {
                    BinaryWriter bw = new BinaryWriter(out_fs);
                    g_fs.Seek(frames[i].PtrData, SeekOrigin.Begin);

                    byte[] headerbytes = new byte[Marshal.SizeOf(ddhead)];
                    GCHandle handle = GCHandle.Alloc(headerbytes, GCHandleType.Pinned);
                    Marshal.StructureToPtr(ddhead, handle.AddrOfPinnedObject(), false);
                    handle.Free();

                    bw.Write(DDSTexture.DWMAGIC);
                    bw.Write(headerbytes);
                    bw.Write(g_br.ReadBytes((int)frames[i].FrameSize));
                }
            }
        }
    }

    DDSTextureHeader pegframe2ddsheader(PegFrame frame)
    {
        DDSTextureHeader header = new DDSTextureHeader();

        header.dwSize = 124;
        header.dwFlags |= DDSTexture.DDSD_CAPS;
        header.dwFlags |= DDSTexture.DDSD_HEIGHT;
        header.dwFlags |= DDSTexture.DDSD_WIDTH;
        header.dwFlags |= DDSTexture.DDSD_PIXELFORMAT;
        if (frame.MipLevels > 0)
            header.dwFlags |= DDSTexture.DDSD_MIPMAPCOUNT;
        header.dwHeight = frame.Width;
        header.dwWidth = frame.Height;
        //header.dwPitchOrLinearSize = 0;
        //header.dwDepth = 0;
        header.dwMipMapCount = frame.MipLevels;
        header.ddspf = new DDSPixelFormat();
        header.ddspf.dwSize = 32;

        switch ((D3DFormat)frame.PixelFormat)
        {
            case D3DFormat.DXT1:
                header.ddspf.dwFlags |= DDSTexture.DDPF_FOURCC;
                header.ddspf.dwFourCC = MakeFourCC('D', 'X', 'T', '1');
                break;
            case D3DFormat.DXT3:
                header.ddspf.dwFlags |= DDSTexture.DDPF_FOURCC;
                header.ddspf.dwFourCC = MakeFourCC('D', 'X', 'T', '3');
                break;
            case D3DFormat.DXT5:
                header.ddspf.dwFlags |= DDSTexture.DDPF_FOURCC;
                header.ddspf.dwFourCC = MakeFourCC('D', 'X', 'T', '5');
                break;
            case D3DFormat.R5G6B5:
                header.ddspf.dwFlags |= DDSTexture.DDPF_RGB;
                header.ddspf.dwRGBBitCount = 16;
                header.ddspf.dwRBitMask = 0xF800;
                header.ddspf.dwGBitMask = 0x07E0;
                header.ddspf.dwBBitMask = 0x001F;
                break;
            case D3DFormat.A1R5G5B5:
                header.ddspf.dwFlags |= DDSTexture.DDPF_RGB | DDSTexture.DDPF_ALPHAPIXELS;
                header.ddspf.dwRGBBitCount = 16;
                header.ddspf.dwRBitMask = 0x7C00;
                header.ddspf.dwGBitMask = 0x03E0;
                header.ddspf.dwBBitMask = 0x001F;
                header.ddspf.dwABitMask = 0x8000;
                break;
            case D3DFormat.A4R4G4B4:
                header.ddspf.dwFlags |= DDSTexture.DDPF_RGB | DDSTexture.DDPF_ALPHAPIXELS;
                header.ddspf.dwRGBBitCount = 16;
                header.ddspf.dwRBitMask = 0x0F00;
                header.ddspf.dwGBitMask = 0x00F0;
                header.ddspf.dwBBitMask = 0x000F;
                header.ddspf.dwABitMask = 0xF000;
                break;
            case D3DFormat.R8G8B8:
                header.ddspf.dwFlags |= DDSTexture.DDPF_RGB;
                header.ddspf.dwRGBBitCount = 24;
                header.ddspf.dwRBitMask = 0xFF0000;
                header.ddspf.dwGBitMask = 0x00FF00;
                header.ddspf.dwBBitMask = 0x0000FF;
                break;
            case D3DFormat.A8R8G8B8:
                header.ddspf.dwFlags |= DDSTexture.DDPF_RGB | DDSTexture.DDPF_ALPHAPIXELS;
                header.ddspf.dwRGBBitCount = 32;
                header.ddspf.dwRBitMask = 0x00FF0000;
                header.ddspf.dwGBitMask = 0x0000FF00;
                header.ddspf.dwBBitMask = 0x000000FF;
                header.ddspf.dwABitMask = 0xFF000000;
                break;
            case D3DFormat.V8U8:
                header.ddspf.dwFlags |= DDSTexture.DDPF_LUMINANCE | DDSTexture.DDPF_ALPHAPIXELS;
                header.ddspf.dwRGBBitCount = 16;
                header.ddspf.dwRBitMask = 0x00FF;
                header.ddspf.dwGBitMask = 0xFF00;
                header.ddspf.dwBBitMask = 0x0000;
                header.ddspf.dwABitMask = 0x0000;
                break;
            case D3DFormat.CxV8U8:
                header.ddspf.dwFlags |= DDSTexture.DDPF_LUMINANCE | DDSTexture.DDPF_ALPHAPIXELS;
                header.ddspf.dwRGBBitCount = 16;
                header.ddspf.dwRBitMask = 0x00FF;
                header.ddspf.dwGBitMask = 0xFF00;
                header.ddspf.dwBBitMask = 0x0000;
                header.ddspf.dwABitMask = 0x0000;
                break;
            case D3DFormat.A8:
                header.ddspf.dwFlags |= DDSTexture.DDPF_ALPHA;
                header.ddspf.dwRGBBitCount = 8;
                header.ddspf.dwABitMask = 0xFF;
                break;
            default:
                // ERROR
                break;
        }
        header.dwCaps |= DDSTexture.DDSCAPS_TEXTURE;
        if (frame.MipLevels > 0)
        {
            header.dwCaps |= DDSTexture.DDSCAPS_COMPLEX;
            header.dwCaps |= DDSTexture.DDSCAPS_MIPMAP;
        }
        return header;
    }
    private static uint MakeFourCC(char ch0, char ch1, char ch2, char ch3)
    {
        return ((uint)ch0) | (((uint)ch1) << 8) | (((uint)ch2) << 16) | (((uint)ch3) << 24);
    }


    public PegHeader ReadPegHeader(FileStream fs)
    {
        byte[] buffer = new byte[Marshal.SizeOf<PegHeader>()];
        fs.Read(buffer, 0, buffer.Length);

        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        PegHeader header = (PegHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PegHeader));
        handle.Free();

        return header;
    }

    public PegFrame ReadPegFrame(FileStream fs)
    {
        byte[] buffer = new byte[Marshal.SizeOf<PegFrame>()];
        fs.Read(buffer, 0, buffer.Length);

        GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        PegFrame frame = (PegFrame)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PegFrame));
        handle.Free();

        return frame;
    }
}
