using System;
using static Sr2Generic;

public static class Sr2GenericJSON
{
    public struct Sr2Vector3JSON
    {
        public Single X { get; set; }
        public Single Y { get; set; }
        public Single Z { get; set; }
        public Sr2Vector3JSON(float X, float Y, float Z) : this()
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public Sr2Vector3JSON(Sr2Vector3 vec) : this()
        {
            this.X = vec.X;
            this.Y = vec.Y;
            this.Z = vec.Z;
        }
        public Sr2Vector3 ToBin()
        {
            Sr2Vector3 vec = new Sr2Vector3();
            vec.X = this.X;
            vec.Y = this.Y;
            vec.Z = this.Z;
            return vec;
        }
    }
    public struct Sr2Vector4JSON
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }
        
        public Sr2Vector4JSON(Sr2Vector4 vec) : this()
        {
            this.X = vec.X;
            this.Y = vec.Y;
            this.Z = vec.Z;
            this.W = vec.W;
        }
        public Sr2Vector4 ToBin()
        {
            Sr2Vector4 vec = new Sr2Vector4();
            vec.X = this.X;
            vec.Y = this.Y;
            vec.Z = this.Z;
            vec.W = this.W;
            return vec;
        }
    }
    public struct Sr2RGBJSON
    {
        public Single R { get; set; }
        public Single G { get; set; }
        public Single B { get; set; }
        public Sr2RGBJSON(float R, float G, float B) : this()
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
        public Sr2RGBJSON(Sr2Vector3 vec) : this()
        {
            this.R = vec.X;
            this.G = vec.Y;
            this.B = vec.Z;
        }
        public Sr2Vector3 ToBin()
        {
            Sr2Vector3 vec = new Sr2Vector3();
            vec.X = this.R;
            vec.Y = this.G;
            vec.Z = this.B;
            return vec;
        }
    }
    public struct Sr2RGBAJSON
    {
        public float R { get; set; }
        public float G { get; set; }
        public float B { get; set; }
        public float A { get; set; }
        public Sr2RGBAJSON(float R, float Y, float Z, float W) : this()
        {
            this.R = R;
            this.G = Y;
            this.B = Z;
            this.A = W;
        }
        public Sr2RGBAJSON(Sr2Vector4 vec) : this()
        {
            this.R = vec.X;
            this.G = vec.Y;
            this.B = vec.Z;
            this.A = vec.W;
        }
        public Sr2Vector4 ToBin()
        {
            Sr2Vector4 vec = new Sr2Vector4();
            vec.X = this.R;
            vec.Y = this.G;
            vec.Z = this.B;
            vec.W = this.A;
            return vec;
        }
    }
    public struct Sr2AABBJSON
    {
        public float MinX { get; set; }
        public float MinY { get; set; }
        public float MinZ { get; set; }
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MaxZ { get; set; }
        
        public Sr2AABBJSON(Sr2Vector3 min, Sr2Vector3 max) : this()
        {
            this.MinX = min.X;
            this.MinY = min.Y;
            this.MinZ = min.Z;
            this.MaxX = max.X;
            this.MaxY = max.Y;
            this.MaxZ = max.Z;
        }


    }
}
