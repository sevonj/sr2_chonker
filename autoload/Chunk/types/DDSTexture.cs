using System;
using System.Runtime.InteropServices;

public static class DDSTexture
{
    public const uint DWMAGIC = 0x20534444;

    public const uint DDSD_CAPS = 0x1;
    public const uint DDSD_HEIGHT = 0x2;
    public const uint DDSD_WIDTH = 0x4;
    public const uint DDSD_PITCH = 0x8;
    public const uint DDSD_PIXELFORMAT = 0x1000;
    public const uint DDSD_MIPMAPCOUNT = 0x20000;
    public const uint DDSD_LINEARSIZE = 0x80000;
    public const uint DDSD_DEPTH = 0x800000;

    public const uint DDPF_ALPHAPIXELS = 0x1;
    public const uint DDPF_ALPHA = 0x2;
    public const uint DDPF_FOURCC = 0x4;
    public const uint DDPF_RGB = 0x40;
    public const uint DDPF_YUV = 0x200;
    public const uint DDPF_LUMINANCE = 0x20000;

    public const uint DDSCAPS_COMPLEX = 0x8;
    public const uint DDSCAPS_MIPMAP = 0x400000;
    public const uint DDSCAPS_TEXTURE = 0x1000;


    [StructLayout(LayoutKind.Sequential)]
    public struct DDSTextureHeader
    {
        public UInt32 dwSize;
        public UInt32 dwFlags;
        public UInt32 dwHeight;
        public UInt32 dwWidth;
        public UInt32 dwPitchOrLinearSize;
        public UInt32 dwDepth;
        public UInt32 dwMipMapCount;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
        public UInt32[] dwReserved1;
        public DDSPixelFormat ddspf;
        public UInt32 dwCaps;
        public UInt32 dwCaps2;
        public UInt32 dwCaps3;
        public UInt32 dwCaps4;
        public UInt32 dwReserved2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct DDSPixelFormat
    {
        public UInt32 dwSize;
        public UInt32 dwFlags;
        public UInt32 dwFourCC;
        public UInt32 dwRGBBitCount;
        public UInt32 dwRBitMask;
        public UInt32 dwGBitMask;
        public UInt32 dwBBitMask;
        public UInt32 dwABitMask;
    }
}