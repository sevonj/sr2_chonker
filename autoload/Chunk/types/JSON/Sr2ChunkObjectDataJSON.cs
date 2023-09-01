using System;
using static Sr2GenericJSON;
using static Sr2ChunkObjectData;

/// Used by cityobjects, but there are always a couple that are left over.
public static class Sr2ChunkObjectDataJSON
{

    public struct Sr2ChunkObjectDataJSONRoot
    {
        // Object data 0
        public UInt32 NumRendermodels { get; set; }
        public Sr2ChunkRendermodelUnknownJSON[] RendermodelUnknowns { get; set; }
        public Sr2ChunkObjectTransformJSON[] ObjectTransforms { get; set; }
        public UInt32 NumModels { get; set; }
        public Sr2ChunkObjectUnknown3JSON[] Unknown3s { get; set; }
        public Sr2ChunkObjectUnknown4JSON[] Unknown4s { get; set; }

        // Object Data 1
        public Sr2ChunkCityobjectJSON[] Cityobjects { get; set; }


        public Sr2ChunkObjectDataJSONRoot(Sr2ChunkObjectData0Header data) : this()
        {
            this.NumRendermodels = data.NumRendermodels;
            this.RendermodelUnknowns = new Sr2ChunkRendermodelUnknownJSON[data.NumRendermodels];
            this.ObjectTransforms = new Sr2ChunkObjectTransformJSON[data.NumObjectTransforms];
            this.NumModels = data.NumModels;
            this.Unknown3s = new Sr2ChunkObjectUnknown3JSON[data.NumUnknown3s];
            this.Unknown4s = new Sr2ChunkObjectUnknown4JSON[data.NumUnknown4s];
        }
    }

    // Object data 0

    public struct Sr2ChunkRendermodelUnknownJSON
    {
        public Int32 Unknown0x00 { get; set; }
        public Int32 Unknown0x04 { get; set; }
        public Int32 Unknown0x08 { get; set; }
        public Int32 Unknown0x0C { get; set; }
        public Int32 Unknown0x10 { get; set; }
        public Int32 Unknown0x14 { get; set; }

        public Sr2ChunkRendermodelUnknownJSON(Sr2ChunkRendermodelUnknown data) : this()
        {
            this.Unknown0x00 = data.Unknown0x00;
            this.Unknown0x04 = data.Unknown0x04;
            this.Unknown0x08 = data.Unknown0x08;
            this.Unknown0x0C = data.Unknown0x0C;
            this.Unknown0x10 = data.Unknown0x10;
            this.Unknown0x14 = data.Unknown0x14;
        }
    }

    public struct Sr2ChunkObjectTransformJSON
    {
        public Sr2Vector3JSON Origin { get; set; }
        public Sr2Vector3JSON BasisX { get; set; }
        public Sr2Vector3JSON BasisY { get; set; }
        public Sr2Vector3JSON BasisZ { get; set; }
        public Single Unknown0x4C { get; set; }
        public Int32 Unknown0x54 { get; set; }
        public UInt32 ModelIdx { get; set; }
        public Int32 Unknown0x5C { get; set; }

        public Sr2ChunkObjectTransformJSON(Sr2ChunkObjectTransform data) : this()
        {
            this.Origin = new Sr2Vector3JSON(data.Origin);
            this.BasisX = new Sr2Vector3JSON(data.BasisX);
            this.BasisY = new Sr2Vector3JSON(data.BasisY);
            this.BasisZ = new Sr2Vector3JSON(data.BasisZ);
            this.Unknown0x4C = data.Unknown0x4C;
            this.Unknown0x54 = data.Unknown0x54;
            this.ModelIdx = data.ModelIdx;
            this.Unknown0x5C = data.Unknown0x5C;
        }
    }

    public struct Sr2ChunkObjectUnknown3JSON
    {
        public Single Unknown0x00 { get; set; }
        public Single Unknown0x04 { get; set; }
        public Single Unknown0x08 { get; set; }
        public UInt32 Unknown0x0C { get; set; }
        public Single Unknown0x10 { get; set; }
        public UInt32 Unknown0x14 { get; set; }
        public Single Unknown0x18 { get; set; }
        public Single Unknown0x1C { get; set; }
        public Single Unknown0x20 { get; set; }
        public UInt32 Unknown0x24 { get; set; }
        public Single Unknown0x28 { get; set; }
        public Single Unknown0x2C { get; set; }
        public Single Unknown0x30 { get; set; }
        public Single Unknown0x34 { get; set; }
        public Single Unknown0x38 { get; set; }
        public Single Unknown0x3C { get; set; }
        public Single Unknown0x40 { get; set; }
        public Single Unknown0x44 { get; set; }
        public Single Unknown0x48 { get; set; }
        public Single Unknown0x4C { get; set; }
        public Single Unknown0x50 { get; set; }
        public Single Unknown0x54 { get; set; }
        public Single Unknown0x58 { get; set; }
        public Single Unknown0x5C { get; set; }
        public UInt32 Unknown0x60 { get; set; }

        public Sr2ChunkObjectUnknown3JSON(Sr2ChunkObjectUnknown3 data) : this()
        {
            this.Unknown0x00 = data.Unknown0x00;
            this.Unknown0x04 = data.Unknown0x04;
            this.Unknown0x08 = data.Unknown0x08;
            this.Unknown0x0C = data.Unknown0x0C;
            this.Unknown0x10 = data.Unknown0x10;
            this.Unknown0x14 = data.Unknown0x14;
            this.Unknown0x18 = data.Unknown0x18;
            this.Unknown0x1C = data.Unknown0x1C;
            this.Unknown0x20 = data.Unknown0x20;
            this.Unknown0x24 = data.Unknown0x24;
            this.Unknown0x28 = data.Unknown0x28;
            this.Unknown0x2C = data.Unknown0x2C;
            this.Unknown0x30 = data.Unknown0x30;
            this.Unknown0x34 = data.Unknown0x34;
            this.Unknown0x38 = data.Unknown0x38;
            this.Unknown0x3C = data.Unknown0x3C;
            this.Unknown0x40 = data.Unknown0x40;
            this.Unknown0x44 = data.Unknown0x44;
            this.Unknown0x48 = data.Unknown0x48;
            this.Unknown0x4C = data.Unknown0x4C;
            this.Unknown0x50 = data.Unknown0x50;
            this.Unknown0x54 = data.Unknown0x54;
            this.Unknown0x58 = data.Unknown0x58;
            this.Unknown0x5C = data.Unknown0x5C;
            this.Unknown0x60 = data.Unknown0x60;
        }
    }

    public struct Sr2ChunkObjectUnknown4JSON
    {
        public Single Unknown0x00 { get; set; }
        public Single Unknown0x04 { get; set; }
        public Single Unknown0x08 { get; set; }
        public Single Unknown0x0C { get; set; }
        public Single Unknown0x10 { get; set; }
        public Single Unknown0x14 { get; set; }
        public Single Unknown0x18 { get; set; }
        public Single Unknown0x1C { get; set; }
        public Single Unknown0x20 { get; set; }
        public Single Unknown0x24 { get; set; }
        public Single Unknown0x28 { get; set; }
        public Single Unknown0x2C { get; set; }
        public Single Unknown0x30 { get; set; }

        public Sr2ChunkObjectUnknown4JSON(Sr2ChunkObjectUnknown4 data) : this()
        {
            this.Unknown0x00 = data.Unknown0x00;
            this.Unknown0x04 = data.Unknown0x04;
            this.Unknown0x08 = data.Unknown0x08;
            this.Unknown0x0C = data.Unknown0x0C;
            this.Unknown0x10 = data.Unknown0x10;
            this.Unknown0x14 = data.Unknown0x14;
            this.Unknown0x18 = data.Unknown0x18;
            this.Unknown0x1C = data.Unknown0x1C;
            this.Unknown0x20 = data.Unknown0x20;
            this.Unknown0x24 = data.Unknown0x24;
            this.Unknown0x28 = data.Unknown0x28;
            this.Unknown0x2C = data.Unknown0x2C;
            this.Unknown0x30 = data.Unknown0x30;
        }
    }

    // Object data 1
    public struct Sr2ChunkCityobjectJSON
    {
        public string Name { get; set; }
        public Sr2AABBJSON CullBox { get; set; }
        public Single CullDistance { get; set; }
        public UInt32 Unknown0x24 { get; set; }
        public Int16 Unknown0x2C { get; set; }
        public Int32 Flags { get; set; }
        public Int32 Unknown0x34 { get; set; }
        public Int32 Unknown0x38 { get; set; }
        public Int32 Unknown0x3C { get; set; }
        public UInt32 TransformIdx { get; set; }
        public UInt32 Unknown0x44 { get; set; }
        public UInt32 Unknown0x48 { get; set; }
        public UInt32 Unknown0x4C { get; set; }

        public Sr2ChunkCityobjectJSON(Sr2ChunkCityobject data) : this()
        {
            this.CullBox = new Sr2AABBJSON(data.CullBoxMin, data.CullBoxMax);
            this.CullDistance = data.CullDistance;
            this.Unknown0x24 = data.Unknown0x24;
            this.Unknown0x2C = data.Unknown0x2C;
            this.Flags = data.Flags;
            this.Unknown0x34 = data.Unknown0x34;
            this.Unknown0x38 = data.Unknown0x38;
            this.Unknown0x3C = data.Unknown0x3C;
            this.TransformIdx = data.TransformIdx;
            this.Unknown0x44 = data.Unknown0x44;
            this.Unknown0x48 = data.Unknown0x48;
            this.Unknown0x4C = data.Unknown0x4C;
        }
    }
}