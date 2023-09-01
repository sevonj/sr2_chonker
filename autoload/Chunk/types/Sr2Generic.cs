using System;
using System.IO;
using System.Runtime.InteropServices;

/// This is probably unnecessary.
public static class Sr2Generic
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Sr2Vector3
    {
        public Single X;
        public Single Y;
        public Single Z;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Sr2Vector4
    {
        public Single X;
        public Single Y;
        public Single Z;
        public Single W;
    }
    public struct NullTerminatedString
    {
        public string Str;
        public NullTerminatedString(FileStream fs) : this()
        {
            BinaryReader br = new BinaryReader(fs);
            while (true)
            {
                char c = br.ReadChar();
                if ((int)c == 0) break;
                this.Str = this.Str + c;
            }
        }
    }
}
