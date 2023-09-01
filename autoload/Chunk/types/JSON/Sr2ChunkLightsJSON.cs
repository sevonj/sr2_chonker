using System;
using static Sr2ChunkLights;
using static Sr2GenericJSON;

public static class Sr2ChunkLightsJSON
{
	public struct Sr2ChunkLightsJSONRoot
	{
		public UInt32 Unknown0x04 { get; set; }
		public Sr2ChunkLightDataJSON[] Lights { get; set; }

		public Sr2ChunkLightsJSONRoot(Sr2ChunkLightHeader data) : this()
		{
			this.Unknown0x04 = data.Unknown0x04;
			this.Lights = new Sr2ChunkLightDataJSON[data.NumLights];
		}
	}

	public struct Sr2ChunkLightDataJSON
	{
		public string Name { get; set; }
		public string Flags { get; set; }
		public Sr2RGBJSON Color { get; set; }
		public UInt32 Unknown0x14 { get; set; }
		public UInt32 Unknown0x18 { get; set; }
		public UInt32 Unknown0x1c { get; set; }
		public Single[] UnknownFloats { get; set; }
		public Int32 Unknown0x28 { get; set; }
		public UInt32 Unknown0x30 { get; set; }
		public Single Unknown0x34 { get; set; }
		public Single Unknown0x38 { get; set; }
		public Sr2Vector3JSON Origin { get; set; }
		public Sr2Vector3JSON BasisX { get; set; }
		public Sr2Vector3JSON BasisY { get; set; }
		public Sr2Vector3JSON BasisZ { get; set; }
		public Single Unknown0x6c { get; set; }
		public Single Unknown0x70 { get; set; }
		public Single RadiusInner { get; set; }
		public Single RadiusOuter { get; set; }
		public Single RenderDistance { get; set; }
		public Int32 Parent { get; set; }
		public UInt32 Unknown0x88 { get; set; }
		public UInt32 Unknown0x8c { get; set; }
		public UInt32 Type { get; set; }

		public Sr2ChunkLightDataJSON(Sr2ChunkLightData data, string name, float[] FloatBlock) : this()
		{
			this.Name = name;
			this.Flags = data.Flags.ToString("X");
			this.Color = new Sr2RGBJSON(data.Color);
			this.Unknown0x14 = data.Unknown0x14;
			this.Unknown0x18 = data.Unknown0x18;
			this.Unknown0x1c = data.Unknown0x1c;
			this.UnknownFloats = FloatBlock;
			this.Unknown0x28 = data.Unknown0x28;
			this.Unknown0x30 = data.Unknown0x30;
			this.Unknown0x34 = data.Unknown0x34;
			this.Unknown0x38 = data.Unknown0x38;
			this.Origin = new Sr2Vector3JSON(data.Origin);
			this.BasisX = new Sr2Vector3JSON(data.BasisX);
			this.BasisY = new Sr2Vector3JSON(data.BasisY);
			this.BasisZ = new Sr2Vector3JSON(data.BasisZ);
			this.Unknown0x6c = data.Unknown0x6c;
			this.Unknown0x70 = data.Unknown0x70;
			this.RadiusInner = data.RadiusInner;
			this.RadiusOuter = data.RadiusOuter;
			this.RenderDistance = data.RenderDistance;
			this.Parent = data.Parent;
			this.Unknown0x88 = data.Unknown0x88;
			this.Unknown0x8c = data.Unknown0x8c;
			this.Type = data.Type;
		}
		public Sr2ChunkLightData ToBin()
		{
			Sr2ChunkLightData data = new Sr2ChunkLightData();
			data.Flags = UInt32.Parse(this.Flags, System.Globalization.NumberStyles.HexNumber);
			data.Color = this.Color.ToBin();
			data.Unknown0x14 = this.Unknown0x14;
			data.Unknown0x18 = this.Unknown0x18;
			data.Unknown0x1c = this.Unknown0x1c;
			data.NumUnknownFloats = (uint)this.UnknownFloats.Length;
			data.Uninitialized0x24 = -1;
			data.Unknown0x28 = this.Unknown0x28;
			data.Uninitialized0x2c = -1;
			data.Unknown0x30 = this.Unknown0x30;
			data.Unknown0x34 = this.Unknown0x34;
			data.Unknown0x38 = this.Unknown0x38;
			data.Origin = this.Origin.ToBin();
			data.BasisX = this.BasisX.ToBin();
			data.BasisY = this.BasisY.ToBin();
			data.BasisZ = this.BasisZ.ToBin();
			data.Unknown0x6c = this.Unknown0x6c;
			data.Unknown0x70 = this.Unknown0x70;
			data.RadiusInner = this.RadiusInner;
			data.RadiusOuter = this.RadiusOuter;
			data.RenderDistance = this.RenderDistance;
			data.Uninitialized0x80 = -1;
			data.Parent = this.Parent;
			data.Unknown0x88 = this.Unknown0x88;
			data.Unknown0x8c = this.Unknown0x8c;
			data.Type = this.Type;
			return data;
		}
	}

}
