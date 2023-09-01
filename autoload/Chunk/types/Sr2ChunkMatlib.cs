using System;
using System.IO;
using System.Runtime.InteropServices;

public static class Sr2ChunkMatlib
{
    [StructLayout(LayoutKind.Explicit, Size = 0x24)]
    public struct Sr2ChunkMatlibHeader
    {
        [FieldOffset(0x00)] public UInt32 NumMaterials;
        [FieldOffset(0x10)] public UInt32 NumShaderConstants;
        [FieldOffset(0x1c)] public UInt32 NumShaderTextureProp;

        public Sr2ChunkMatlibHeader(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkMatlibHeader>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkMatlibHeader)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkMatlibHeader));
            handle.Free();
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x06)]
    public struct Sr2ChunkMatlibUnknown2
    {
        [FieldOffset(0x00)] public UInt16 X;
        [FieldOffset(0x02)] public UInt16 Y;
        [FieldOffset(0x04)] public UInt16 Z;

        public Sr2ChunkMatlibUnknown2(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkMatlibUnknown2>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkMatlibUnknown2)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkMatlibUnknown2));
            handle.Free();
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x4)]
    public struct Sr2ChunkMatlibTexture
    {
        [FieldOffset(0x00)] public Int16 Idx;
        [FieldOffset(0x02)] public Int16 Flags;

        public Sr2ChunkMatlibTexture(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkMatlibTexture>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkMatlibTexture)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkMatlibTexture));
            handle.Free();
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x18)]
    public struct Sr2ChunkMatlibMaterial
    {
        [FieldOffset(0x00)] public UInt32 Unknown0x00;
        [FieldOffset(0x04)] public UInt32 Shader;       // Probably
        [FieldOffset(0x08)] public UInt32 Unknown0x08;
        [FieldOffset(0x0c)] public UInt16 NumUnknown2;
        [FieldOffset(0x0e)] public UInt16 NumTextures;
        [FieldOffset(0x10)] public UInt32 Flags;
        [FieldOffset(0x14)] public Int32 Unknown0x14;

        public Sr2ChunkMatlibMaterial(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkMatlibMaterial>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkMatlibMaterial)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkMatlibMaterial));
            handle.Free();
        }
    }

    // One per material
    [StructLayout(LayoutKind.Explicit, Size = 0x10)]
    public struct Sr2ChunkMatlibConstData
    {
        [FieldOffset(0x00)] public UInt32 NumConstants0; // Total number of floats in first constant stream
        [FieldOffset(0x04)] public UInt32 IdxConstants0; // Starting index of this stream in constant block
        [FieldOffset(0x08)] public UInt32 NumConstants1; // Total number of floats in second constant stream
        [FieldOffset(0x0c)] public UInt32 IdxConstants1; // Starting index..

        public Sr2ChunkMatlibConstData(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkMatlibConstData>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkMatlibConstData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkMatlibConstData));
            handle.Free();
        }
    }

    // These are probably texture properties for a particular shader.
    public struct Sr2ChunkMatlibShaderTextureProp
    {
        public UInt32 Unknown0x00;      // Texture?
        public UInt32 Shader;           // You should always be able to find one or more materials with matching shader
        public UInt16 NumShaderPropData;
        public UInt16 Unknown0x0A;
        public UInt32 Uninitialized0xC;

        public Sr2ChunkMatlibShaderTextureProp(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkMatlibShaderTextureProp>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkMatlibShaderTextureProp)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkMatlibShaderTextureProp));
            handle.Free();
        }
    }
}