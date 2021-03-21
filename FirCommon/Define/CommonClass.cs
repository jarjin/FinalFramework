using LiteNetLib;
using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

    public class ClassTypeBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            return null;
        }
    }

    public class SerializeUtil
    {
        public static void Serialize(string binraryPath, object instance)
        {
            IFormatter serializer = new BinaryFormatter();
            SurrogateSelector selector = new SurrogateSelector();
            var context = new StreamingContext(StreamingContextStates.All);
            selector.AddSurrogate(typeof(Vector2), context, new Vector2SerializationSurrogate());
            selector.AddSurrogate(typeof(Vector3), context, new Vector3SerializationSurrogate());
            selector.AddSurrogate(typeof(Color), context, new ColorSerializationSurrogate());
            selector.AddSurrogate(typeof(Color32), context, new Color32SerializationSurrogate());
            serializer.SurrogateSelector = selector;
            using (var saveFile = new FileStream(binraryPath, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(saveFile, instance);
            }
        }

        public static T Deserialize<T>(string fullPath) where T : class
        {
            IFormatter serializer = new BinaryFormatter();
            SurrogateSelector selector = new SurrogateSelector();
            var context = new StreamingContext(StreamingContextStates.All);
            selector.AddSurrogate(typeof(Vector2), context, new Vector2SerializationSurrogate());
            selector.AddSurrogate(typeof(Vector3), context, new Vector3SerializationSurrogate());
            selector.AddSurrogate(typeof(Color), context, new ColorSerializationSurrogate());
            selector.AddSurrogate(typeof(Color32), context, new Color32SerializationSurrogate());
            serializer.SurrogateSelector = selector;
            using (var loadFile = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                return serializer.Deserialize(loadFile) as T;
            }
        }
    }

    public class ClientPeer
    {
        public NetPeer peer;

        public int Id
        {
            get { return peer.Id; }
        }

        public IPEndPoint EndPoint
        {
            get { return peer.EndPoint; }
        }

        public ClientPeer(NetPeer peer)
        {
            this.peer = peer; 
        }
    }
}