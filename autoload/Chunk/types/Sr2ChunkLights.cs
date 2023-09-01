using System;
using System.IO;
using System.Runtime.InteropServices;
using static Sr2Generic;

/// Light related structs in SR2 .chunk_pc files
public static class Sr2ChunkLights
{
    public const UInt32 FLAG_UNKNOWN0 = 0x8;
    public const UInt32 FLAG_UNKNOWN1 = 0x10;
    public const UInt32 FLAG_UNKNOWN2 = 0x20;
    public const UInt32 FLAG_UNKNOWN3 = 0x40;
    public const UInt32 FLAG_UNKNOWN4 = 0x80;
    public const UInt32 FLAG_LIGHT_LEVEL = 0x100;
    public const UInt32 FLAG_LIGHT_CHARACTER = 0x200;
    public const UInt32 FLAG_SHADOW_LEVEL = 0x400;
    public const UInt32 FLAG_SHADOW_CHARACTER = 0x800;
    public const UInt32 FLAG_UNKNOWN9 = 0x2000;
    public const UInt32 FLAG_UNKNOWN10 = 0x8000;
    public const UInt32 FLAG_UNKNOWN11 = 0x20000;


    [StructLayout(LayoutKind.Explicit)]
    public struct Sr2ChunkLightHeader
    {
        [FieldOffset(0x00)] public UInt32 NumLights;
        [FieldOffset(0x04)] public UInt32 Unknown0x04;
        
        public Sr2ChunkLightHeader(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkLightHeader>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkLightHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkLightHeader));
            handle.Free();
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct Sr2ChunkLightData
    {
        [FieldOffset(0x00)] public UInt32 Flags;
        [FieldOffset(0x08)] public Sr2Vector3 Color;
        [FieldOffset(0x14)] public UInt32 Unknown0x14;
        [FieldOffset(0x18)] public UInt32 Unknown0x18;
        [FieldOffset(0x1c)] public UInt32 Unknown0x1c;
        [FieldOffset(0x20)] public UInt32 NumUnknownFloats;
        [FieldOffset(0x24)] public Int32 Uninitialized0x24;     // -1
        [FieldOffset(0x28)] public Int32 Unknown0x28;           // Iggy's performance complaint
        [FieldOffset(0x2c)] public Int32 Uninitialized0x2c;     // -1
        [FieldOffset(0x30)] public UInt32 Unknown0x30;
        [FieldOffset(0x34)] public Single Unknown0x34;
        [FieldOffset(0x38)] public Single Unknown0x38;
        [FieldOffset(0x3c)] public Sr2Vector3 Origin;
        [FieldOffset(0x48)] public Sr2Vector3 BasisX;
        [FieldOffset(0x54)] public Sr2Vector3 BasisY;
        [FieldOffset(0x60)] public Sr2Vector3 BasisZ;
        [FieldOffset(0x6c)] public Single Unknown0x6c;
        [FieldOffset(0x70)] public Single Unknown0x70;
        [FieldOffset(0x74)] public Single RadiusInner;
        [FieldOffset(0x78)] public Single RadiusOuter;
        [FieldOffset(0x7c)] public Single RenderDistance;
        [FieldOffset(0x80)] public Int32 Uninitialized0x80;     // -1
        [FieldOffset(0x84)] public Int32 Parent;                // Index of a city object. -1 if none.
        [FieldOffset(0x88)] public UInt32 Unknown0x88;
        [FieldOffset(0x8c)] public UInt32 Unknown0x8c;
        [FieldOffset(0x90)] public UInt32 Type;                 // Possible values: 0, 1, 2, 3

        public Sr2ChunkLightData(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkLightData>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkLightData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkLightData));
            handle.Free();
        }
    }
}
