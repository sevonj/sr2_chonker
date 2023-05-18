// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using System.Collections.Generic;

namespace Kaitai
{

    /// <summary>
    /// MIT License
    /// Copyright (c) 2022 Möyh Mäyhem
    /// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the &quot;Software&quot;), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
    /// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
    /// THE SOFTWARE IS PROVIDED &quot;AS IS&quot;, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
    /// 
    /// 
    /// The chunk parser is incomplete, but it gets pretty far.
    /// </summary>
    public partial class Sr2ChunkPc : KaitaiStruct
    {
        public static Sr2ChunkPc FromFile(string fileName)
        {
            return new Sr2ChunkPc(new KaitaiStream(fileName));
        }

        public Sr2ChunkPc(KaitaiStream p__io, KaitaiStruct p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            __unnamed2 = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_2, new byte[] { 14, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 14, 0, 0, 0 }, Unnamed_2, M_Io, "/seq/2");
            }
            _meshlibrary = m_io.ReadBytes(4);
            _header0x10 = m_io.ReadU4le();
            _header0x14 = m_io.ReadBytes(24);
            __unnamed6 = m_io.ReadBytes(8);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_6, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, Unnamed_6, M_Io, "/seq/6");
            }
            _headerXx = m_io.ReadBytes(40);
            __unnamed8 = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_8, new byte[] { 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_8, M_Io, "/seq/8");
            }
            _headerXxx = m_io.ReadBytes(20);
            __unnamed10 = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_10, new byte[] { 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_10, M_Io, "/seq/10");
            }
            _headerXxxx = m_io.ReadBytes(20);
            _lenGChunk1 = m_io.ReadU4le();
            _lenGChunk2 = m_io.ReadU4le();
            _numCityobjects = m_io.ReadU4le();
            _numUnknown23 = m_io.ReadU4le();
            _header0x9c = m_io.ReadU4le();
            _header0xa0 = m_io.ReadU4le();
            __unnamed18 = m_io.ReadBytes(16);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_18, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Unnamed_18, M_Io, "/seq/18");
            }
            _numMeshMovers = m_io.ReadU4le();
            _numUnknown27 = m_io.ReadU4le();
            _numUnknown28 = m_io.ReadU4le();
            _numUnknown29 = m_io.ReadU4le();
            _numUnknown30 = m_io.ReadU4le();
            _numUnknown31 = m_io.ReadU4le();
            _header0xcc = m_io.ReadU4le();
            _numUnknown26 = m_io.ReadU4le();
            _worldMinX = m_io.ReadF4le();
            _worldMinY = m_io.ReadF4le();
            _worldMinZ = m_io.ReadF4le();
            _worldMaxX = m_io.ReadF4le();
            _worldMaxY = m_io.ReadF4le();
            _worldMaxZ = m_io.ReadF4le();
            _header0xec = m_io.ReadF4le();
            _numUnknown32 = m_io.ReadU4le();
            __unnamed35 = m_io.ReadBytes(12);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_35, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, Unnamed_35, M_Io, "/seq/35");
            }
            _numTextureNames = m_io.ReadU4le();
            __unnamed37 = m_io.ReadBytes((NumTextureNames * 4));
            _textureNames = new List<string>();
            for (var i = 0; i < NumTextureNames; i++)
            {
                _textureNames.Add(System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true)));
            }
            __unnamed39 = new Align(16, m_io, this, m_root);
            _numRendermodels = m_io.ReadU4le();
            _numCityobjectParts = m_io.ReadU4le();
            _modelCount = m_io.ReadU4le();
            _numUnknown3s = m_io.ReadU4le();
            _numUnknown4s = m_io.ReadU4le();
            __unnamed45 = new Align(16, m_io, this, m_root);
            _rendermodelUnk0s = new List<RendermodelUnk0>();
            for (var i = 0; i < NumRendermodels; i++)
            {
                _rendermodelUnk0s.Add(new RendermodelUnk0(m_io, this, m_root));
            }
            __unnamed47 = new Align(16, m_io, this, m_root);
            _cityobjectPartsOffset = new Offset(M_Io.Pos, m_io, this, m_root);
            _cityobjectParts = new List<CityobjectPart>();
            for (var i = 0; i < NumCityobjectParts; i++)
            {
                _cityobjectParts.Add(new CityobjectPart(m_io, this, m_root));
            }
            __unnamed50 = new Align(16, m_io, this, m_root);
            _unknown3s = new List<Unknown3>();
            for (var i = 0; i < NumUnknown3s; i++)
            {
                _unknown3s.Add(new Unknown3(m_io, this, m_root));
            }
            __unnamed52 = new Align(16, m_io, this, m_root);
            _unknown4s = new List<Unknown4>();
            for (var i = 0; i < NumUnknown4s; i++)
            {
                _unknown4s.Add(new Unknown4(m_io, this, m_root));
            }
            __unnamed54 = new Align(16, m_io, this, m_root);
            _numBakedCollisionVertices = m_io.ReadU4le();
            _bakedCollisionVertices = new List<Vec3>();
            for (var i = 0; i < NumBakedCollisionVertices; i++)
            {
                _bakedCollisionVertices.Add(new Vec3(m_io, this, m_root));
            }
            _numBakedCollisionUnk0 = m_io.ReadU4le();
            _bakedCollisionUnk0 = m_io.ReadBytes((3 * NumBakedCollisionUnk0));
            _numBakedCollisionUnk1 = m_io.ReadU4le();
            _bakedCollisionUnk1 = m_io.ReadBytes((4 * NumBakedCollisionUnk1));
            _numBakedCollisionUnk2 = m_io.ReadU4le();
            _bakedCollisionUnk2 = m_io.ReadBytes((12 * NumBakedCollisionUnk2));
            __unnamed63 = new Align(16, m_io, this, m_root);
            _lenBakedCollisionMopp = m_io.ReadU4le();
            __unnamed65 = new Align(16, m_io, this, m_root);
            _bakedCollisionMopp = m_io.ReadBytes(LenBakedCollisionMopp);
            __unnamed67 = new Align(4, m_io, this, m_root);
            _bakedCollisionAabbMin = new Vec3(m_io, this, m_root);
            _bakedCollisionAabbMax = new Vec3(m_io, this, m_root);
            __unnamed70 = new Align(16, m_io, this, m_root);
            _indexBuffers = new List<IndexBufferData>();
            for (var i = 0; i < ModelCount; i++)
            {
                _indexBuffers.Add(new IndexBufferData(i, m_io, this, m_root));
            }
            _vertexBuffers = new List<VertexBuffer>();
            for (var i = 0; i < ModelCount; i++)
            {
                _vertexBuffers.Add(new VertexBuffer(IndexBuffers[i].NumBlocks, m_io, this, m_root));
            }
            __unnamed73 = new Align(16, m_io, this, m_root);
            _physModels = new List<PhysModelBuffer>();
            for (var i = 0; i < ModelCount; i++)
            {
                _physModels.Add(new PhysModelBuffer(IndexBuffers[i].Type, IndexBuffers[i].NumIndices, VertexBuffers[i].Blocks[0].NumVertices, m_io, this, m_root));
            }
            _numMaterials = m_io.ReadU4le();
            __unnamed76 = new Align(16, m_io, this, m_root);
            _numMatShaderParams = m_io.ReadU4le();
            __unnamed78 = m_io.ReadBytes(8);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_78, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }, Unnamed_78, M_Io, "/seq/78");
            }
            _numMatUnknown3s = m_io.ReadU4le();
            _matUnknown1 = m_io.ReadU4le();
            _materials = new List<Material>();
            for (var i = 0; i < NumMaterials; i++)
            {
                _materials.Add(new Material(m_io, this, m_root));
            }
            _matConstants = new List<MatConstant>();
            for (var i = 0; i < NumMaterials; i++)
            {
                _matConstants.Add(new MatConstant(Materials[i].NumConstants, m_io, this, m_root));
            }
            _matUnknown2s = new List<MatUnknown2>();
            for (var i = 0; i < NumMaterials; i++)
            {
                _matUnknown2s.Add(new MatUnknown2(m_io, this, m_root));
            }
            __unnamed84 = new Align(16, m_io, this, m_root);
            _matShaderParams = new List<float>();
            for (var i = 0; i < NumMatShaderParams; i++)
            {
                _matShaderParams.Add(m_io.ReadF4le());
            }
            _matTextures = new List<MatTexCont>();
            for (var i = 0; i < NumMaterials; i++)
            {
                _matTextures.Add(new MatTexCont(m_io, this, m_root));
            }
            _matUnknown3s = new List<MatUnknown3>();
            for (var i = 0; i < NumMatUnknown3s; i++)
            {
                _matUnknown3s.Add(new MatUnknown3(m_io, this, m_root));
            }
            _matUnknown3Unk2 = new List<byte[]>();
            for (var i = 0; i < NumMatUnknown3s; i++)
            {
                _matUnknown3Unk2.Add(m_io.ReadBytes((MatUnknown3s[i].Unk2Count * 4)));
            }
            _rendermodels = new List<Rendermodel>();
            for (var i = 0; i < NumRendermodels; i++)
            {
                _rendermodels.Add(new Rendermodel(m_io, this, m_root));
            }
            _cityobjects = new List<Cityobject>();
            for (var i = 0; i < NumCityobjects; i++)
            {
                _cityobjects.Add(new Cityobject(m_io, this, m_root));
            }
            _cityobjectNames = new List<string>();
            for (var i = 0; i < NumCityobjects; i++)
            {
                _cityobjectNames.Add(System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true)));
            }
            __unnamed92 = new Align(16, m_io, this, m_root);
            _lenUnknownNames = m_io.ReadU4le();
            _unknownNames = m_io.ReadBytes(LenUnknownNames);
            __unnamed95 = new Align(16, m_io, this, m_root);
            _numUnknown13 = m_io.ReadU4le();
            _unknown13 = new List<uint>();
            for (var i = 0; i < NumUnknown13; i++)
            {
                _unknown13.Add(m_io.ReadU4le());
            }
            __unnamed98 = new Align(16, m_io, this, m_root);
            _lenPad17 = m_io.ReadU4le();
            __unnamed100 = m_io.ReadBytes(LenPad17);
            __unnamed101 = new Align(16, m_io, this, m_root);
            _numUnknown18s = m_io.ReadU4le();
            _unknown18s = new List<Unknown18>();
            for (var i = 0; i < NumUnknown18s; i++)
            {
                _unknown18s.Add(new Unknown18(m_io, this, m_root));
            }
            __unnamed104 = new Align(16, m_io, this, m_root);
            _numUnknown19 = m_io.ReadU4le();
            __unnamed106 = m_io.ReadBytes((NumUnknown19 * 28));
            _unknown19 = new List<float>();
            for (var i = 0; i < (NumUnknown19 * 7); i++)
            {
                _unknown19.Add(m_io.ReadF4le());
            }
            _numUnknown20 = m_io.ReadU4le();
            __unnamed109 = m_io.ReadBytes((NumUnknown20 * 12));
            _unknown20 = new List<byte[]>();
            for (var i = 0; i < NumUnknown20; i++)
            {
                _unknown20.Add(m_io.ReadBytes(12));
            }
            __unnamed111 = new Align(16, m_io, this, m_root);
            _numUnknown21 = m_io.ReadU4le();
            _unknown21Pad = m_io.ReadBytes((NumUnknown21 * 8));
            _unknown21 = new List<uint>();
            for (var i = 0; i < (NumUnknown21 * 2); i++)
            {
                _unknown21.Add(m_io.ReadU4le());
            }
            __unnamed115 = new Align(16, m_io, this, m_root);
            _numUnknown22 = m_io.ReadU4le();
            _unknown22 = new List<uint>();
            for (var i = 0; i < NumUnknown22; i++)
            {
                _unknown22.Add(m_io.ReadU4le());
            }
            __unnamed118 = new Align(16, m_io, this, m_root);
            _unknown23 = new List<float>();
            for (var i = 0; i < (NumUnknown23 * 12); i++)
            {
                _unknown23.Add(m_io.ReadF4le());
            }
            _numUnknown24s = m_io.ReadU4le();
            __unnamed121 = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_121, new byte[] { 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_121, M_Io, "/seq/121");
            }
            _numUnknown25 = m_io.ReadU4le();
            __unnamed123 = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Unnamed_123, new byte[] { 0, 0, 0, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_123, M_Io, "/seq/123");
            }
            _unknown24s = new List<Unknown24>();
            for (var i = 0; i < NumUnknown24s; i++)
            {
                _unknown24s.Add(new Unknown24(m_io, this, m_root));
            }
            _unknown25 = new List<byte[]>();
            for (var i = 0; i < NumUnknown25; i++)
            {
                _unknown25.Add(m_io.ReadBytes(2));
            }
            __unnamed126 = new Align(16, m_io, this, m_root);
            _unknown26 = new List<byte[]>();
            for (var i = 0; i < NumUnknown26; i++)
            {
                _unknown26.Add(m_io.ReadBytes(2));
            }
            __unnamed128 = new Align(16, m_io, this, m_root);
            _meshMovers = new List<MeshMover>();
            for (var i = 0; i < NumMeshMovers; i++)
            {
                _meshMovers.Add(new MeshMover(m_io, this, m_root));
            }
            _unknown27 = new List<byte[]>();
            for (var i = 0; i < NumUnknown27; i++)
            {
                _unknown27.Add(m_io.ReadBytes(24));
            }
            _unknown28 = new List<byte[]>();
            for (var i = 0; i < NumUnknown28; i++)
            {
                _unknown28.Add(m_io.ReadBytes(36));
            }
            _unknown29 = new List<byte[]>();
            for (var i = 0; i < NumUnknown29; i++)
            {
                _unknown29.Add(m_io.ReadBytes(4));
            }
            _unknown30 = new List<byte[]>();
            for (var i = 0; i < NumUnknown30; i++)
            {
                _unknown30.Add(m_io.ReadBytes(12));
            }
            _unknown31 = new List<byte[]>();
            for (var i = 0; i < NumUnknown31; i++)
            {
                _unknown31.Add(m_io.ReadBytes(8));
            }
            _unknown32 = new List<byte[]>();
            for (var i = 0; i < NumUnknown32; i++)
            {
                _unknown32.Add(m_io.ReadBytes(8));
            }
            _meshMoverNames = new List<MeshMoverName>();
            for (var i = 0; i < NumMeshMovers; i++)
            {
                _meshMoverNames.Add(new MeshMoverName(MeshMovers[i].StartCount, m_io, this, m_root));
            }
            __unnamed137 = new Align(16, m_io, this, m_root);
            _numLights = new NumLightsType(m_io, this, m_root);
            _lightsOffset = new Offset(M_Io.Pos, m_io, this, m_root);
            if (NumLights.Result != 0) {
                _lightUnknown = m_io.ReadU4le();
            }
            _lightData = new List<LightDataType>();
            for (var i = 0; i < NumLights.Result; i++)
            {
                _lightData.Add(new LightDataType(m_io, this, m_root));
            }
            _lightNames = new List<string>();
            for (var i = 0; i < NumLights.Result; i++)
            {
                _lightNames.Add(System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytesTerm(0, false, true, true)));
            }
            __unnamed143 = new Align(16, m_io, this, m_root);
            _lightUnkfloats = new List<LightUnkfloatsType>();
            for (var i = 0; i < NumLights.Result; i++)
            {
                _lightUnkfloats.Add(new LightUnkfloatsType(LightData[i].NumLightUnkfloats, m_io, this, m_root));
            }
            __unnamed145 = new Align(16, m_io, this, m_root);
        }
        public partial class MatUnknown3 : KaitaiStruct
        {
            public static MatUnknown3 FromFile(string fileName)
            {
                return new MatUnknown3(new KaitaiStream(fileName));
            }

            public MatUnknown3(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
                __unnamed3 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_3, new byte[] { 255, 255, 255, 255 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 255, 255, 255, 255 }, Unnamed_3, M_Io, "/types/mat_unknown3/seq/3");
                }
            }
            private byte[] _unk;
            private ushort _unk2Count;
            private ushort _unk3;
            private byte[] __unnamed3;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public byte[] Unk { get { return _unk; } }
            public ushort Unk2Count { get { return _unk2Count; } }
            public ushort Unk3 { get { return _unk3; } }
            public byte[] Unnamed_3 { get { return __unnamed3; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Unknown4 : KaitaiStruct
        {
            public static Unknown4 FromFile(string fileName)
            {
                return new Unknown4(new KaitaiStream(fileName));
            }

            public Unknown4(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
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
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class LightDataType : KaitaiStruct
        {
            public static LightDataType FromFile(string fileName)
            {
                return new LightDataType(new KaitaiStream(fileName));
            }

            public LightDataType(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_shadowCharacter = false;
                f_shadowLevel = false;
                f_bitflag22 = false;
                f_bitflag8 = false;
                f_bitflag2 = false;
                f_bitflag10 = false;
                f_lightCharacter = false;
                f_lightLevel = false;
                f_bitflag3 = false;
                f_bitflag4 = false;
                f_bitflag1 = false;
                f_bitflag0 = false;
                _read();
            }
            private void _read()
            {
                _bitmask = new List<bool>();
                for (var i = 0; i < 32; i++)
                {
                    _bitmask.Add(m_io.ReadBitsIntBe(1) != 0);
                }
                m_io.AlignToByte();
                __unnamed1 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_1, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_1, M_Io, "/types/light_data_type/seq/1");
                }
                _r = m_io.ReadF4le();
                _g = m_io.ReadF4le();
                _b = m_io.ReadF4le();
                _unk5 = m_io.ReadU4le();
                _unk6 = m_io.ReadU4le();
                _unk7 = m_io.ReadU4le();
                _numLightUnkfloats = m_io.ReadU4le();
                __unnamed9 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_9, new byte[] { 255, 255, 255, 255 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 255, 255, 255, 255 }, Unnamed_9, M_Io, "/types/light_data_type/seq/9");
                }
                _unk10 = m_io.ReadS4le();
                __unnamed11 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_11, new byte[] { 255, 255, 255, 255 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 255, 255, 255, 255 }, Unnamed_11, M_Io, "/types/light_data_type/seq/11");
                }
                _unk12 = m_io.ReadU4le();
                _unkf13 = m_io.ReadF4le();
                _unkf14 = m_io.ReadF4le();
                _position = new Vec3(m_io, this, m_root);
                _basisX = new Vec3(m_io, this, m_root);
                _basisY = new Vec3(m_io, this, m_root);
                _basisZ = new Vec3(m_io, this, m_root);
                _unkf27 = m_io.ReadF4le();
                _unkf28 = m_io.ReadF4le();
                _radiusInner = m_io.ReadF4le();
                _radiusOuter = m_io.ReadF4le();
                _renderDist = m_io.ReadF4le();
                __unnamed24 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_24, new byte[] { 255, 255, 255, 255 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 255, 255, 255, 255 }, Unnamed_24, M_Io, "/types/light_data_type/seq/24");
                }
                _parentCityobject = m_io.ReadS4le();
                _unk34 = m_io.ReadU4le();
                _unk35 = m_io.ReadU4le();
                _type = m_io.ReadU4le();
            }
            private bool f_shadowCharacter;
            private bool _shadowCharacter;
            public bool ShadowCharacter
            {
                get
                {
                    if (f_shadowCharacter)
                        return _shadowCharacter;
                    _shadowCharacter = (bool) (Bitmask[12]);
                    f_shadowCharacter = true;
                    return _shadowCharacter;
                }
            }
            private bool f_shadowLevel;
            private bool _shadowLevel;
            public bool ShadowLevel
            {
                get
                {
                    if (f_shadowLevel)
                        return _shadowLevel;
                    _shadowLevel = (bool) (Bitmask[13]);
                    f_shadowLevel = true;
                    return _shadowLevel;
                }
            }
            private bool f_bitflag22;
            private bool _bitflag22;
            public bool Bitflag22
            {
                get
                {
                    if (f_bitflag22)
                        return _bitflag22;
                    _bitflag22 = (bool) (Bitmask[22]);
                    f_bitflag22 = true;
                    return _bitflag22;
                }
            }
            private bool f_bitflag8;
            private bool _bitflag8;
            public bool Bitflag8
            {
                get
                {
                    if (f_bitflag8)
                        return _bitflag8;
                    _bitflag8 = (bool) (Bitmask[8]);
                    f_bitflag8 = true;
                    return _bitflag8;
                }
            }
            private bool f_bitflag2;
            private bool _bitflag2;
            public bool Bitflag2
            {
                get
                {
                    if (f_bitflag2)
                        return _bitflag2;
                    _bitflag2 = (bool) (Bitmask[2]);
                    f_bitflag2 = true;
                    return _bitflag2;
                }
            }
            private bool f_bitflag10;
            private bool _bitflag10;
            public bool Bitflag10
            {
                get
                {
                    if (f_bitflag10)
                        return _bitflag10;
                    _bitflag10 = (bool) (Bitmask[10]);
                    f_bitflag10 = true;
                    return _bitflag10;
                }
            }
            private bool f_lightCharacter;
            private bool _lightCharacter;
            public bool LightCharacter
            {
                get
                {
                    if (f_lightCharacter)
                        return _lightCharacter;
                    _lightCharacter = (bool) (Bitmask[14]);
                    f_lightCharacter = true;
                    return _lightCharacter;
                }
            }
            private bool f_lightLevel;
            private bool _lightLevel;
            public bool LightLevel
            {
                get
                {
                    if (f_lightLevel)
                        return _lightLevel;
                    _lightLevel = (bool) (Bitmask[15]);
                    f_lightLevel = true;
                    return _lightLevel;
                }
            }
            private bool f_bitflag3;
            private bool _bitflag3;
            public bool Bitflag3
            {
                get
                {
                    if (f_bitflag3)
                        return _bitflag3;
                    _bitflag3 = (bool) (Bitmask[3]);
                    f_bitflag3 = true;
                    return _bitflag3;
                }
            }
            private bool f_bitflag4;
            private bool _bitflag4;
            public bool Bitflag4
            {
                get
                {
                    if (f_bitflag4)
                        return _bitflag4;
                    _bitflag4 = (bool) (Bitmask[4]);
                    f_bitflag4 = true;
                    return _bitflag4;
                }
            }
            private bool f_bitflag1;
            private bool _bitflag1;
            public bool Bitflag1
            {
                get
                {
                    if (f_bitflag1)
                        return _bitflag1;
                    _bitflag1 = (bool) (Bitmask[1]);
                    f_bitflag1 = true;
                    return _bitflag1;
                }
            }
            private bool f_bitflag0;
            private bool _bitflag0;
            public bool Bitflag0
            {
                get
                {
                    if (f_bitflag0)
                        return _bitflag0;
                    _bitflag0 = (bool) (Bitmask[0]);
                    f_bitflag0 = true;
                    return _bitflag0;
                }
            }
            private List<bool> _bitmask;
            private byte[] __unnamed1;
            private float _r;
            private float _g;
            private float _b;
            private uint _unk5;
            private uint _unk6;
            private uint _unk7;
            private uint _numLightUnkfloats;
            private byte[] __unnamed9;
            private int _unk10;
            private byte[] __unnamed11;
            private uint _unk12;
            private float _unkf13;
            private float _unkf14;
            private Vec3 _position;
            private Vec3 _basisX;
            private Vec3 _basisY;
            private Vec3 _basisZ;
            private float _unkf27;
            private float _unkf28;
            private float _radiusInner;
            private float _radiusOuter;
            private float _renderDist;
            private byte[] __unnamed24;
            private int _parentCityobject;
            private uint _unk34;
            private uint _unk35;
            private uint _type;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public List<bool> Bitmask { get { return _bitmask; } }
            public byte[] Unnamed_1 { get { return __unnamed1; } }
            public float R { get { return _r; } }
            public float G { get { return _g; } }
            public float B { get { return _b; } }

            /// <summary>
            /// bitmask?
            /// </summary>
            public uint Unk5 { get { return _unk5; } }

            /// <summary>
            /// bitmask?
            /// </summary>
            public uint Unk6 { get { return _unk6; } }

            /// <summary>
            /// bitmask?
            /// </summary>
            public uint Unk7 { get { return _unk7; } }
            public uint NumLightUnkfloats { get { return _numLightUnkfloats; } }
            public byte[] Unnamed_9 { get { return __unnamed9; } }
            public int Unk10 { get { return _unk10; } }
            public byte[] Unnamed_11 { get { return __unnamed11; } }

            /// <summary>
            /// bitmask?
            /// </summary>
            public uint Unk12 { get { return _unk12; } }
            public float Unkf13 { get { return _unkf13; } }
            public float Unkf14 { get { return _unkf14; } }
            public Vec3 Position { get { return _position; } }
            public Vec3 BasisX { get { return _basisX; } }
            public Vec3 BasisY { get { return _basisY; } }
            public Vec3 BasisZ { get { return _basisZ; } }
            public float Unkf27 { get { return _unkf27; } }
            public float Unkf28 { get { return _unkf28; } }
            public float RadiusInner { get { return _radiusInner; } }
            public float RadiusOuter { get { return _radiusOuter; } }
            public float RenderDist { get { return _renderDist; } }
            public byte[] Unnamed_24 { get { return __unnamed24; } }
            public int ParentCityobject { get { return _parentCityobject; } }

            /// <summary>
            /// bitmask?
            /// </summary>
            public uint Unk34 { get { return _unk34; } }

            /// <summary>
            /// often 0, otherwise &lt;250
            /// </summary>
            public uint Unk35 { get { return _unk35; } }
            public uint Type { get { return _type; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class MeshMover : KaitaiStruct
        {
            public static MeshMover FromFile(string fileName)
            {
                return new MeshMover(new KaitaiStream(fileName));
            }

            public MeshMover(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public byte[] Unk0 { get { return _unk0; } }
            public ushort StartCount { get { return _startCount; } }
            public byte[] Unk1 { get { return _unk1; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Align : KaitaiStruct
        {
            public Align(uint p_size, KaitaiStream p__io, KaitaiStruct p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _size = p_size;
                _read();
            }
            private void _read()
            {
                __unnamed0 = m_io.ReadBytes(KaitaiStream.Mod((Size - M_Io.Pos), Size));
            }
            private byte[] __unnamed0;
            private uint _size;
            private Sr2ChunkPc m_root;
            private KaitaiStruct m_parent;
            public byte[] Unnamed_0 { get { return __unnamed0; } }
            public uint Size { get { return _size; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class RendermodelUnk0 : KaitaiStruct
        {
            public static RendermodelUnk0 FromFile(string fileName)
            {
                return new RendermodelUnk0(new KaitaiStream(fileName));
            }

            public RendermodelUnk0(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public uint Unk0 { get { return _unk0; } }
            public uint Unk1 { get { return _unk1; } }
            public uint Unk2 { get { return _unk2; } }
            public uint Unk3 { get { return _unk3; } }
            public uint Unk4 { get { return _unk4; } }
            public uint Unk5 { get { return _unk5; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class MatUnknown2 : KaitaiStruct
        {
            public static MatUnknown2 FromFile(string fileName)
            {
                return new MatUnknown2(new KaitaiStream(fileName));
            }

            public MatUnknown2(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _data = new List<uint>();
                for (var i = 0; i < 4; i++)
                {
                    _data.Add(m_io.ReadU4le());
                }
            }
            private List<uint> _data;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public List<uint> Data { get { return _data; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Unknown33 : KaitaiStruct
        {
            public static Unknown33 FromFile(string fileName)
            {
                return new Unknown33(new KaitaiStream(fileName));
            }

            public Unknown33(KaitaiStream p__io, KaitaiStruct p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _boxMin = new Vec3(m_io, this, m_root);
                __unnamed1 = m_io.ReadBytes(4);
                _boxMax = new Vec3(m_io, this, m_root);
                __unnamed3 = m_io.ReadBytes(4);
                _unk = m_io.ReadBytes(16);
            }
            private Vec3 _boxMin;
            private byte[] __unnamed1;
            private Vec3 _boxMax;
            private byte[] __unnamed3;
            private byte[] _unk;
            private Sr2ChunkPc m_root;
            private KaitaiStruct m_parent;
            public Vec3 BoxMin { get { return _boxMin; } }
            public byte[] Unnamed_1 { get { return __unnamed1; } }
            public Vec3 BoxMax { get { return _boxMax; } }
            public byte[] Unnamed_3 { get { return __unnamed3; } }
            public byte[] Unk { get { return _unk; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class MatTex : KaitaiStruct
        {
            public static MatTex FromFile(string fileName)
            {
                return new MatTex(new KaitaiStream(fileName));
            }

            public MatTex(KaitaiStream p__io, Sr2ChunkPc.MatTexCont p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc.MatTexCont m_parent;
            public ushort TexId { get { return _texId; } }
            public ushort TexFlag { get { return _texFlag; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc.MatTexCont M_Parent { get { return m_parent; } }
        }
        public partial class MatConstant : KaitaiStruct
        {
            public MatConstant(ushort p_numConstants, KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _numConstants = p_numConstants;
                _read();
            }
            private void _read()
            {
                _data = m_io.ReadBytes((NumConstants * 6));
                __unnamed1 = m_io.ReadBytes(KaitaiStream.Mod((4 - (NumConstants * 6)), 4));
            }
            private byte[] _data;
            private byte[] __unnamed1;
            private ushort _numConstants;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public byte[] Data { get { return _data; } }
            public byte[] Unnamed_1 { get { return __unnamed1; } }
            public ushort NumConstants { get { return _numConstants; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class VertexBuffer : KaitaiStruct
        {
            public VertexBuffer(ushort p_numBlocks, KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _numBlocks = p_numBlocks;
                _read();
            }
            private void _read()
            {
                _blocks = new List<VertexBufferData>();
                for (var i = 0; i < NumBlocks; i++)
                {
                    _blocks.Add(new VertexBufferData(m_io, this, m_root));
                }
            }
            private List<VertexBufferData> _blocks;
            private ushort _numBlocks;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public List<VertexBufferData> Blocks { get { return _blocks; } }

            /// <summary>
            /// Number of vert blocks.
            /// </summary>
            public ushort NumBlocks { get { return _numBlocks; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class MeshMoverName : KaitaiStruct
        {
            public MeshMoverName(ushort p_startCount, KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public string Name { get { return _name; } }
            public List<string> StartNames { get { return _startNames; } }
            public ushort StartCount { get { return _startCount; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class LightUnkfloatsType : KaitaiStruct
        {
            public LightUnkfloatsType(uint p_num, KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _num = p_num;
                _read();
            }
            private void _read()
            {
                _unk = new List<float>();
                for (var i = 0; i < Num; i++)
                {
                    _unk.Add(m_io.ReadF4le());
                }
            }
            private List<float> _unk;
            private uint _num;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public List<float> Unk { get { return _unk; } }
            public uint Num { get { return _num; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class PhysModelBuffer : KaitaiStruct
        {
            public PhysModelBuffer(uint p_type, uint p_numIndices, uint p_numVertices, KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _type = p_type;
                _numIndices = p_numIndices;
                _numVertices = p_numVertices;
                _read();
            }
            private void _read()
            {
                if (Type == 7) {
                    _vertexBuffer = m_io.ReadBytes((NumVertices * 12));
                }
                __unnamed1 = new Align(16, m_io, this, m_root);
                if (Type == 7) {
                    _indexBuffer = m_io.ReadBytes((NumIndices * 2));
                }
                __unnamed3 = new Align(16, m_io, this, m_root);
            }
            private byte[] _vertexBuffer;
            private Align __unnamed1;
            private byte[] _indexBuffer;
            private Align __unnamed3;
            private uint _type;
            private uint _numIndices;
            private uint _numVertices;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public byte[] VertexBuffer { get { return _vertexBuffer; } }
            public Align Unnamed_1 { get { return __unnamed1; } }
            public byte[] IndexBuffer { get { return _indexBuffer; } }
            public Align Unnamed_3 { get { return __unnamed3; } }
            public uint Type { get { return _type; } }
            public uint NumIndices { get { return _numIndices; } }
            public uint NumVertices { get { return _numVertices; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Offset : KaitaiStruct
        {
            public Offset(long p_input, KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _input = p_input;
                f_temp = false;
                _read();
            }
            private void _read()
            {
            }
            private bool f_temp;
            private long _temp;
            public long Temp
            {
                get
                {
                    if (f_temp)
                        return _temp;
                    _temp = (long) (Input);
                    f_temp = true;
                    return _temp;
                }
            }
            private long _input;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public long Input { get { return _input; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class IndexBufferData : KaitaiStruct
        {
            public IndexBufferData(int p_i, KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _i = p_i;
                _read();
            }
            private void _read()
            {
                _type = m_io.ReadU2le();
                _numBlocks = m_io.ReadU2le();
                _numIndices = m_io.ReadU4le();
                __unnamed3 = m_io.ReadBytes(12);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_3, new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 0, 0, 0, 0 }, Unnamed_3, M_Io, "/types/index_buffer_data/seq/3");
                }
            }
            private ushort _type;
            private ushort _numBlocks;
            private uint _numIndices;
            private byte[] __unnamed3;
            private int _i;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;

            /// <summary>
            /// Type 0 is gpu rendermodel, located in g_chunk file. Massive shared buffers for rendermodels. Never more than one of these per chunk.
            /// Type 7 is cpu collision model, located in this file almost right below. Separate buffers for each collision model.
            /// No other types are used in any chunks.
            /// </summary>
            public ushort Type { get { return _type; } }

            /// <summary>
            /// Number of vertex blocks
            /// </summary>
            public ushort NumBlocks { get { return _numBlocks; } }
            public uint NumIndices { get { return _numIndices; } }
            public byte[] Unnamed_3 { get { return __unnamed3; } }
            public int I { get { return _i; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class CityobjectPart : KaitaiStruct
        {
            public static CityobjectPart FromFile(string fileName)
            {
                return new CityobjectPart(new KaitaiStream(fileName));
            }

            public CityobjectPart(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _pos = new Vec3(m_io, this, m_root);
                _basisX = new Vec3(m_io, this, m_root);
                _basisY = new Vec3(m_io, this, m_root);
                _basisZ = new Vec3(m_io, this, m_root);
                _restOfTheTransform = m_io.ReadBytes(36);
                _unk0 = m_io.ReadU4le();
                _rendermodelId = m_io.ReadU4le();
                _unk1 = m_io.ReadU4le();
            }
            private Vec3 _pos;
            private Vec3 _basisX;
            private Vec3 _basisY;
            private Vec3 _basisZ;
            private byte[] _restOfTheTransform;
            private uint _unk0;
            private uint _rendermodelId;
            private uint _unk1;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public Vec3 Pos { get { return _pos; } }
            public Vec3 BasisX { get { return _basisX; } }
            public Vec3 BasisY { get { return _basisY; } }
            public Vec3 BasisZ { get { return _basisZ; } }
            public byte[] RestOfTheTransform { get { return _restOfTheTransform; } }
            public uint Unk0 { get { return _unk0; } }
            public uint RendermodelId { get { return _rendermodelId; } }
            public uint Unk1 { get { return _unk1; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Material : KaitaiStruct
        {
            public static Material FromFile(string fileName)
            {
                return new Material(new KaitaiStream(fileName));
            }

            public Material(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk = m_io.ReadBytes(10);
                __unnamed1 = m_io.ReadBytes(2);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_1, new byte[] { 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0 }, Unnamed_1, M_Io, "/types/material/seq/1");
                }
                _numConstants = m_io.ReadU2le();
                _numTextures = m_io.ReadU2le();
                _matFlags = m_io.ReadU4le();
                _unk2 = m_io.ReadS4le();
            }
            private byte[] _unk;
            private byte[] __unnamed1;
            private ushort _numConstants;
            private ushort _numTextures;
            private uint _matFlags;
            private int _unk2;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public byte[] Unk { get { return _unk; } }
            public byte[] Unnamed_1 { get { return __unnamed1; } }

            /// <summary>
            /// in SRIV
            /// </summary>
            public ushort NumConstants { get { return _numConstants; } }
            public ushort NumTextures { get { return _numTextures; } }

            /// <summary>
            /// probably
            /// </summary>
            public uint MatFlags { get { return _matFlags; } }

            /// <summary>
            /// name_checksum in SRIV
            /// </summary>
            public int Unk2 { get { return _unk2; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Vec3 : KaitaiStruct
        {
            public static Vec3 FromFile(string fileName)
            {
                return new Vec3(new KaitaiStream(fileName));
            }

            public Vec3(KaitaiStream p__io, KaitaiStruct p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            private Sr2ChunkPc m_root;
            private KaitaiStruct m_parent;
            public float X { get { return _x; } }
            public float Y { get { return _y; } }
            public float Z { get { return _z; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
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

            public Rendermodel(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
                __unnamed6 = new Align(16, m_io, this, m_root);
                if (CheckSubmeshes != 0) {
                    _submeshUnk0 = m_io.ReadU2le();
                }
                if (CheckSubmeshes != 0) {
                    _numSubmeshes = m_io.ReadU2le();
                }
                if (CheckSubmeshes != 0) {
                    _submeshUnk1 = m_io.ReadBytes(4);
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
                    _numSubmesh2s = m_io.ReadU2le();
                }
                if (CheckSubmesh2 != 0) {
                    _submesh2Unk1 = m_io.ReadBytes(4);
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
                for (var i = 0; i < NumSubmeshes; i++)
                {
                    _submeshes.Add(new Submesh(m_io, this, m_root));
                }
                if (CheckSubmesh2 != 0) {
                    _submesh2s = new List<Submesh2>();
                    for (var i = 0; i < NumSubmesh2s; i++)
                    {
                        _submesh2s.Add(new Submesh2(m_io, this, m_root));
                    }
                }
            }
            public partial class Submesh : KaitaiStruct
            {
                public static Submesh FromFile(string fileName)
                {
                    return new Submesh(new KaitaiStream(fileName));
                }

                public Submesh(KaitaiStream p__io, Sr2ChunkPc.Rendermodel p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _vertBufferId = m_io.ReadU4le();
                    _offIndices = m_io.ReadU4le();
                    _offVertices = m_io.ReadU4le();
                    _numIndices = m_io.ReadU2le();
                    _materialId = m_io.ReadU2le();
                }
                private uint _vertBufferId;
                private uint _offIndices;
                private uint _offVertices;
                private ushort _numIndices;
                private ushort _materialId;
                private Sr2ChunkPc m_root;
                private Sr2ChunkPc.Rendermodel m_parent;
                public uint VertBufferId { get { return _vertBufferId; } }
                public uint OffIndices { get { return _offIndices; } }
                public uint OffVertices { get { return _offVertices; } }
                public ushort NumIndices { get { return _numIndices; } }
                public ushort MaterialId { get { return _materialId; } }
                public Sr2ChunkPc M_Root { get { return m_root; } }
                public Sr2ChunkPc.Rendermodel M_Parent { get { return m_parent; } }
            }
            public partial class Submesh2 : KaitaiStruct
            {
                public static Submesh2 FromFile(string fileName)
                {
                    return new Submesh2(new KaitaiStream(fileName));
                }

                public Submesh2(KaitaiStream p__io, Sr2ChunkPc.Rendermodel p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
                    _numIndices = m_io.ReadU2le();
                    _materialId = m_io.ReadU2le();
                }
                private byte[] _unused;
                private byte[] _unknown;
                private byte[] _unused2;
                private ushort _numIndices;
                private ushort _materialId;
                private Sr2ChunkPc m_root;
                private Sr2ChunkPc.Rendermodel m_parent;
                public byte[] Unused { get { return _unused; } }
                public byte[] Unknown { get { return _unknown; } }
                public byte[] Unused2 { get { return _unused2; } }
                public ushort NumIndices { get { return _numIndices; } }
                public ushort MaterialId { get { return _materialId; } }
                public Sr2ChunkPc M_Root { get { return m_root; } }
                public Sr2ChunkPc.Rendermodel M_Parent { get { return m_parent; } }
            }
            private Vec3 _bboxMin;
            private Vec3 _bboxMax;
            private uint _unk0;
            private uint _unk1;
            private uint _checkSubmeshes;
            private uint _checkSubmesh2;
            private Align __unnamed6;
            private ushort? _submeshUnk0;
            private ushort? _numSubmeshes;
            private byte[] _submeshUnk1;
            private uint? _submeshUnk2;
            private byte[] _submeshPad1;
            private ushort? _submesh2Unk0;
            private ushort? _numSubmesh2s;
            private byte[] _submesh2Unk1;
            private uint? _submesh2Unk2;
            private byte[] _submesh2Pad1;
            private List<Submesh> _submeshes;
            private List<Submesh2> _submesh2s;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public Vec3 BboxMin { get { return _bboxMin; } }
            public Vec3 BboxMax { get { return _bboxMax; } }
            public uint Unk0 { get { return _unk0; } }
            public uint Unk1 { get { return _unk1; } }
            public uint CheckSubmeshes { get { return _checkSubmeshes; } }
            public uint CheckSubmesh2 { get { return _checkSubmesh2; } }
            public Align Unnamed_6 { get { return __unnamed6; } }
            public ushort? SubmeshUnk0 { get { return _submeshUnk0; } }
            public ushort? NumSubmeshes { get { return _numSubmeshes; } }
            public byte[] SubmeshUnk1 { get { return _submeshUnk1; } }
            public uint? SubmeshUnk2 { get { return _submeshUnk2; } }
            public byte[] SubmeshPad1 { get { return _submeshPad1; } }
            public ushort? Submesh2Unk0 { get { return _submesh2Unk0; } }
            public ushort? NumSubmesh2s { get { return _numSubmesh2s; } }
            public byte[] Submesh2Unk1 { get { return _submesh2Unk1; } }
            public uint? Submesh2Unk2 { get { return _submesh2Unk2; } }
            public byte[] Submesh2Pad1 { get { return _submesh2Pad1; } }
            public List<Submesh> Submeshes { get { return _submeshes; } }
            public List<Submesh2> Submesh2s { get { return _submesh2s; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Unknown3 : KaitaiStruct
        {
            public static Unknown3 FromFile(string fileName)
            {
                return new Unknown3(new KaitaiStream(fileName));
            }

            public Unknown3(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
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
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
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

            public Cityobject(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _bboxMin = new Vec3(m_io, this, m_root);
                __unnamed1 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_1, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_1, M_Io, "/types/cityobject/seq/1");
                }
                _bboxMax = new Vec3(m_io, this, m_root);
                _cullDistance = m_io.ReadF4le();
                __unnamed4 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_4, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_4, M_Io, "/types/cityobject/seq/4");
                }
                _unk0 = m_io.ReadBytes(4);
                __unnamed6 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_6, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_6, M_Io, "/types/cityobject/seq/6");
                }
                _unk1 = m_io.ReadS2le();
                __unnamed8 = m_io.ReadBytes(2);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_8, new byte[] { 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0 }, Unnamed_8, M_Io, "/types/cityobject/seq/8");
                }
                _flags = m_io.ReadBytes(4);
                _unk3 = m_io.ReadBytes(4);
                _unk8 = m_io.ReadBytes(4);
                _unk4 = m_io.ReadS4le();
                _cityobjectPartId = m_io.ReadU4le();
                _unk5 = m_io.ReadU4le();
                _unk6 = m_io.ReadS4le();
                _unk7 = m_io.ReadS4le();
            }
            private Vec3 _bboxMin;
            private byte[] __unnamed1;
            private Vec3 _bboxMax;
            private float _cullDistance;
            private byte[] __unnamed4;
            private byte[] _unk0;
            private byte[] __unnamed6;
            private short _unk1;
            private byte[] __unnamed8;
            private byte[] _flags;
            private byte[] _unk3;
            private byte[] _unk8;
            private int _unk4;
            private uint _cityobjectPartId;
            private uint _unk5;
            private int _unk6;
            private int _unk7;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public Vec3 BboxMin { get { return _bboxMin; } }
            public byte[] Unnamed_1 { get { return __unnamed1; } }
            public Vec3 BboxMax { get { return _bboxMax; } }
            public float CullDistance { get { return _cullDistance; } }
            public byte[] Unnamed_4 { get { return __unnamed4; } }
            public byte[] Unk0 { get { return _unk0; } }
            public byte[] Unnamed_6 { get { return __unnamed6; } }
            public short Unk1 { get { return _unk1; } }
            public byte[] Unnamed_8 { get { return __unnamed8; } }
            public byte[] Flags { get { return _flags; } }
            public byte[] Unk3 { get { return _unk3; } }
            public byte[] Unk8 { get { return _unk8; } }
            public int Unk4 { get { return _unk4; } }
            public uint CityobjectPartId { get { return _cityobjectPartId; } }
            public uint Unk5 { get { return _unk5; } }
            public int Unk6 { get { return _unk6; } }
            public int Unk7 { get { return _unk7; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Unknown24 : KaitaiStruct
        {
            public static Unknown24 FromFile(string fileName)
            {
                return new Unknown24(new KaitaiStream(fileName));
            }

            public Unknown24(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _boxMin = new Vec3(m_io, this, m_root);
                _unk0 = m_io.ReadU4le();
                _boxMax = new Vec3(m_io, this, m_root);
                _unk1 = m_io.ReadU4le();
                _unk2 = m_io.ReadU4le();
                __unnamed5 = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_5, new byte[] { 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0, 0, 0 }, Unnamed_5, M_Io, "/types/unknown24/seq/5");
                }
                _unk4 = m_io.ReadU4le();
                _unk5 = m_io.ReadU4le();
            }
            private Vec3 _boxMin;
            private uint _unk0;
            private Vec3 _boxMax;
            private uint _unk1;
            private uint _unk2;
            private byte[] __unnamed5;
            private uint _unk4;
            private uint _unk5;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public Vec3 BoxMin { get { return _boxMin; } }
            public uint Unk0 { get { return _unk0; } }
            public Vec3 BoxMax { get { return _boxMax; } }
            public uint Unk1 { get { return _unk1; } }
            public uint Unk2 { get { return _unk2; } }
            public byte[] Unnamed_5 { get { return __unnamed5; } }
            public uint Unk4 { get { return _unk4; } }
            public uint Unk5 { get { return _unk5; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class VertexBufferData : KaitaiStruct
        {
            public static VertexBufferData FromFile(string fileName)
            {
                return new VertexBufferData(new KaitaiStream(fileName));
            }

            public VertexBufferData(KaitaiStream p__io, Sr2ChunkPc.VertexBuffer p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _vertStride1 = m_io.ReadU1();
                _numUvChannels = m_io.ReadU1();
                _vertStride0 = m_io.ReadU2le();
                _numVertices = m_io.ReadU4le();
                __unnamed4 = m_io.ReadBytes(8);
                if (!((KaitaiStream.ByteArrayCompare(Unnamed_4, new byte[] { 255, 255, 255, 255, 0, 0, 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 255, 255, 255, 255, 0, 0, 0, 0 }, Unnamed_4, M_Io, "/types/vertex_buffer_data/seq/4");
                }
            }
            private byte _vertStride1;
            private byte _numUvChannels;
            private ushort _vertStride0;
            private uint _numVertices;
            private byte[] __unnamed4;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc.VertexBuffer m_parent;

            /// <summary>
            /// (stride of stream 1 in SRIV) Vertex normals? Correlates with extra 2B data.
            /// </summary>
            public byte VertStride1 { get { return _vertStride1; } }

            /// <summary>
            /// Number of UV channels (from 0 to 4). Each UV channel takes 4B.
            /// </summary>
            public byte NumUvChannels { get { return _numUvChannels; } }

            /// <summary>
            /// (stride of stream 0 in SRIV)
            /// </summary>
            public ushort VertStride0 { get { return _vertStride0; } }
            public uint NumVertices { get { return _numVertices; } }

            /// <summary>
            /// Pointers generated at runtime, probably
            /// </summary>
            public byte[] Unnamed_4 { get { return __unnamed4; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc.VertexBuffer M_Parent { get { return m_parent; } }
        }
        public partial class MatTexCont : KaitaiStruct
        {
            public static MatTexCont FromFile(string fileName)
            {
                return new MatTexCont(new KaitaiStream(fileName));
            }

            public MatTexCont(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
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
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public List<MatTex> TexData { get { return _texData; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class NumLightsType : KaitaiStruct
        {
            public static NumLightsType FromFile(string fileName)
            {
                return new NumLightsType(new KaitaiStream(fileName));
            }

            public NumLightsType(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_result = false;
                _read();
            }
            private void _read()
            {
                _input = m_io.ReadU4le();
            }
            private bool f_result;
            private uint _result;
            public uint Result
            {
                get
                {
                    if (f_result)
                        return _result;
                    _result = (uint) ((Input != 1212891981 ? Input : 0));
                    f_result = true;
                    return _result;
                }
            }
            private uint _input;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public uint Input { get { return _input; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        public partial class Unknown18 : KaitaiStruct
        {
            public static Unknown18 FromFile(string fileName)
            {
                return new Unknown18(new KaitaiStream(fileName));
            }

            public Unknown18(KaitaiStream p__io, Sr2ChunkPc p__parent = null, Sr2ChunkPc p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unk0 = m_io.ReadU4le();
                _numData0 = m_io.ReadU4le();
                __unnamed2 = m_io.ReadBytes((NumData0 * 12));
                _data0 = new List<uint>();
                for (var i = 0; i < NumData0; i++)
                {
                    _data0.Add(m_io.ReadU4le());
                }
                _numData1 = m_io.ReadU4le();
                __unnamed5 = m_io.ReadBytes((NumData1 * 12));
                _data1 = new List<uint>();
                for (var i = 0; i < NumData1; i++)
                {
                    _data1.Add(m_io.ReadU4le());
                }
                _numData2 = m_io.ReadU4le();
                __unnamed8 = m_io.ReadBytes((NumData2 * 12));
                _data2 = new List<uint>();
                for (var i = 0; i < NumData2; i++)
                {
                    _data2.Add(m_io.ReadU4le());
                }
            }
            private uint _unk0;
            private uint _numData0;
            private byte[] __unnamed2;
            private List<uint> _data0;
            private uint _numData1;
            private byte[] __unnamed5;
            private List<uint> _data1;
            private uint _numData2;
            private byte[] __unnamed8;
            private List<uint> _data2;
            private Sr2ChunkPc m_root;
            private Sr2ChunkPc m_parent;
            public uint Unk0 { get { return _unk0; } }
            public uint NumData0 { get { return _numData0; } }
            public byte[] Unnamed_2 { get { return __unnamed2; } }
            public List<uint> Data0 { get { return _data0; } }
            public uint NumData1 { get { return _numData1; } }
            public byte[] Unnamed_5 { get { return __unnamed5; } }
            public List<uint> Data1 { get { return _data1; } }
            public uint NumData2 { get { return _numData2; } }
            public byte[] Unnamed_8 { get { return __unnamed8; } }
            public List<uint> Data2 { get { return _data2; } }
            public Sr2ChunkPc M_Root { get { return m_root; } }
            public Sr2ChunkPc M_Parent { get { return m_parent; } }
        }
        private byte[] _magic;
        private byte[] _version;
        private byte[] __unnamed2;
        private byte[] _meshlibrary;
        private uint _header0x10;
        private byte[] _header0x14;
        private byte[] __unnamed6;
        private byte[] _headerXx;
        private byte[] __unnamed8;
        private byte[] _headerXxx;
        private byte[] __unnamed10;
        private byte[] _headerXxxx;
        private uint _lenGChunk1;
        private uint _lenGChunk2;
        private uint _numCityobjects;
        private uint _numUnknown23;
        private uint _header0x9c;
        private uint _header0xa0;
        private byte[] __unnamed18;
        private uint _numMeshMovers;
        private uint _numUnknown27;
        private uint _numUnknown28;
        private uint _numUnknown29;
        private uint _numUnknown30;
        private uint _numUnknown31;
        private uint _header0xcc;
        private uint _numUnknown26;
        private float _worldMinX;
        private float _worldMinY;
        private float _worldMinZ;
        private float _worldMaxX;
        private float _worldMaxY;
        private float _worldMaxZ;
        private float _header0xec;
        private uint _numUnknown32;
        private byte[] __unnamed35;
        private uint _numTextureNames;
        private byte[] __unnamed37;
        private List<string> _textureNames;
        private Align __unnamed39;
        private uint _numRendermodels;
        private uint _numCityobjectParts;
        private uint _modelCount;
        private uint _numUnknown3s;
        private uint _numUnknown4s;
        private Align __unnamed45;
        private List<RendermodelUnk0> _rendermodelUnk0s;
        private Align __unnamed47;
        private Offset _cityobjectPartsOffset;
        private List<CityobjectPart> _cityobjectParts;
        private Align __unnamed50;
        private List<Unknown3> _unknown3s;
        private Align __unnamed52;
        private List<Unknown4> _unknown4s;
        private Align __unnamed54;
        private uint _numBakedCollisionVertices;
        private List<Vec3> _bakedCollisionVertices;
        private uint _numBakedCollisionUnk0;
        private byte[] _bakedCollisionUnk0;
        private uint _numBakedCollisionUnk1;
        private byte[] _bakedCollisionUnk1;
        private uint _numBakedCollisionUnk2;
        private byte[] _bakedCollisionUnk2;
        private Align __unnamed63;
        private uint _lenBakedCollisionMopp;
        private Align __unnamed65;
        private byte[] _bakedCollisionMopp;
        private Align __unnamed67;
        private Vec3 _bakedCollisionAabbMin;
        private Vec3 _bakedCollisionAabbMax;
        private Align __unnamed70;
        private List<IndexBufferData> _indexBuffers;
        private List<VertexBuffer> _vertexBuffers;
        private Align __unnamed73;
        private List<PhysModelBuffer> _physModels;
        private uint _numMaterials;
        private Align __unnamed76;
        private uint _numMatShaderParams;
        private byte[] __unnamed78;
        private uint _numMatUnknown3s;
        private uint _matUnknown1;
        private List<Material> _materials;
        private List<MatConstant> _matConstants;
        private List<MatUnknown2> _matUnknown2s;
        private Align __unnamed84;
        private List<float> _matShaderParams;
        private List<MatTexCont> _matTextures;
        private List<MatUnknown3> _matUnknown3s;
        private List<byte[]> _matUnknown3Unk2;
        private List<Rendermodel> _rendermodels;
        private List<Cityobject> _cityobjects;
        private List<string> _cityobjectNames;
        private Align __unnamed92;
        private uint _lenUnknownNames;
        private byte[] _unknownNames;
        private Align __unnamed95;
        private uint _numUnknown13;
        private List<uint> _unknown13;
        private Align __unnamed98;
        private uint _lenPad17;
        private byte[] __unnamed100;
        private Align __unnamed101;
        private uint _numUnknown18s;
        private List<Unknown18> _unknown18s;
        private Align __unnamed104;
        private uint _numUnknown19;
        private byte[] __unnamed106;
        private List<float> _unknown19;
        private uint _numUnknown20;
        private byte[] __unnamed109;
        private List<byte[]> _unknown20;
        private Align __unnamed111;
        private uint _numUnknown21;
        private byte[] _unknown21Pad;
        private List<uint> _unknown21;
        private Align __unnamed115;
        private uint _numUnknown22;
        private List<uint> _unknown22;
        private Align __unnamed118;
        private List<float> _unknown23;
        private uint _numUnknown24s;
        private byte[] __unnamed121;
        private uint _numUnknown25;
        private byte[] __unnamed123;
        private List<Unknown24> _unknown24s;
        private List<byte[]> _unknown25;
        private Align __unnamed126;
        private List<byte[]> _unknown26;
        private Align __unnamed128;
        private List<MeshMover> _meshMovers;
        private List<byte[]> _unknown27;
        private List<byte[]> _unknown28;
        private List<byte[]> _unknown29;
        private List<byte[]> _unknown30;
        private List<byte[]> _unknown31;
        private List<byte[]> _unknown32;
        private List<MeshMoverName> _meshMoverNames;
        private Align __unnamed137;
        private NumLightsType _numLights;
        private Offset _lightsOffset;
        private uint? _lightUnknown;
        private List<LightDataType> _lightData;
        private List<string> _lightNames;
        private Align __unnamed143;
        private List<LightUnkfloatsType> _lightUnkfloats;
        private Align __unnamed145;
        private Sr2ChunkPc m_root;
        private KaitaiStruct m_parent;
        public byte[] Magic { get { return _magic; } }
        public byte[] Version { get { return _version; } }
        public byte[] Unnamed_2 { get { return __unnamed2; } }
        public byte[] Meshlibrary { get { return _meshlibrary; } }
        public uint Header0x10 { get { return _header0x10; } }
        public byte[] Header0x14 { get { return _header0x14; } }
        public byte[] Unnamed_6 { get { return __unnamed6; } }
        public byte[] HeaderXx { get { return _headerXx; } }
        public byte[] Unnamed_8 { get { return __unnamed8; } }
        public byte[] HeaderXxx { get { return _headerXxx; } }
        public byte[] Unnamed_10 { get { return __unnamed10; } }
        public byte[] HeaderXxxx { get { return _headerXxxx; } }
        public uint LenGChunk1 { get { return _lenGChunk1; } }
        public uint LenGChunk2 { get { return _lenGChunk2; } }
        public uint NumCityobjects { get { return _numCityobjects; } }
        public uint NumUnknown23 { get { return _numUnknown23; } }
        public uint Header0x9c { get { return _header0x9c; } }
        public uint Header0xa0 { get { return _header0xa0; } }
        public byte[] Unnamed_18 { get { return __unnamed18; } }
        public uint NumMeshMovers { get { return _numMeshMovers; } }
        public uint NumUnknown27 { get { return _numUnknown27; } }
        public uint NumUnknown28 { get { return _numUnknown28; } }
        public uint NumUnknown29 { get { return _numUnknown29; } }
        public uint NumUnknown30 { get { return _numUnknown30; } }
        public uint NumUnknown31 { get { return _numUnknown31; } }
        public uint Header0xcc { get { return _header0xcc; } }
        public uint NumUnknown26 { get { return _numUnknown26; } }
        public float WorldMinX { get { return _worldMinX; } }
        public float WorldMinY { get { return _worldMinY; } }
        public float WorldMinZ { get { return _worldMinZ; } }
        public float WorldMaxX { get { return _worldMaxX; } }
        public float WorldMaxY { get { return _worldMaxY; } }
        public float WorldMaxZ { get { return _worldMaxZ; } }
        public float Header0xec { get { return _header0xec; } }
        public uint NumUnknown32 { get { return _numUnknown32; } }
        public byte[] Unnamed_35 { get { return __unnamed35; } }
        public uint NumTextureNames { get { return _numTextureNames; } }
        public byte[] Unnamed_37 { get { return __unnamed37; } }
        public List<string> TextureNames { get { return _textureNames; } }
        public Align Unnamed_39 { get { return __unnamed39; } }
        public uint NumRendermodels { get { return _numRendermodels; } }
        public uint NumCityobjectParts { get { return _numCityobjectParts; } }
        public uint ModelCount { get { return _modelCount; } }
        public uint NumUnknown3s { get { return _numUnknown3s; } }
        public uint NumUnknown4s { get { return _numUnknown4s; } }
        public Align Unnamed_45 { get { return __unnamed45; } }
        public List<RendermodelUnk0> RendermodelUnk0s { get { return _rendermodelUnk0s; } }
        public Align Unnamed_47 { get { return __unnamed47; } }
        public Offset CityobjectPartsOffset { get { return _cityobjectPartsOffset; } }
        public List<CityobjectPart> CityobjectParts { get { return _cityobjectParts; } }
        public Align Unnamed_50 { get { return __unnamed50; } }
        public List<Unknown3> Unknown3s { get { return _unknown3s; } }
        public Align Unnamed_52 { get { return __unnamed52; } }
        public List<Unknown4> Unknown4s { get { return _unknown4s; } }
        public Align Unnamed_54 { get { return __unnamed54; } }
        public uint NumBakedCollisionVertices { get { return _numBakedCollisionVertices; } }
        public List<Vec3> BakedCollisionVertices { get { return _bakedCollisionVertices; } }
        public uint NumBakedCollisionUnk0 { get { return _numBakedCollisionUnk0; } }
        public byte[] BakedCollisionUnk0 { get { return _bakedCollisionUnk0; } }
        public uint NumBakedCollisionUnk1 { get { return _numBakedCollisionUnk1; } }
        public byte[] BakedCollisionUnk1 { get { return _bakedCollisionUnk1; } }
        public uint NumBakedCollisionUnk2 { get { return _numBakedCollisionUnk2; } }
        public byte[] BakedCollisionUnk2 { get { return _bakedCollisionUnk2; } }
        public Align Unnamed_63 { get { return __unnamed63; } }
        public uint LenBakedCollisionMopp { get { return _lenBakedCollisionMopp; } }
        public Align Unnamed_65 { get { return __unnamed65; } }
        public byte[] BakedCollisionMopp { get { return _bakedCollisionMopp; } }
        public Align Unnamed_67 { get { return __unnamed67; } }
        public Vec3 BakedCollisionAabbMin { get { return _bakedCollisionAabbMin; } }
        public Vec3 BakedCollisionAabbMax { get { return _bakedCollisionAabbMax; } }
        public Align Unnamed_70 { get { return __unnamed70; } }
        public List<IndexBufferData> IndexBuffers { get { return _indexBuffers; } }
        public List<VertexBuffer> VertexBuffers { get { return _vertexBuffers; } }
        public Align Unnamed_73 { get { return __unnamed73; } }
        public List<PhysModelBuffer> PhysModels { get { return _physModels; } }
        public uint NumMaterials { get { return _numMaterials; } }
        public Align Unnamed_76 { get { return __unnamed76; } }
        public uint NumMatShaderParams { get { return _numMatShaderParams; } }
        public byte[] Unnamed_78 { get { return __unnamed78; } }
        public uint NumMatUnknown3s { get { return _numMatUnknown3s; } }
        public uint MatUnknown1 { get { return _matUnknown1; } }
        public List<Material> Materials { get { return _materials; } }
        public List<MatConstant> MatConstants { get { return _matConstants; } }
        public List<MatUnknown2> MatUnknown2s { get { return _matUnknown2s; } }
        public Align Unnamed_84 { get { return __unnamed84; } }
        public List<float> MatShaderParams { get { return _matShaderParams; } }
        public List<MatTexCont> MatTextures { get { return _matTextures; } }
        public List<MatUnknown3> MatUnknown3s { get { return _matUnknown3s; } }
        public List<byte[]> MatUnknown3Unk2 { get { return _matUnknown3Unk2; } }
        public List<Rendermodel> Rendermodels { get { return _rendermodels; } }
        public List<Cityobject> Cityobjects { get { return _cityobjects; } }
        public List<string> CityobjectNames { get { return _cityobjectNames; } }
        public Align Unnamed_92 { get { return __unnamed92; } }
        public uint LenUnknownNames { get { return _lenUnknownNames; } }
        public byte[] UnknownNames { get { return _unknownNames; } }
        public Align Unnamed_95 { get { return __unnamed95; } }
        public uint NumUnknown13 { get { return _numUnknown13; } }
        public List<uint> Unknown13 { get { return _unknown13; } }
        public Align Unnamed_98 { get { return __unnamed98; } }
        public uint LenPad17 { get { return _lenPad17; } }
        public byte[] Unnamed_100 { get { return __unnamed100; } }
        public Align Unnamed_101 { get { return __unnamed101; } }
        public uint NumUnknown18s { get { return _numUnknown18s; } }
        public List<Unknown18> Unknown18s { get { return _unknown18s; } }
        public Align Unnamed_104 { get { return __unnamed104; } }
        public uint NumUnknown19 { get { return _numUnknown19; } }
        public byte[] Unnamed_106 { get { return __unnamed106; } }
        public List<float> Unknown19 { get { return _unknown19; } }
        public uint NumUnknown20 { get { return _numUnknown20; } }
        public byte[] Unnamed_109 { get { return __unnamed109; } }
        public List<byte[]> Unknown20 { get { return _unknown20; } }
        public Align Unnamed_111 { get { return __unnamed111; } }
        public uint NumUnknown21 { get { return _numUnknown21; } }
        public byte[] Unknown21Pad { get { return _unknown21Pad; } }
        public List<uint> Unknown21 { get { return _unknown21; } }
        public Align Unnamed_115 { get { return __unnamed115; } }
        public uint NumUnknown22 { get { return _numUnknown22; } }
        public List<uint> Unknown22 { get { return _unknown22; } }
        public Align Unnamed_118 { get { return __unnamed118; } }
        public List<float> Unknown23 { get { return _unknown23; } }
        public uint NumUnknown24s { get { return _numUnknown24s; } }
        public byte[] Unnamed_121 { get { return __unnamed121; } }
        public uint NumUnknown25 { get { return _numUnknown25; } }
        public byte[] Unnamed_123 { get { return __unnamed123; } }
        public List<Unknown24> Unknown24s { get { return _unknown24s; } }
        public List<byte[]> Unknown25 { get { return _unknown25; } }
        public Align Unnamed_126 { get { return __unnamed126; } }
        public List<byte[]> Unknown26 { get { return _unknown26; } }
        public Align Unnamed_128 { get { return __unnamed128; } }
        public List<MeshMover> MeshMovers { get { return _meshMovers; } }
        public List<byte[]> Unknown27 { get { return _unknown27; } }
        public List<byte[]> Unknown28 { get { return _unknown28; } }
        public List<byte[]> Unknown29 { get { return _unknown29; } }
        public List<byte[]> Unknown30 { get { return _unknown30; } }
        public List<byte[]> Unknown31 { get { return _unknown31; } }
        public List<byte[]> Unknown32 { get { return _unknown32; } }
        public List<MeshMoverName> MeshMoverNames { get { return _meshMoverNames; } }
        public Align Unnamed_137 { get { return __unnamed137; } }
        public NumLightsType NumLights { get { return _numLights; } }
        public Offset LightsOffset { get { return _lightsOffset; } }
        public uint? LightUnknown { get { return _lightUnknown; } }
        public List<LightDataType> LightData { get { return _lightData; } }
        public List<string> LightNames { get { return _lightNames; } }
        public Align Unnamed_143 { get { return __unnamed143; } }
        public List<LightUnkfloatsType> LightUnkfloats { get { return _lightUnkfloats; } }
        public Align Unnamed_145 { get { return __unnamed145; } }
        public Sr2ChunkPc M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
