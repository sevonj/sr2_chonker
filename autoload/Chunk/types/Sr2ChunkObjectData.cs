using System;
using System.IO;
using System.Runtime.InteropServices;
using static Sr2Generic;

public static class Sr2ChunkObjectData
{
    // Object Data 0 (these are near the beginning, after texture list)
    [StructLayout(LayoutKind.Explicit, Size = 0x20)]
    public struct Sr2ChunkObjectData0Header
    {
        [FieldOffset(0x00)] public UInt32 NumRendermodels;
        [FieldOffset(0x04)] public UInt32 NumObjectTransforms;
        [FieldOffset(0x08)] public UInt32 NumModels;
        [FieldOffset(0x0C)] public UInt32 NumUnknown3s;
        [FieldOffset(0x10)] public UInt32 NumUnknown4s;

        public Sr2ChunkObjectData0Header(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkObjectData0Header>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkObjectData0Header)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkObjectData0Header));
            handle.Free();
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x18)]
    public struct Sr2ChunkRendermodelUnknown
    {
        [FieldOffset(0x00)] public Int32 Unknown0x00;
        [FieldOffset(0x04)] public Int32 Unknown0x04;
        [FieldOffset(0x08)] public Int32 Unknown0x08;
        [FieldOffset(0x0C)] public Int32 Unknown0x0C;
        [FieldOffset(0x10)] public Int32 Unknown0x10;
        [FieldOffset(0x14)] public Int32 Unknown0x14;

        public Sr2ChunkRendermodelUnknown(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkRendermodelUnknown>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkRendermodelUnknown)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkRendermodelUnknown));
            handle.Free();
        }
    }

    /// Used by cityobjects, but there are always a couple that are left over.
    [StructLayout(LayoutKind.Explicit, Size = 0x50)]
    public struct Sr2ChunkObjectTransform
    {
        [FieldOffset(0x00)] public Sr2Vector3 Origin;
        [FieldOffset(0x0C)] public Sr2Vector3 BasisX;
        [FieldOffset(0x18)] public Sr2Vector3 BasisY;
        [FieldOffset(0x24)] public Sr2Vector3 BasisZ;
        [FieldOffset(0x4C)] public Single Unknown0x4C;
        [FieldOffset(0x54)] public Int32 Unknown0x54;
        [FieldOffset(0x58)] public UInt32 ModelIdx; // Rendermodel
        [FieldOffset(0x5C)] public Int32 Unknown0x5C;

        public Sr2ChunkObjectTransform(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkObjectTransform>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkObjectTransform)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkObjectTransform));
            handle.Free();
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x64)]
    public struct Sr2ChunkObjectUnknown3
    {
        [FieldOffset(0x00)] public Single Unknown0x00;
        [FieldOffset(0x04)] public Single Unknown0x04;
        [FieldOffset(0x08)] public Single Unknown0x08;
        [FieldOffset(0x0C)] public UInt32 Unknown0x0C;
        [FieldOffset(0x10)] public Single Unknown0x10;
        [FieldOffset(0x14)] public UInt32 Unknown0x14;
        [FieldOffset(0x18)] public Single Unknown0x18;
        [FieldOffset(0x1C)] public Single Unknown0x1C;
        [FieldOffset(0x20)] public Single Unknown0x20;
        [FieldOffset(0x24)] public UInt32 Unknown0x24;
        [FieldOffset(0x28)] public Single Unknown0x28;
        [FieldOffset(0x2C)] public UInt32 Unknown0x2C;
        [FieldOffset(0x30)] public Single Unknown0x30;
        [FieldOffset(0x34)] public Single Unknown0x34;
        [FieldOffset(0x38)] public Single Unknown0x38;
        [FieldOffset(0x3C)] public Single Unknown0x3C;
        [FieldOffset(0x40)] public Single Unknown0x40;
        [FieldOffset(0x44)] public Single Unknown0x44;
        [FieldOffset(0x48)] public Single Unknown0x48;
        [FieldOffset(0x4C)] public Single Unknown0x4C;
        [FieldOffset(0x50)] public Single Unknown0x50;
        [FieldOffset(0x54)] public Single Unknown0x54;
        [FieldOffset(0x58)] public Single Unknown0x58;
        [FieldOffset(0x5C)] public Single Unknown0x5C;
        [FieldOffset(0x60)] public UInt32 Unknown0x60;

        public Sr2ChunkObjectUnknown3(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkObjectUnknown3>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkObjectUnknown3)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkObjectUnknown3));
            handle.Free();
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 0x34)]
    public struct Sr2ChunkObjectUnknown4
    {
        [FieldOffset(0x00)] public Single Unknown0x00;
        [FieldOffset(0x04)] public Single Unknown0x04;
        [FieldOffset(0x08)] public Single Unknown0x08;
        [FieldOffset(0x0C)] public Single Unknown0x0C;
        [FieldOffset(0x10)] public Single Unknown0x10;
        [FieldOffset(0x14)] public Single Unknown0x14;
        [FieldOffset(0x18)] public Single Unknown0x18;
        [FieldOffset(0x1C)] public Single Unknown0x1C;
        [FieldOffset(0x20)] public Single Unknown0x20;
        [FieldOffset(0x24)] public Single Unknown0x24;
        [FieldOffset(0x28)] public Single Unknown0x28;
        [FieldOffset(0x2C)] public Single Unknown0x2C;
        [FieldOffset(0x30)] public Single Unknown0x30;

        public Sr2ChunkObjectUnknown4(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkObjectUnknown4>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkObjectUnknown4)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkObjectUnknown4));
            handle.Free();
        }
    }

    // Object Data 1 (these come after models in the file):
    [StructLayout(LayoutKind.Explicit, Size = 0x50)]
    public struct Sr2ChunkCityobject
    {
        [FieldOffset(0x00)] public Sr2Vector3 CullBoxMin; // CullBox is an AABB used for culling
        [FieldOffset(0x0C)] public UInt32 Uninitialized0x0C;
        [FieldOffset(0x10)] public Sr2Vector3 CullBoxMax;
        [FieldOffset(0x1C)] public Single CullDistance;
        [FieldOffset(0x20)] public UInt32 Uninitialized0x20;
        [FieldOffset(0x24)] public UInt32 Unknown0x24;
        [FieldOffset(0x28)] public UInt32 Uninitialized0x28;
        [FieldOffset(0x2C)] public Int16 Unknown0x2C;
        [FieldOffset(0x30)] public Int32 Flags;
        [FieldOffset(0x34)] public Int32 Unknown0x34;
        [FieldOffset(0x38)] public Int32 Unknown0x38; // Almost always 0. Exceptions: sr2_skybox, sr2_intdkmissunkdk, sr2_intarcutlimo, sr2_intaicutjyucar
        [FieldOffset(0x3C)] public Int32 Unknown0x3C;
        [FieldOffset(0x40)] public UInt32 TransformIdx; // Transforms are separate
        [FieldOffset(0x44)] public UInt32 Unknown0x44;
        [FieldOffset(0x48)] public UInt32 Unknown0x48;
        [FieldOffset(0x4C)] public UInt32 Unknown0x4C;

        public Sr2ChunkCityobject(FileStream fs) : this()
        {
            byte[] buffer = new byte[Marshal.SizeOf<Sr2ChunkCityobject>()];
            fs.Read(buffer, 0, buffer.Length);

            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            this = (Sr2ChunkCityobject)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(Sr2ChunkCityobject));
            handle.Free();
        }
    }
}