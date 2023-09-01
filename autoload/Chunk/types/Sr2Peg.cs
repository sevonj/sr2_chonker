using System;
using System.Runtime.InteropServices;

public static class Sr2PegPc
{
    public enum D3DFormat
    {
        DXT1 = 400,
        DXT3 = 401,
        DXT5 = 402,
        R5G6B5 = 403,
        A1R5G5B5 = 404,
        A4R4G4B4 = 405,
        R8G8B8 = 406,
        A8R8G8B8 = 407,
        V8U8 = 408,
        CxV8U8 = 409,
        A8 = 410
    }

    [StructLayout(LayoutKind.Sequential, Size = 0x18)]
    public struct PegHeader
    {
        public Int32 Signature;     //
        public UInt16 Version;      //
        public UInt16 Platform;     // See peg_platform enum (but didn't provide it?)
        public Int32 DirBlockSize;  // Size of peg_pc (this file)
        public Int32 DataBlockSize; // Size of g_peg_pc
        public UInt16 NumBitmaps;   // Total number of textures.
        public UInt16 Flags;        //
        public UInt16 TotalEntries; // Total number of frames.
        public UInt16 AlignValue;   //
    }

    [StructLayout(LayoutKind.Sequential, Size = 0x30)]
    public struct PegFrame
    {
        public UInt32 PtrData;          // Image data in g_peg (Is always aligned by 16B)
        public UInt16 Width;            // Image width
        public UInt16 Height;           // Image height
        public UInt16 PixelFormat;      // DDS Pixel Format
        public UInt16 PaletteFormat;    // Palette Format
        public UInt16 AnimTilesWidth;   // for animated textures using an anim sheet BM_F_ANIM_SHEET
        public UInt16 AnimTilesHeight;  //
        public UInt16 NumFrames;        //
        public UInt16 Flags;            //
        public UInt32 PtrFilename;      // Not used in SR2?
        public UInt16 PaletteSize;      // Palettesize
        public byte FPS;                // Frames Per Second
        public byte MipLevels;          // Mipmap levels
        public UInt32 FrameSize;        // size of image data
        public UInt32 PtrNext;          // each base bitmap_entry will maintain a linked list of
        public UInt32 PtrPrev;          // actual peg entries so we can do unloading of pegs very quickly
        public UInt32 Cache0;           // generic texture caching data, used differently on different platforms
        public UInt32 Cache1;           // 
    }
}