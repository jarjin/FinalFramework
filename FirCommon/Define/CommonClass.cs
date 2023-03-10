using System.Runtime.Serialization;
using UnityEngine;

namespace FirCommon.Data
{
    public sealed class Vector2SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector2 v = (Vector2)obj;
            info.AddValue("x", v.x);
            info.AddValue("y", v.y);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector2 v = (Vector2)obj;
            v.x = info.GetSingle("x");
            v.y = info.GetSingle("y");
            return (object)v;
        }
    }

    public sealed class Vector3SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Vector3 v = (Vector3)obj;
            info.AddValue("x", v.x);
            info.AddValue("y", v.y);
            info.AddValue("z", v.z);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 v = (Vector3)obj;
            v.x = info.GetSingle("x");
            v.y = info.GetSingle("y");
            v.z = info.GetSingle("z");
            return (object)v;
        }
    }

    public sealed class ColorSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color v = (Color)obj;
            info.AddValue("r", v.r);
            info.AddValue("g", v.g);
            info.AddValue("b", v.b);
            info.AddValue("a", v.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color v = (Color)obj;
            v.r = info.GetSingle("r");
            v.g = info.GetSingle("g");
            v.b = info.GetSingle("b");
            v.a = info.GetSingle("a");
            return (object)v;
        }
    }

    public sealed class Color32SerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Color32 v = (Color32)obj;
            info.AddValue("r", v.r);
            info.AddValue("g", v.g);
            info.AddValue("b", v.b);

            info.AddValue("a", v.a);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Color32 v = (Color32)obj;
            v.r = info.GetByte("r");
            v.g = info.GetByte("g");
            v.b = info.GetByte("b");
            v.a = info.GetByte("a");
            return (object)v;
        }
    }
}

