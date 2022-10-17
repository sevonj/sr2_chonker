// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using System.Collections.Generic;

namespace Kaitai
{

    /// <summary>
    /// - id: unknown33_count
    ///   type: u4
    /// 
    /// - id: pad33
    ///   size: 4 #contents: [0,0,0,0]
    /// - id: unknown33b_count
    ///   type: u4
    /// - id: pad33b
    ///   size: 4 #contents: [0,0,0,0]
    /// 
    /// - id: unknown33
    ///   size: 48
    ///   repeat: expr
    ///   repeat-expr: unknown33_count
    /// - id: unknown33b
    ///   type: u2
    ///   repeat: expr
    ///   repeat-expr: unknown33b_count
    /// </summary>
    public partial class Sr2CpuChunkPc : KaitaiStruct
    {
        public static Sr2CpuChunkPc FromFile(string fileName)
        {
            return new Sr2CpuChunkPc(new KaitaiStream(fileName));
        }

        public Sr2CpuChunkPc(KaitaiStream p__io, KaitaiStruct p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _magic = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Magic, new byte[] { 18, 202, 202, 187 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 18, 202, 202, 187 }, Magic, M_Io, "/seq/0");
            }
            _version = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Version, new byte[] { 121, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 121, 0, 0, 0 }, Version, M_Io, "/seq/1");
            }
            _header0x8 = m_io.ReadU4le();
            _header0xc = m_io.ReadU4le();
            _header0x10 = m_io.ReadU4le();
            _header0x14 = m_io.ReadBytes(128);
            _cityobjectCount = m_io.ReadU4le();
            _unknown23Count = m_io.ReadU4le();
            _header0x9c = m_io.ReadU4le();
            _header0xa0 = m_io.ReadU4le();
            _header0xa4 = m_io.ReadBytes(16);
            _meshMoverCount = m_io.ReadU4le();
            _unknown27Count = m_io.ReadU4le();
            _unknown28Count = m_io.ReadU4le();
            _unknown29Count = m_io.ReadU4le();
            _unknown30Count = m_io.ReadU4le();
            _unknown31Count = m_io.ReadU4le();
            _header0xcc = m_io.ReadU4le();
            _header0xd0 = m_io.ReadU4le();
            _headerWorldpos0X = m_io.ReadF4le();
            _headerWorldpos0Y = m_io.ReadF4le();
            _headerWorldpos0Z = m_io.ReadF4le();
            _header0xe0 = m_io.ReadF4le();
            _header0xe4 = m_io.ReadF4le();
            _header0xe8 = m_io.ReadF4le();
            _header0xec = m_io.ReadF4le();
            _header0xf0 = m_io.ReadU4le();
            _header0xf4 = m_io.ReadBytes(12);
            _textureCount = m_io.ReadU4le();
            _texturePadding = m_io.ReadBytes((TextureCount * 4));
            _textureNames = new List<string>();
            for (var i = 0; i < TextureCount; i++)
            {
                _textureNames.Add(System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true)));
            }
            _align = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _rendermodelCount = m_io.ReadU4le();
            _cityobjectPartCount = m_io.ReadU4le();
            _modelCount = m_io.ReadU4le();
            _unknown3Count = m_io.ReadU4le();
            _unknown4Count = m_io.ReadU4le();
            _align1 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _rendermodelUnk0s = new List<RendermodelUnk0>();
            for (var i = 0; i < RendermodelCount; i++)
            {
                _rendermodelUnk0s.Add(new RendermodelUnk0(m_io, this, m_root));
            }
            _align2 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _cityobjectParts = new List<CityobjectPart>();
            for (var i = 0; i < CityobjectPartCount; i++)
            {
                _cityobjectParts.Add(new CityobjectPart(m_io, this, m_root));
            }
            _align3 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknown3s = new List<Unknown3>();
            for (var i = 0; i < Unknown3Count; i++)
            {
                _unknown3s.Add(new Unknown3(m_io, this, m_root));
            }
            _align4 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknown4s = new List<Unknown4>();
            for (var i = 0; i < Unknown4Count; i++)
            {
                _unknown4s.Add(new Unknown4(m_io, this, m_root));
            }
            _align5 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unkWorldposCount = m_io.ReadU4le();
            _unkWorldpositions = new List<Vec3>();
            for (var i = 0; i < UnkWorldposCount; i++)
            {
                _unkWorldpositions.Add(new Vec3(m_io, this, m_root));
            }
            _unknown6Count = m_io.ReadU4le();
            _unknown6s = new List<byte[]>();
            for (var i = 0; i < Unknown6Count; i++)
            {
                _unknown6s.Add(m_io.ReadBytes(3));
            }
            _unknown7Count = m_io.ReadU4le();
            _unknown7s = new List<uint>();
            for (var i = 0; i < Unknown7Count; i++)
            {
                _unknown7s.Add(m_io.ReadU4le());
            }
            _unknown8Count = m_io.ReadU4le();
            _unknown8s = new List<byte[]>();
            for (var i = 0; i < Unknown8Count; i++)
            {
                _unknown8s.Add(m_io.ReadBytes(12));
            }
            _align6 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _havokMoppSize = m_io.ReadU4le();
            _align7 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _havokMopp = m_io.ReadBytes(HavokMoppSize);
            _align8 = m_io.ReadBytes(KaitaiStream.Mod((4 - M_Io.Pos), 4));
            _unknown10min = new Vec3(m_io, this, m_root);
            _unknown10max = new Vec3(m_io, this, m_root);
            _align9 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _modelHeaders = new List<ModelHeader>();
            for (var i = 0; i < ModelCount; i++)
            {
                _modelHeaders.Add(new ModelHeader(i, m_io, this, m_root));
            }
            _vertHeaders = new List<VertHeaderCont>();
            for (var i = 0; i < ModelCount; i++)
            {
                _vertHeaders.Add(new VertHeaderCont(ModelHeaders[i].VertHeaderCount, m_io, this, m_root));
            }
            _align10 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _physModels = new List<PhysModelBuffer>();
            for (var i = 0; i < ModelCount; i++)
            {
                _physModels.Add(new PhysModelBuffer(ModelHeaders[i].Type == 7, ModelHeaders[i].IndexCount, VertHeaders[i].VertHeader[0].VertCount, m_io, this, m_root));
            }
            _materialCount = m_io.ReadU4le();
            _align11 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _matShaderParamCount = m_io.ReadU4le();
            _padMat = m_io.ReadBytes(8);
            _matUnknown3Count = m_io.ReadU4le();
            _matUnknown1 = m_io.ReadU4le();
            _materials = new List<Material>();
            for (var i = 0; i < MaterialCount; i++)
            {
                _materials.Add(new Material(m_io, this, m_root));
            }
            _matBitflags = new List<MatBitflag>();
            for (var i = 0; i < MaterialCount; i++)
            {
                _matBitflags.Add(new MatBitflag(Materials[i].BitflagCount, m_io, this, m_root));
            }
            _matUnknown2 = new List<byte[]>();
            for (var i = 0; i < MaterialCount; i++)
            {
                _matUnknown2.Add(m_io.ReadBytes(16));
            }
            _align12 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _matShaderParams = new List<float>();
            for (var i = 0; i < MatShaderParamCount; i++)
            {
                _matShaderParams.Add(m_io.ReadF4le());
            }
            _matTextures = new List<MatTexCont>();
            for (var i = 0; i < MaterialCount; i++)
            {
                _matTextures.Add(new MatTexCont(m_io, this, m_root));
            }
            _matUnknown3s = new List<MatUnknown3>();
            for (var i = 0; i < MatUnknown3Count; i++)
            {
                _matUnknown3s.Add(new MatUnknown3(m_io, this, m_root));
            }
            _matUnknown3Unk2 = new List<byte[]>();
            for (var i = 0; i < MatUnknown3Count; i++)
            {
                _matUnknown3Unk2.Add(m_io.ReadBytes((MatUnknown3s[i].Unk2Count * 4)));
            }
            _rendermodels = new List<Rendermodel>();
            for (var i = 0; i < RendermodelCount; i++)
            {
                _rendermodels.Add(new Rendermodel(m_io, this, m_root));
            }
            _cityobjects = new List<Cityobject>();
            for (var i = 0; i < CityobjectCount; i++)
            {
                _cityobjects.Add(new Cityobject(m_io, this, m_root));
            }
            _cityobjectNames = new List<string>();
            for (var i = 0; i < CityobjectCount; i++)
            {
                _cityobjectNames.Add(System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true)));
            }
            _align13 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknownNamesLen = m_io.ReadU4le();
            _unknownNames = m_io.ReadBytes(UnknownNamesLen);
            _align14 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknown13Count = m_io.ReadU4le();
            _unknown13 = new List<uint>();
            for (var i = 0; i < Unknown13Count; i++)
            {
                _unknown13.Add(m_io.ReadU4le());
            }
            _align15 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _cdPad17Size = m_io.ReadU4le();
            _cdPad17 = m_io.ReadBytes(CdPad17Size);
            _align17 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknown18Count = m_io.ReadU4le();
            _unknown18s = new List<Unknown18>();
            for (var i = 0; i < Unknown18Count; i++)
            {
                _unknown18s.Add(new Unknown18(m_io, this, m_root));
            }
            _align18 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknown19Count = m_io.ReadU4le();
            _pad19 = m_io.ReadBytes((Unknown19Count * 28));
            _unknown19 = new List<float>();
            for (var i = 0; i < (Unknown19Count * 7); i++)
            {
                _unknown19.Add(m_io.ReadF4le());
            }
            _unknown20Count = m_io.ReadU4le();
            _pad20 = m_io.ReadBytes((Unknown20Count * 12));
            _unknown20 = new List<byte[]>();
            for (var i = 0; i < Unknown20Count; i++)
            {
                _unknown20.Add(m_io.ReadBytes(12));
            }
            _align20 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknown21Count = m_io.ReadU4le();
            _unknown21Pad = m_io.ReadBytes((Unknown21Count * 8));
            _unknown21 = new List<uint>();
            for (var i = 0; i < (Unknown21Count * 2); i++)
            {
                _unknown21.Add(m_io.ReadU4le());
            }
            _align21 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknown22Count = m_io.ReadU4le();
            _unknown22 = new List<uint>();
            for (var i = 0; i < Unknown22Count; i++)
            {
                _unknown22.Add(m_io.ReadU4le());
            }
            _align22 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _unknown23 = new List<float>();
            for (var i = 0; i < (Unknown23Count * 12); i++)
            {
                _unknown23.Add(m_io.ReadF4le());
            }
            _unknown24Count = m_io.ReadU4le();
            _pad24 = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Pad24, new byte[] { 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Pad24, M_Io, "/seq/112");
            }
            _unknown25Count = m_io.ReadU4le();
            _pad25 = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Pad25, new byte[] { 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Pad25, M_Io, "/seq/114");
            }
            _unknown24 = new List<byte[]>();
            for (var i = 0; i < Unknown24Count; i++)
            {
                _unknown24.Add(m_io.ReadBytes(48));
            }
            _unknown25 = new List<byte[]>();
            for (var i = 0; i < Unknown25Count; i++)
            {
                _unknown25.Add(m_io.ReadBytes(2));
            }
            _align25 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _meshMovers = new List<MeshMover>();
            for (var i = 0; i < MeshMoverCount; i++)
            {
                _meshMovers.Add(new MeshMover(m_io, this, m_root));
            }
            _unknown27 = new List<byte[]>();
            for (var i = 0; i < Unknown27Count; i++)
            {
                _unknown27.Add(m_io.ReadBytes(24));
            }
            _unknown28 = new List<byte[]>();
            for (var i = 0; i < Unknown28Count; i++)
            {
                _unknown28.Add(m_io.ReadBytes(36));
            }
            _unknown29 = new List<byte[]>();
            for (var i = 0; i < Unknown29Count; i++)
            {
                _unknown29.Add(m_io.ReadBytes(4));
            }
            _unknown30 = new List<byte[]>();
            for (var i = 0; i < Unknown30Count; i++)
            {
                _unknown30.Add(m_io.ReadBytes(12));
            }
            _unknown31 = new List<byte[]>();
            for (var i = 0; i < Unknown31Count; i++)
            {
                _unknown31.Add(m_io.ReadBytes(8));
            }
            _unknown32 = new List<byte[]>();
            for (var i = 0; i < Header0xf0; i++)
            {
                _unknown32.Add(m_io.ReadBytes(8));
            }
            _meshMoverNames = new List<MeshMoverName>();
            for (var i = 0; i < MeshMoverCount; i++)
            {
                _meshMoverNames.Add(new MeshMoverName(MeshMovers[i].StartCount, m_io, this, m_root));
            }
            _align32 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            _lightCount = m_io.ReadU4le();
            if (LightCount != 1212891981) {
                _lightSections = new LightSection(LightCount, m_io, this, m_root);
            }
        }
        public partial class MatUnknown3 : KaitaiStruct
        {
            public static MatUnknown3 FromFile(string fileName)
            {
                return new MatUnknown3(new KaitaiStream(fileName));
            }

            public MatUnknown3(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk = m_io.ReadBytes(8);
                _unk2Count = m_io.ReadU2le();
                _unk3 = m_io.ReadU2le();
                _pad = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Pad, new byte[] { 255, 255, 255, 255 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 255, 255, 255, 255 }, Pad, M_Io, "/types/mat_unknown3/seq/3");
                }
            }
            private byte[] _unk;
            private ushort _unk2Count;
            private ushort _unk3;
            private byte[] _pad;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public byte[] Unk { get { return _unk; } }
            public ushort Unk2Count { get { return _unk2Count; } }
            public ushort Unk3 { get { return _unk3; } }
            public byte[] Pad { get { return _pad; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Unknown4 : KaitaiStruct
        {
            public static Unknown4 FromFile(string fileName)
            {
                return new Unknown4(new KaitaiStream(fileName));
            }

            public Unknown4(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _float0 = m_io.ReadF4le();
                _float1 = m_io.ReadF4le();
                _float2 = m_io.ReadF4le();
                _float3 = m_io.ReadF4le();
                _float4 = m_io.ReadF4le();
                _float5 = m_io.ReadF4le();
                _float6 = m_io.ReadF4le();
                _float7 = m_io.ReadF4le();
                _float8 = m_io.ReadF4le();
                _float9 = m_io.ReadF4le();
                _floata = m_io.ReadF4le();
                _floatb = m_io.ReadF4le();
                _floatc = m_io.ReadF4le();
            }
            private float _float0;
            private float _float1;
            private float _float2;
            private float _float3;
            private float _float4;
            private float _float5;
            private float _float6;
            private float _float7;
            private float _float8;
            private float _float9;
            private float _floata;
            private float _floatb;
            private float _floatc;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public float Float0 { get { return _float0; } }
            public float Float1 { get { return _float1; } }
            public float Float2 { get { return _float2; } }
            public float Float3 { get { return _float3; } }
            public float Float4 { get { return _float4; } }
            public float Float5 { get { return _float5; } }
            public float Float6 { get { return _float6; } }
            public float Float7 { get { return _float7; } }
            public float Float8 { get { return _float8; } }
            public float Float9 { get { return _float9; } }
            public float Floata { get { return _floata; } }
            public float Floatb { get { return _floatb; } }
            public float Floatc { get { return _floatc; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class LightSection : KaitaiStruct
        {
            public LightSection(uint p_lightCount, KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _lightCount = p_lightCount;
                _read();
            }
            private void _read()
            {
                _unknown26b = m_io.ReadU4le();
                _lights = new List<Light>();
                for (var i = 0; i < LightCount; i++)
                {
                    _lights.Add(new Light(m_io, this, m_root));
                }
                _lightNames = new List<string>();
                for (var i = 0; i < LightCount; i++)
                {
                    _lightNames.Add(System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true)));
                }
                _alignLight = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
                _lightUnk2 = new List<byte[]>();
                for (var i = 0; i < LightCount; i++)
                {
                    _lightUnk2.Add(m_io.ReadBytes((Lights[i].Unk8 * 4)));
                }
                _alignLight2 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            }
            private uint _unknown26b;
            private List<Light> _lights;
            private List<string> _lightNames;
            private byte[] _alignLight;
            private List<byte[]> _lightUnk2;
            private byte[] _alignLight2;
            private uint _lightCount;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public uint Unknown26b { get { return _unknown26b; } }
            public List<Light> Lights { get { return _lights; } }
            public List<string> LightNames { get { return _lightNames; } }
            public byte[] AlignLight { get { return _alignLight; } }
            public List<byte[]> LightUnk2 { get { return _lightUnk2; } }
            public byte[] AlignLight2 { get { return _alignLight2; } }
            public uint LightCount { get { return _lightCount; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }

        /// <summary>
        /// Describes a vertex buffer.
        /// 
        /// unk2b_count:
        ///   Probs vertex normals or something
        ///   Each adds 2B to vert_size.
        ///   
        /// uv_count:
        ///   How many uv sets are used.
        ///   Each UV set adds 4B to vert_size
        ///   
        /// vert_size:
        ///   Size is 12B + above.
        ///   Physmodels don't use the above so size is always 12B.
        ///   
        /// unk: 
        ///   There's probably some meaning to this but it's 
        ///   _always_ the same. 0xffffffff00000000
        /// </summary>
        public partial class VertHeader : KaitaiStruct
        {
            public static VertHeader FromFile(string fileName)
            {
                return new VertHeader(new KaitaiStream(fileName));
            }

            public VertHeader(KaitaiStream p__io, Sr2CpuChunkPc.VertHeaderCont p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk2bCount = m_io.ReadU1();
                _uvCount = m_io.ReadU1();
                _vertSize = m_io.ReadU2le();
                _vertCount = m_io.ReadU4le();
                _pad = m_io.ReadBytes(8);
                if (!((KaitaiStream.ByteArrayCompare(Pad, new byte[] { 255, 255, 255, 255, 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 255, 255, 255, 255, 0, 0, 0, 0 }, Pad, M_Io, "/types/vert_header/seq/4");
                }
            }
            private byte _unk2bCount;
            private byte _uvCount;
            private ushort _vertSize;
            private uint _vertCount;
            private byte[] _pad;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc.VertHeaderCont m_parent;
            public byte Unk2bCount { get { return _unk2bCount; } }
            public byte UvCount { get { return _uvCount; } }
            public ushort VertSize { get { return _vertSize; } }
            public uint VertCount { get { return _vertCount; } }
            public byte[] Pad { get { return _pad; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc.VertHeaderCont M_Parent { get { return m_parent; } }
        }
        public partial class MeshMover : KaitaiStruct
        {
            public static MeshMover FromFile(string fileName)
            {
                return new MeshMover(new KaitaiStream(fileName));
            }

            public MeshMover(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk0 = m_io.ReadBytes(14);
                _startCount = m_io.ReadU2le();
                _unk1 = m_io.ReadBytes(12);
            }
            private byte[] _unk0;
            private ushort _startCount;
            private byte[] _unk1;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public byte[] Unk0 { get { return _unk0; } }
            public ushort StartCount { get { return _startCount; } }
            public byte[] Unk1 { get { return _unk1; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class RendermodelUnk0 : KaitaiStruct
        {
            public static RendermodelUnk0 FromFile(string fileName)
            {
                return new RendermodelUnk0(new KaitaiStream(fileName));
            }

            public RendermodelUnk0(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk0 = m_io.ReadU4le();
                _unk1 = m_io.ReadU4le();
                _unk2 = m_io.ReadU4le();
                _unk3 = m_io.ReadU4le();
                _unk4 = m_io.ReadU4le();
                _unk5 = m_io.ReadU4le();
            }
            private uint _unk0;
            private uint _unk1;
            private uint _unk2;
            private uint _unk3;
            private uint _unk4;
            private uint _unk5;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public uint Unk0 { get { return _unk0; } }
            public uint Unk1 { get { return _unk1; } }
            public uint Unk2 { get { return _unk2; } }
            public uint Unk3 { get { return _unk3; } }
            public uint Unk4 { get { return _unk4; } }
            public uint Unk5 { get { return _unk5; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }

        /// <summary>
        /// TODO: See todo in model_header.
        /// </summary>
        public partial class VertHeaderCont : KaitaiStruct
        {
            public VertHeaderCont(ushort p_vertHeaderCount, KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _vertHeaderCount = p_vertHeaderCount;
                _read();
            }
            private void _read()
            {
                _vertHeader = new List<VertHeader>();
                for (var i = 0; i < VertHeaderCount; i++)
                {
                    _vertHeader.Add(new VertHeader(m_io, this, m_root));
                }
            }
            private List<VertHeader> _vertHeader;
            private ushort _vertHeaderCount;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public List<VertHeader> VertHeader { get { return _vertHeader; } }
            public ushort VertHeaderCount { get { return _vertHeaderCount; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class MatTex : KaitaiStruct
        {
            public static MatTex FromFile(string fileName)
            {
                return new MatTex(new KaitaiStream(fileName));
            }

            public MatTex(KaitaiStream p__io, Sr2CpuChunkPc.MatTexCont p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _texId = m_io.ReadU2le();
                _texFlag = m_io.ReadU2le();
            }
            private ushort _texId;
            private ushort _texFlag;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc.MatTexCont m_parent;
            public ushort TexId { get { return _texId; } }
            public ushort TexFlag { get { return _texFlag; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc.MatTexCont M_Parent { get { return m_parent; } }
        }
        public partial class MeshMoverName : KaitaiStruct
        {
            public MeshMoverName(ushort p_startCount, KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _startCount = p_startCount;
                _read();
            }
            private void _read()
            {
                _name = System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                _startNames = new List<string>();
                for (var i = 0; i < StartCount; i++)
                {
                    _startNames.Add(System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true)));
                }
            }
            private string _name;
            private List<string> _startNames;
            private ushort _startCount;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public string Name { get { return _name; } }
            public List<string> StartNames { get { return _startNames; } }
            public ushort StartCount { get { return _startCount; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class PhysModelBuffer : KaitaiStruct
        {
            public PhysModelBuffer(bool p_isPhysmodel, uint p_indexCount, uint p_vertCount, KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _isPhysmodel = p_isPhysmodel;
                _indexCount = p_indexCount;
                _vertCount = p_vertCount;
                _read();
            }
            private void _read()
            {
                if (IsPhysmodel) {
                    _vbuf = new List<Vec3>();
                    for (var i = 0; i < VertCount; i++)
                    {
                        _vbuf.Add(new Vec3(m_io, this, m_root));
                    }
                }
                _align = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
                if (IsPhysmodel) {
                    _ibuf = new List<ushort>();
                    for (var i = 0; i < IndexCount; i++)
                    {
                        _ibuf.Add(m_io.ReadU2le());
                    }
                }
                _align1 = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
            }
            private List<Vec3> _vbuf;
            private byte[] _align;
            private List<ushort> _ibuf;
            private byte[] _align1;
            private bool _isPhysmodel;
            private uint _indexCount;
            private uint _vertCount;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public List<Vec3> Vbuf { get { return _vbuf; } }
            public byte[] Align { get { return _align; } }
            public List<ushort> Ibuf { get { return _ibuf; } }
            public byte[] Align1 { get { return _align1; } }
            public bool IsPhysmodel { get { return _isPhysmodel; } }
            public uint IndexCount { get { return _indexCount; } }
            public uint VertCount { get { return _vertCount; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class MatBitflag : KaitaiStruct
        {
            public MatBitflag(ushort p_size, KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _size = p_size;
                _read();
            }
            private void _read()
            {
                _data = m_io.ReadBytes((Size * 6));
                _align = m_io.ReadBytes(KaitaiStream.Mod((4 - (Size * 6)), 4));
            }
            private byte[] _data;
            private byte[] _align;
            private ushort _size;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public byte[] Data { get { return _data; } }
            public byte[] Align { get { return _align; } }
            public ushort Size { get { return _size; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class CityobjectPart : KaitaiStruct
        {
            public static CityobjectPart FromFile(string fileName)
            {
                return new CityobjectPart(new KaitaiStream(fileName));
            }

            public CityobjectPart(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _pos = new Vec3(m_io, this, m_root);
                _restOfTheTransform = m_io.ReadBytes(72);
                _unk0 = m_io.ReadU4le();
                _rendermodelId = m_io.ReadU4le();
                _unk1 = m_io.ReadU4le();
            }
            private Vec3 _pos;
            private byte[] _restOfTheTransform;
            private uint _unk0;
            private uint _rendermodelId;
            private uint _unk1;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public Vec3 Pos { get { return _pos; } }
            public byte[] RestOfTheTransform { get { return _restOfTheTransform; } }
            public uint Unk0 { get { return _unk0; } }
            public uint RendermodelId { get { return _rendermodelId; } }
            public uint Unk1 { get { return _unk1; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }

        /// <summary>
        /// TODO: Move mat_textures, mat_bitflags, etc. inside material.
        /// </summary>
        public partial class Material : KaitaiStruct
        {
            public static Material FromFile(string fileName)
            {
                return new Material(new KaitaiStream(fileName));
            }

            public Material(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk = m_io.ReadBytes(12);
                _bitflagCount = m_io.ReadU2le();
                _texCount = m_io.ReadU2le();
                _pad1 = m_io.ReadBytes(2);
                _unk2 = m_io.ReadU2le();
                _pad3 = m_io.ReadBytes(4);
            }
            private byte[] _unk;
            private ushort _bitflagCount;
            private ushort _texCount;
            private byte[] _pad1;
            private ushort _unk2;
            private byte[] _pad3;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public byte[] Unk { get { return _unk; } }
            public ushort BitflagCount { get { return _bitflagCount; } }
            public ushort TexCount { get { return _texCount; } }
            public byte[] Pad1 { get { return _pad1; } }
            public ushort Unk2 { get { return _unk2; } }
            public byte[] Pad3 { get { return _pad3; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Vec3 : KaitaiStruct
        {
            public static Vec3 FromFile(string fileName)
            {
                return new Vec3(new KaitaiStream(fileName));
            }

            public Vec3(KaitaiStream p__io, KaitaiStruct p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _x = m_io.ReadF4le();
                _y = m_io.ReadF4le();
                _z = m_io.ReadF4le();
            }
            private float _x;
            private float _y;
            private float _z;
            private Sr2CpuChunkPc m_root;
            private KaitaiStruct m_parent;
            public float X { get { return _x; } }
            public float Y { get { return _y; } }
            public float Z { get { return _z; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }

        /// <summary>
        /// Describes vertex/index offsets and counts in buffers.
        /// Rendermodels are divided to submeshes per material.
        /// Some models use what I named submesh2. This format is unknown
        /// and in such cases the normal submeshes are garbage too.
        /// 
        /// To be confirmed: bbox is a bounding box with min and max vert coords.
        /// Bbox usage is unknown.
        /// </summary>
        public partial class Rendermodel : KaitaiStruct
        {
            public static Rendermodel FromFile(string fileName)
            {
                return new Rendermodel(new KaitaiStream(fileName));
            }

            public Rendermodel(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _bboxMin = new Vec3(m_io, this, m_root);
                _bboxMax = new Vec3(m_io, this, m_root);
                _unk0 = m_io.ReadU4le();
                _unk1 = m_io.ReadU4le();
                _checkSubmeshes = m_io.ReadU4le();
                _checkSubmesh2 = m_io.ReadU4le();
                _align = m_io.ReadBytes(KaitaiStream.Mod((16 - M_Io.Pos), 16));
                if (CheckSubmeshes != 0) {
                    _submeshUnk0 = m_io.ReadU2le();
                }
                if (CheckSubmeshes != 0) {
                    _submeshCount = m_io.ReadU2le();
                }
                if (CheckSubmeshes != 0) {
                    _submeshUnkflag = m_io.ReadBytes(4);
                }
                if (CheckSubmeshes != 0) {
                    _submeshUnk2 = m_io.ReadU4le();
                }
                if (CheckSubmeshes != 0) {
                    _submeshPad1 = m_io.ReadBytes(4);
                    if (!((KaitaiStream.ByteArrayCompare(SubmeshPad1, new byte[] { 0, 0, 0, 0 }) == 0)))
                    {
                        throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, SubmeshPad1, M_Io, "/types/rendermodel/seq/11");
                    }
                }
                if (CheckSubmesh2 != 0) {
                    _submesh2Unk0 = m_io.ReadU2le();
                }
                if (CheckSubmesh2 != 0) {
                    _submesh2Count = m_io.ReadU2le();
                }
                if (CheckSubmesh2 != 0) {
                    _submesh2Unkflag = m_io.ReadBytes(4);
                }
                if (CheckSubmesh2 != 0) {
                    _submesh2Unk2 = m_io.ReadU4le();
                }
                if (CheckSubmesh2 != 0) {
                    _submesh2Pad1 = m_io.ReadBytes(4);
                    if (!((KaitaiStream.ByteArrayCompare(Submesh2Pad1, new byte[] { 0, 0, 0, 0 }) == 0)))
                    {
                        throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Submesh2Pad1, M_Io, "/types/rendermodel/seq/16");
                    }
                }
                _submeshes = new List<Submesh>();
                for (var i = 0; i < SubmeshCount; i++)
                {
                    _submeshes.Add(new Submesh(m_io, this, m_root));
                }
                _submesh2s = new List<Submesh2>();
                for (var i = 0; i < Submesh2Count; i++)
                {
                    _submesh2s.Add(new Submesh2(m_io, this, m_root));
                }
            }
            public partial class Submesh : KaitaiStruct
            {
                public static Submesh FromFile(string fileName)
                {
                    return new Submesh(new KaitaiStream(fileName));
                }

                public Submesh(KaitaiStream p__io, Sr2CpuChunkPc.Rendermodel p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _vertBufferId = m_io.ReadU4le();
                    _indexOffset = m_io.ReadU4le();
                    _vertOffset = m_io.ReadU4le();
                    _indexCount = m_io.ReadU2le();
                    _materialId = m_io.ReadU2le();
                }
                private uint _vertBufferId;
                private uint _indexOffset;
                private uint _vertOffset;
                private ushort _indexCount;
                private ushort _materialId;
                private Sr2CpuChunkPc m_root;
                private Sr2CpuChunkPc.Rendermodel m_parent;
                public uint VertBufferId { get { return _vertBufferId; } }
                public uint IndexOffset { get { return _indexOffset; } }
                public uint VertOffset { get { return _vertOffset; } }
                public ushort IndexCount { get { return _indexCount; } }
                public ushort MaterialId { get { return _materialId; } }
                public Sr2CpuChunkPc M_Root { get { return m_root; } }
                public Sr2CpuChunkPc.Rendermodel M_Parent { get { return m_parent; } }
            }
            public partial class Submesh2 : KaitaiStruct
            {
                public static Submesh2 FromFile(string fileName)
                {
                    return new Submesh2(new KaitaiStream(fileName));
                }

                public Submesh2(KaitaiStream p__io, Sr2CpuChunkPc.Rendermodel p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _unused = m_io.ReadBytes(4);
                    if (!((KaitaiStream.ByteArrayCompare(Unused, new byte[] { 0, 0, 0, 0 }) == 0)))
                    {
                        throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unused, M_Io, "/types/rendermodel/types/submesh2/seq/0");
                    }
                    _unknown = m_io.ReadBytes(4);
                    _unused2 = m_io.ReadBytes(4);
                    if (!((KaitaiStream.ByteArrayCompare(Unused2, new byte[] { 0, 0, 0, 0 }) == 0)))
                    {
                        throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unused2, M_Io, "/types/rendermodel/types/submesh2/seq/2");
                    }
                    _indexCount = m_io.ReadU2le();
                    _materialId = m_io.ReadU2le();
                }
                private byte[] _unused;
                private byte[] _unknown;
                private byte[] _unused2;
                private ushort _indexCount;
                private ushort _materialId;
                private Sr2CpuChunkPc m_root;
                private Sr2CpuChunkPc.Rendermodel m_parent;
                public byte[] Unused { get { return _unused; } }
                public byte[] Unknown { get { return _unknown; } }
                public byte[] Unused2 { get { return _unused2; } }
                public ushort IndexCount { get { return _indexCount; } }
                public ushort MaterialId { get { return _materialId; } }
                public Sr2CpuChunkPc M_Root { get { return m_root; } }
                public Sr2CpuChunkPc.Rendermodel M_Parent { get { return m_parent; } }
            }
            private Vec3 _bboxMin;
            private Vec3 _bboxMax;
            private uint _unk0;
            private uint _unk1;
            private uint _checkSubmeshes;
            private uint _checkSubmesh2;
            private byte[] _align;
            private ushort? _submeshUnk0;
            private ushort? _submeshCount;
            private byte[] _submeshUnkflag;
            private uint? _submeshUnk2;
            private byte[] _submeshPad1;
            private ushort? _submesh2Unk0;
            private ushort? _submesh2Count;
            private byte[] _submesh2Unkflag;
            private uint? _submesh2Unk2;
            private byte[] _submesh2Pad1;
            private List<Submesh> _submeshes;
            private List<Submesh2> _submesh2s;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public Vec3 BboxMin { get { return _bboxMin; } }
            public Vec3 BboxMax { get { return _bboxMax; } }
            public uint Unk0 { get { return _unk0; } }
            public uint Unk1 { get { return _unk1; } }
            public uint CheckSubmeshes { get { return _checkSubmeshes; } }
            public uint CheckSubmesh2 { get { return _checkSubmesh2; } }
            public byte[] Align { get { return _align; } }
            public ushort? SubmeshUnk0 { get { return _submeshUnk0; } }
            public ushort? SubmeshCount { get { return _submeshCount; } }
            public byte[] SubmeshUnkflag { get { return _submeshUnkflag; } }
            public uint? SubmeshUnk2 { get { return _submeshUnk2; } }
            public byte[] SubmeshPad1 { get { return _submeshPad1; } }
            public ushort? Submesh2Unk0 { get { return _submesh2Unk0; } }
            public ushort? Submesh2Count { get { return _submesh2Count; } }
            public byte[] Submesh2Unkflag { get { return _submesh2Unkflag; } }
            public uint? Submesh2Unk2 { get { return _submesh2Unk2; } }
            public byte[] Submesh2Pad1 { get { return _submesh2Pad1; } }
            public List<Submesh> Submeshes { get { return _submeshes; } }
            public List<Submesh2> Submesh2s { get { return _submesh2s; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Unknown3 : KaitaiStruct
        {
            public static Unknown3 FromFile(string fileName)
            {
                return new Unknown3(new KaitaiStream(fileName));
            }

            public Unknown3(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _float0 = m_io.ReadF4le();
                _float1 = m_io.ReadF4le();
                _float2 = m_io.ReadF4le();
                _float3 = m_io.ReadF4le();
                _float4 = m_io.ReadF4le();
                _float5 = m_io.ReadF4le();
                _float6 = m_io.ReadF4le();
                _float7 = m_io.ReadF4le();
                _float8 = m_io.ReadF4le();
                _float9 = m_io.ReadF4le();
                _floata = m_io.ReadF4le();
                _floatb = m_io.ReadF4le();
                _floatc = m_io.ReadF4le();
                _floatd = m_io.ReadF4le();
                _floate = m_io.ReadF4le();
                _floatf = m_io.ReadF4le();
                _float10 = m_io.ReadF4le();
                _float11 = m_io.ReadF4le();
                _float12 = m_io.ReadF4le();
                _float13 = m_io.ReadF4le();
                _float14 = m_io.ReadF4le();
                _float15 = m_io.ReadF4le();
                _float16 = m_io.ReadF4le();
                _float17 = m_io.ReadF4le();
                _float18 = m_io.ReadF4le();
            }
            private float _float0;
            private float _float1;
            private float _float2;
            private float _float3;
            private float _float4;
            private float _float5;
            private float _float6;
            private float _float7;
            private float _float8;
            private float _float9;
            private float _floata;
            private float _floatb;
            private float _floatc;
            private float _floatd;
            private float _floate;
            private float _floatf;
            private float _float10;
            private float _float11;
            private float _float12;
            private float _float13;
            private float _float14;
            private float _float15;
            private float _float16;
            private float _float17;
            private float _float18;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public float Float0 { get { return _float0; } }
            public float Float1 { get { return _float1; } }
            public float Float2 { get { return _float2; } }
            public float Float3 { get { return _float3; } }
            public float Float4 { get { return _float4; } }
            public float Float5 { get { return _float5; } }
            public float Float6 { get { return _float6; } }
            public float Float7 { get { return _float7; } }
            public float Float8 { get { return _float8; } }
            public float Float9 { get { return _float9; } }
            public float Floata { get { return _floata; } }
            public float Floatb { get { return _floatb; } }
            public float Floatc { get { return _floatc; } }
            public float Floatd { get { return _floatd; } }
            public float Floate { get { return _floate; } }
            public float Floatf { get { return _floatf; } }
            public float Float10 { get { return _float10; } }
            public float Float11 { get { return _float11; } }
            public float Float12 { get { return _float12; } }
            public float Float13 { get { return _float13; } }
            public float Float14 { get { return _float14; } }
            public float Float15 { get { return _float15; } }
            public float Float16 { get { return _float16; } }
            public float Float17 { get { return _float17; } }
            public float Float18 { get { return _float18; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }

        /// <summary>
        /// bbox is used for culling.
        /// </summary>
        public partial class Cityobject : KaitaiStruct
        {
            public static Cityobject FromFile(string fileName)
            {
                return new Cityobject(new KaitaiStream(fileName));
            }

            public Cityobject(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _bboxMin = new Vec3(m_io, this, m_root);
                _pad = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Pad, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Pad, M_Io, "/types/cityobject/seq/1");
                }
                _bboxMax = new Vec3(m_io, this, m_root);
                _cullDistance = m_io.ReadF4le();
                _pad1 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Pad1, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Pad1, M_Io, "/types/cityobject/seq/4");
                }
                _unk0 = m_io.ReadBytes(4);
                _pad2 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Pad2, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Pad2, M_Io, "/types/cityobject/seq/6");
                }
                _unk1 = m_io.ReadU2le();
                _pad3 = m_io.ReadBytes(2);
                if (!((KaitaiStream.ByteArrayCompare(Pad3, new byte[] { 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0 }, Pad3, M_Io, "/types/cityobject/seq/8");
                }
                _flags = m_io.ReadBytes(4);
                _unk3 = m_io.ReadBytes(4);
                _pad4 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Pad4, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Pad4, M_Io, "/types/cityobject/seq/11");
                }
                _unk4 = m_io.ReadBytes(4);
                _cityobjectPartId = m_io.ReadU4le();
                _unk5 = m_io.ReadU4le();
                _unk6 = m_io.ReadBytes(4);
                _unk7 = m_io.ReadBytes(4);
            }
            private Vec3 _bboxMin;
            private byte[] _pad;
            private Vec3 _bboxMax;
            private float _cullDistance;
            private byte[] _pad1;
            private byte[] _unk0;
            private byte[] _pad2;
            private ushort _unk1;
            private byte[] _pad3;
            private byte[] _flags;
            private byte[] _unk3;
            private byte[] _pad4;
            private byte[] _unk4;
            private uint _cityobjectPartId;
            private uint _unk5;
            private byte[] _unk6;
            private byte[] _unk7;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public Vec3 BboxMin { get { return _bboxMin; } }
            public byte[] Pad { get { return _pad; } }
            public Vec3 BboxMax { get { return _bboxMax; } }
            public float CullDistance { get { return _cullDistance; } }
            public byte[] Pad1 { get { return _pad1; } }
            public byte[] Unk0 { get { return _unk0; } }
            public byte[] Pad2 { get { return _pad2; } }
            public ushort Unk1 { get { return _unk1; } }
            public byte[] Pad3 { get { return _pad3; } }
            public byte[] Flags { get { return _flags; } }
            public byte[] Unk3 { get { return _unk3; } }
            public byte[] Pad4 { get { return _pad4; } }
            public byte[] Unk4 { get { return _unk4; } }
            public uint CityobjectPartId { get { return _cityobjectPartId; } }
            public uint Unk5 { get { return _unk5; } }
            public byte[] Unk6 { get { return _unk6; } }
            public byte[] Unk7 { get { return _unk7; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Light : KaitaiStruct
        {
            public static Light FromFile(string fileName)
            {
                return new Light(new KaitaiStream(fileName));
            }

            public Light(KaitaiStream p__io, Sr2CpuChunkPc.LightSection p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk0 = m_io.ReadU4le();
                _unk1 = m_io.ReadU4le();
                _unk2 = m_io.ReadF4le();
                _unk3 = m_io.ReadF4le();
                _unk4 = m_io.ReadF4le();
                _unk5 = m_io.ReadU4le();
                _unk6 = m_io.ReadU4le();
                _unk7 = m_io.ReadU4le();
                _unk8 = m_io.ReadU4le();
                _unk9 = m_io.ReadU4le();
                _unk10 = m_io.ReadU4le();
                _unk11 = m_io.ReadU4le();
                _unk12 = m_io.ReadU4le();
                _unk13 = m_io.ReadF4le();
                _unk14 = m_io.ReadU4le();
                _pos = new Vec3(m_io, this, m_root);
                _unk18 = m_io.ReadF4le();
                _unk19 = m_io.ReadF4le();
                _unk20 = m_io.ReadU4le();
                _unk21 = m_io.ReadU4le();
                _unk22 = m_io.ReadF4le();
                _unk23 = m_io.ReadU4le();
                _unk24 = m_io.ReadU4le();
                _unk25 = m_io.ReadU4le();
                _unk26 = m_io.ReadU4le();
                _unk27 = m_io.ReadU4le();
                _unk28 = m_io.ReadU4le();
                _unk29 = m_io.ReadU4le();
                _unk30 = m_io.ReadU4le();
                _unk31 = m_io.ReadU4le();
                _unk32 = m_io.ReadU4le();
                _unk33 = m_io.ReadU4le();
                _unk34 = m_io.ReadU4le();
                _unk35 = m_io.ReadU4le();
                _unk36 = m_io.ReadU4le();
            }
            private uint _unk0;
            private uint _unk1;
            private float _unk2;
            private float _unk3;
            private float _unk4;
            private uint _unk5;
            private uint _unk6;
            private uint _unk7;
            private uint _unk8;
            private uint _unk9;
            private uint _unk10;
            private uint _unk11;
            private uint _unk12;
            private float _unk13;
            private uint _unk14;
            private Vec3 _pos;
            private float _unk18;
            private float _unk19;
            private uint _unk20;
            private uint _unk21;
            private float _unk22;
            private uint _unk23;
            private uint _unk24;
            private uint _unk25;
            private uint _unk26;
            private uint _unk27;
            private uint _unk28;
            private uint _unk29;
            private uint _unk30;
            private uint _unk31;
            private uint _unk32;
            private uint _unk33;
            private uint _unk34;
            private uint _unk35;
            private uint _unk36;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc.LightSection m_parent;
            public uint Unk0 { get { return _unk0; } }
            public uint Unk1 { get { return _unk1; } }
            public float Unk2 { get { return _unk2; } }
            public float Unk3 { get { return _unk3; } }
            public float Unk4 { get { return _unk4; } }
            public uint Unk5 { get { return _unk5; } }
            public uint Unk6 { get { return _unk6; } }
            public uint Unk7 { get { return _unk7; } }
            public uint Unk8 { get { return _unk8; } }
            public uint Unk9 { get { return _unk9; } }
            public uint Unk10 { get { return _unk10; } }
            public uint Unk11 { get { return _unk11; } }
            public uint Unk12 { get { return _unk12; } }
            public float Unk13 { get { return _unk13; } }
            public uint Unk14 { get { return _unk14; } }
            public Vec3 Pos { get { return _pos; } }
            public float Unk18 { get { return _unk18; } }
            public float Unk19 { get { return _unk19; } }
            public uint Unk20 { get { return _unk20; } }
            public uint Unk21 { get { return _unk21; } }
            public float Unk22 { get { return _unk22; } }
            public uint Unk23 { get { return _unk23; } }
            public uint Unk24 { get { return _unk24; } }
            public uint Unk25 { get { return _unk25; } }
            public uint Unk26 { get { return _unk26; } }
            public uint Unk27 { get { return _unk27; } }
            public uint Unk28 { get { return _unk28; } }
            public uint Unk29 { get { return _unk29; } }
            public uint Unk30 { get { return _unk30; } }
            public uint Unk31 { get { return _unk31; } }
            public uint Unk32 { get { return _unk32; } }
            public uint Unk33 { get { return _unk33; } }
            public uint Unk34 { get { return _unk34; } }
            public uint Unk35 { get { return _unk35; } }
            public uint Unk36 { get { return _unk36; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc.LightSection M_Parent { get { return m_parent; } }
        }
        public partial class MatTexCont : KaitaiStruct
        {
            public static MatTexCont FromFile(string fileName)
            {
                return new MatTexCont(new KaitaiStream(fileName));
            }

            public MatTexCont(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _texData = new List<MatTex>();
                for (var i = 0; i < 16; i++)
                {
                    _texData.Add(new MatTex(m_io, this, m_root));
                }
            }
            private List<MatTex> _texData;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public List<MatTex> TexData { get { return _texData; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Unknown18 : KaitaiStruct
        {
            public static Unknown18 FromFile(string fileName)
            {
                return new Unknown18(new KaitaiStream(fileName));
            }

            public Unknown18(KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk0 = m_io.ReadU4le();
                _count0 = m_io.ReadU4le();
                _pad0 = m_io.ReadBytes((Count0 * 12));
                _data0 = new List<uint>();
                for (var i = 0; i < Count0; i++)
                {
                    _data0.Add(m_io.ReadU4le());
                }
                _count1 = m_io.ReadU4le();
                _pad1 = m_io.ReadBytes((Count1 * 12));
                _data1 = new List<uint>();
                for (var i = 0; i < Count1; i++)
                {
                    _data1.Add(m_io.ReadU4le());
                }
                _count2 = m_io.ReadU4le();
                _pad2 = m_io.ReadBytes((Count2 * 12));
                _data2 = new List<uint>();
                for (var i = 0; i < Count2; i++)
                {
                    _data2.Add(m_io.ReadU4le());
                }
            }
            private uint _unk0;
            private uint _count0;
            private byte[] _pad0;
            private List<uint> _data0;
            private uint _count1;
            private byte[] _pad1;
            private List<uint> _data1;
            private uint _count2;
            private byte[] _pad2;
            private List<uint> _data2;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public uint Unk0 { get { return _unk0; } }
            public uint Count0 { get { return _count0; } }
            public byte[] Pad0 { get { return _pad0; } }
            public List<uint> Data0 { get { return _data0; } }
            public uint Count1 { get { return _count1; } }
            public byte[] Pad1 { get { return _pad1; } }
            public List<uint> Data1 { get { return _data1; } }
            public uint Count2 { get { return _count2; } }
            public byte[] Pad2 { get { return _pad2; } }
            public List<uint> Data2 { get { return _data2; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }

        /// <summary>
        /// A model header will talk about one of these two of things:
        /// 
        /// - A physmodel (type 7)
        ///   Each physmodel has a separate buffer and therefore each has its own
        ///   model_header. The buffers are stored in this file right after vert
        ///   headers.
        /// 
        /// - Rendermodels (type 0)
        ///   All buffers used for rendermodels are under this header. There is
        ///   exactly one rendermodel header per chunk (or none?).
        ///   Contains multiple vert_headers: one for each rendermodel vert buffer.
        ///   Buffers are stored in g_chunk file.
        ///   This does not tell how to get individual rendermodels. See rendermodel
        ///   type for that.
        ///   
        /// TODO: organize vert_headers in here somehow and get rid of vert_header_cont.
        /// </summary>
        public partial class ModelHeader : KaitaiStruct
        {
            public ModelHeader(int p_i, KaitaiStream p__io, Sr2CpuChunkPc p__parent = null, Sr2CpuChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _i = p_i;
                _read();
            }
            private void _read()
            {
                _type = m_io.ReadU2le();
                _vertHeaderCount = m_io.ReadU2le();
                _indexCount = m_io.ReadU4le();
                _unk = m_io.ReadBytes(12);
            }
            private ushort _type;
            private ushort _vertHeaderCount;
            private uint _indexCount;
            private byte[] _unk;
            private int _i;
            private Sr2CpuChunkPc m_root;
            private Sr2CpuChunkPc m_parent;
            public ushort Type { get { return _type; } }
            public ushort VertHeaderCount { get { return _vertHeaderCount; } }
            public uint IndexCount { get { return _indexCount; } }
            public byte[] Unk { get { return _unk; } }
            public int I { get { return _i; } }
            public Sr2CpuChunkPc M_Root { get { return m_root; } }
            public Sr2CpuChunkPc M_Parent { get { return m_parent; } }
        }
        private byte[] _magic;
        private byte[] _version;
        private uint _header0x8;
        private uint _header0xc;
        private uint _header0x10;
        private byte[] _header0x14;
        private uint _cityobjectCount;
        private uint _unknown23Count;
        private uint _header0x9c;
        private uint _header0xa0;
        private byte[] _header0xa4;
        private uint _meshMoverCount;
        private uint _unknown27Count;
        private uint _unknown28Count;
        private uint _unknown29Count;
        private uint _unknown30Count;
        private uint _unknown31Count;
        private uint _header0xcc;
        private uint _header0xd0;
        private float _headerWorldpos0X;
        private float _headerWorldpos0Y;
        private float _headerWorldpos0Z;
        private float _header0xe0;
        private float _header0xe4;
        private float _header0xe8;
        private float _header0xec;
        private uint _header0xf0;
        private byte[] _header0xf4;
        private uint _textureCount;
        private byte[] _texturePadding;
        private List<string> _textureNames;
        private byte[] _align;
        private uint _rendermodelCount;
        private uint _cityobjectPartCount;
        private uint _modelCount;
        private uint _unknown3Count;
        private uint _unknown4Count;
        private byte[] _align1;
        private List<RendermodelUnk0> _rendermodelUnk0s;
        private byte[] _align2;
        private List<CityobjectPart> _cityobjectParts;
        private byte[] _align3;
        private List<Unknown3> _unknown3s;
        private byte[] _align4;
        private List<Unknown4> _unknown4s;
        private byte[] _align5;
        private uint _unkWorldposCount;
        private List<Vec3> _unkWorldpositions;
        private uint _unknown6Count;
        private List<byte[]> _unknown6s;
        private uint _unknown7Count;
        private List<uint> _unknown7s;
        private uint _unknown8Count;
        private List<byte[]> _unknown8s;
        private byte[] _align6;
        private uint _havokMoppSize;
        private byte[] _align7;
        private byte[] _havokMopp;
        private byte[] _align8;
        private Vec3 _unknown10min;
        private Vec3 _unknown10max;
        private byte[] _align9;
        private List<ModelHeader> _modelHeaders;
        private List<VertHeaderCont> _vertHeaders;
        private byte[] _align10;
        private List<PhysModelBuffer> _physModels;
        private uint _materialCount;
        private byte[] _align11;
        private uint _matShaderParamCount;
        private byte[] _padMat;
        private uint _matUnknown3Count;
        private uint _matUnknown1;
        private List<Material> _materials;
        private List<MatBitflag> _matBitflags;
        private List<byte[]> _matUnknown2;
        private byte[] _align12;
        private List<float> _matShaderParams;
        private List<MatTexCont> _matTextures;
        private List<MatUnknown3> _matUnknown3s;
        private List<byte[]> _matUnknown3Unk2;
        private List<Rendermodel> _rendermodels;
        private List<Cityobject> _cityobjects;
        private List<string> _cityobjectNames;
        private byte[] _align13;
        private uint _unknownNamesLen;
        private byte[] _unknownNames;
        private byte[] _align14;
        private uint _unknown13Count;
        private List<uint> _unknown13;
        private byte[] _align15;
        private uint _cdPad17Size;
        private byte[] _cdPad17;
        private byte[] _align17;
        private uint _unknown18Count;
        private List<Unknown18> _unknown18s;
        private byte[] _align18;
        private uint _unknown19Count;
        private byte[] _pad19;
        private List<float> _unknown19;
        private uint _unknown20Count;
        private byte[] _pad20;
        private List<byte[]> _unknown20;
        private byte[] _align20;
        private uint _unknown21Count;
        private byte[] _unknown21Pad;
        private List<uint> _unknown21;
        private byte[] _align21;
        private uint _unknown22Count;
        private List<uint> _unknown22;
        private byte[] _align22;
        private List<float> _unknown23;
        private uint _unknown24Count;
        private byte[] _pad24;
        private uint _unknown25Count;
        private byte[] _pad25;
        private List<byte[]> _unknown24;
        private List<byte[]> _unknown25;
        private byte[] _align25;
        private List<MeshMover> _meshMovers;
        private List<byte[]> _unknown27;
        private List<byte[]> _unknown28;
        private List<byte[]> _unknown29;
        private List<byte[]> _unknown30;
        private List<byte[]> _unknown31;
        private List<byte[]> _unknown32;
        private List<MeshMoverName> _meshMoverNames;
        private byte[] _align32;
        private uint _lightCount;
        private LightSection _lightSections;
        private Sr2CpuChunkPc m_root;
        private KaitaiStruct m_parent;
        public byte[] Magic { get { return _magic; } }
        public byte[] Version { get { return _version; } }
        public uint Header0x8 { get { return _header0x8; } }
        public uint Header0xc { get { return _header0xc; } }
        public uint Header0x10 { get { return _header0x10; } }
        public byte[] Header0x14 { get { return _header0x14; } }
        public uint CityobjectCount { get { return _cityobjectCount; } }
        public uint Unknown23Count { get { return _unknown23Count; } }
        public uint Header0x9c { get { return _header0x9c; } }
        public uint Header0xa0 { get { return _header0xa0; } }
        public byte[] Header0xa4 { get { return _header0xa4; } }
        public uint MeshMoverCount { get { return _meshMoverCount; } }
        public uint Unknown27Count { get { return _unknown27Count; } }
        public uint Unknown28Count { get { return _unknown28Count; } }
        public uint Unknown29Count { get { return _unknown29Count; } }
        public uint Unknown30Count { get { return _unknown30Count; } }
        public uint Unknown31Count { get { return _unknown31Count; } }
        public uint Header0xcc { get { return _header0xcc; } }
        public uint Header0xd0 { get { return _header0xd0; } }
        public float HeaderWorldpos0X { get { return _headerWorldpos0X; } }
        public float HeaderWorldpos0Y { get { return _headerWorldpos0Y; } }
        public float HeaderWorldpos0Z { get { return _headerWorldpos0Z; } }
        public float Header0xe0 { get { return _header0xe0; } }
        public float Header0xe4 { get { return _header0xe4; } }
        public float Header0xe8 { get { return _header0xe8; } }
        public float Header0xec { get { return _header0xec; } }
        public uint Header0xf0 { get { return _header0xf0; } }
        public byte[] Header0xf4 { get { return _header0xf4; } }
        public uint TextureCount { get { return _textureCount; } }
        public byte[] TexturePadding { get { return _texturePadding; } }
        public List<string> TextureNames { get { return _textureNames; } }
        public byte[] Align { get { return _align; } }
        public uint RendermodelCount { get { return _rendermodelCount; } }
        public uint CityobjectPartCount { get { return _cityobjectPartCount; } }
        public uint ModelCount { get { return _modelCount; } }
        public uint Unknown3Count { get { return _unknown3Count; } }
        public uint Unknown4Count { get { return _unknown4Count; } }
        public byte[] Align1 { get { return _align1; } }
        public List<RendermodelUnk0> RendermodelUnk0s { get { return _rendermodelUnk0s; } }
        public byte[] Align2 { get { return _align2; } }
        public List<CityobjectPart> CityobjectParts { get { return _cityobjectParts; } }
        public byte[] Align3 { get { return _align3; } }
        public List<Unknown3> Unknown3s { get { return _unknown3s; } }
        public byte[] Align4 { get { return _align4; } }
        public List<Unknown4> Unknown4s { get { return _unknown4s; } }
        public byte[] Align5 { get { return _align5; } }
        public uint UnkWorldposCount { get { return _unkWorldposCount; } }
        public List<Vec3> UnkWorldpositions { get { return _unkWorldpositions; } }
        public uint Unknown6Count { get { return _unknown6Count; } }
        public List<byte[]> Unknown6s { get { return _unknown6s; } }
        public uint Unknown7Count { get { return _unknown7Count; } }
        public List<uint> Unknown7s { get { return _unknown7s; } }
        public uint Unknown8Count { get { return _unknown8Count; } }
        public List<byte[]> Unknown8s { get { return _unknown8s; } }
        public byte[] Align6 { get { return _align6; } }
        public uint HavokMoppSize { get { return _havokMoppSize; } }
        public byte[] Align7 { get { return _align7; } }
        public byte[] HavokMopp { get { return _havokMopp; } }
        public byte[] Align8 { get { return _align8; } }
        public Vec3 Unknown10min { get { return _unknown10min; } }
        public Vec3 Unknown10max { get { return _unknown10max; } }
        public byte[] Align9 { get { return _align9; } }
        public List<ModelHeader> ModelHeaders { get { return _modelHeaders; } }
        public List<VertHeaderCont> VertHeaders { get { return _vertHeaders; } }
        public byte[] Align10 { get { return _align10; } }
        public List<PhysModelBuffer> PhysModels { get { return _physModels; } }
        public uint MaterialCount { get { return _materialCount; } }
        public byte[] Align11 { get { return _align11; } }
        public uint MatShaderParamCount { get { return _matShaderParamCount; } }
        public byte[] PadMat { get { return _padMat; } }
        public uint MatUnknown3Count { get { return _matUnknown3Count; } }
        public uint MatUnknown1 { get { return _matUnknown1; } }
        public List<Material> Materials { get { return _materials; } }
        public List<MatBitflag> MatBitflags { get { return _matBitflags; } }
        public List<byte[]> MatUnknown2 { get { return _matUnknown2; } }
        public byte[] Align12 { get { return _align12; } }
        public List<float> MatShaderParams { get { return _matShaderParams; } }
        public List<MatTexCont> MatTextures { get { return _matTextures; } }
        public List<MatUnknown3> MatUnknown3s { get { return _matUnknown3s; } }
        public List<byte[]> MatUnknown3Unk2 { get { return _matUnknown3Unk2; } }
        public List<Rendermodel> Rendermodels { get { return _rendermodels; } }
        public List<Cityobject> Cityobjects { get { return _cityobjects; } }
        public List<string> CityobjectNames { get { return _cityobjectNames; } }
        public byte[] Align13 { get { return _align13; } }
        public uint UnknownNamesLen { get { return _unknownNamesLen; } }
        public byte[] UnknownNames { get { return _unknownNames; } }
        public byte[] Align14 { get { return _align14; } }
        public uint Unknown13Count { get { return _unknown13Count; } }
        public List<uint> Unknown13 { get { return _unknown13; } }
        public byte[] Align15 { get { return _align15; } }
        public uint CdPad17Size { get { return _cdPad17Size; } }
        public byte[] CdPad17 { get { return _cdPad17; } }
        public byte[] Align17 { get { return _align17; } }
        public uint Unknown18Count { get { return _unknown18Count; } }
        public List<Unknown18> Unknown18s { get { return _unknown18s; } }
        public byte[] Align18 { get { return _align18; } }
        public uint Unknown19Count { get { return _unknown19Count; } }
        public byte[] Pad19 { get { return _pad19; } }
        public List<float> Unknown19 { get { return _unknown19; } }
        public uint Unknown20Count { get { return _unknown20Count; } }
        public byte[] Pad20 { get { return _pad20; } }
        public List<byte[]> Unknown20 { get { return _unknown20; } }
        public byte[] Align20 { get { return _align20; } }
        public uint Unknown21Count { get { return _unknown21Count; } }
        public byte[] Unknown21Pad { get { return _unknown21Pad; } }
        public List<uint> Unknown21 { get { return _unknown21; } }
        public byte[] Align21 { get { return _align21; } }
        public uint Unknown22Count { get { return _unknown22Count; } }
        public List<uint> Unknown22 { get { return _unknown22; } }
        public byte[] Align22 { get { return _align22; } }
        public List<float> Unknown23 { get { return _unknown23; } }
        public uint Unknown24Count { get { return _unknown24Count; } }
        public byte[] Pad24 { get { return _pad24; } }
        public uint Unknown25Count { get { return _unknown25Count; } }
        public byte[] Pad25 { get { return _pad25; } }
        public List<byte[]> Unknown24 { get { return _unknown24; } }
        public List<byte[]> Unknown25 { get { return _unknown25; } }
        public byte[] Align25 { get { return _align25; } }
        public List<MeshMover> MeshMovers { get { return _meshMovers; } }
        public List<byte[]> Unknown27 { get { return _unknown27; } }
        public List<byte[]> Unknown28 { get { return _unknown28; } }
        public List<byte[]> Unknown29 { get { return _unknown29; } }
        public List<byte[]> Unknown30 { get { return _unknown30; } }
        public List<byte[]> Unknown31 { get { return _unknown31; } }
        public List<byte[]> Unknown32 { get { return _unknown32; } }
        public List<MeshMoverName> MeshMoverNames { get { return _meshMoverNames; } }
        public byte[] Align32 { get { return _align32; } }
        public uint LightCount { get { return _lightCount; } }
        public LightSection LightSections { get { return _lightSections; } }
        public Sr2CpuChunkPc M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
