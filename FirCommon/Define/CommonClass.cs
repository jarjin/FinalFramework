using ProtoBuf;
using UnityEngine;

namespace FirCommon.Data
{
    [ProtoContract]
    public sealed class SurrogateVector2
    {
        [ProtoMember(1)]
        float x;
        [ProtoMember(2)]
        float y;

        public SurrogateVector2() { }

        public SurrogateVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector3(SurrogateVector2 v)
        {
            return new Vector3(v.x, v.y);
        }

        public static implicit operator SurrogateVector2(Vector2 v)
        {
            return new SurrogateVector2(v.x, v.y);
        }
    }

    [ProtoContract]
    public sealed class SurrogateVector3
    {
        [ProtoMember(1)]
        float x;
        [ProtoMember(2)]
        float y;
        [ProtoMember(3)]
        float z;

        public SurrogateVector3() { }

        public SurrogateVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3(SurrogateVector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static implicit operator SurrogateVector3(Vector3 v)
        {
            return new SurrogateVector3(v.x, v.y, v.z);
        }
    }

    [ProtoContract]
    public sealed class SurrogateColor
    {
        [ProtoMember(1)]
        float r;
        [ProtoMember(2)]
        float g;
        [ProtoMember(3)]
        float b;
        [ProtoMember(4)]
        float a;

        public SurrogateColor() { }

        public SurrogateColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator Color(SurrogateColor v)
        {
            return new Color(v.r, v.g, v.b, v.a);
        }

        public static implicit operator SurrogateColor(Color v)
        {
            return new SurrogateColor(v.r, v.g, v.b, v.a);
        }
    }

    [ProtoContract]
    public sealed class SurrogateColor32
    {
        [ProtoMember(1)]
        byte r;
        [ProtoMember(2)]
        byte g;
        [ProtoMember(3)]
        byte b;
        [ProtoMember(4)]
        byte a;

        public SurrogateColor32() { }

        public SurrogateColor32(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static implicit operator Color32(SurrogateColor32 v)
        {
            return new Color32(v.r, v.g, v.b, v.a);
        }

        public static implicit operator SurrogateColor32(Color32 v)
        {
            return new SurrogateColor32(v.r, v.g, v.b, v.a);
        }
    }
}

