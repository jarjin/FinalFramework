using UnityEngine;

namespace FirCommon.Data
{
    public class FVector2
    {
        float x;
        float y;

        public FVector2() { }

        public FVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 ToValue()
        {
            return new Vector2(x, y);
        }
    }

    public class FVector3
    {
        float x;
        float y;
        float z;

        public FVector3() { }

        public FVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 ToValue()
        {
            return new Vector3(x, y, z);
        }
    }

    public class FColor
    {
        float r;
        float g;
        float b;
        float a;

        public FColor() { }

        public FColor(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color ToValue()
        {
            return new Color(r, g, b, a);
        }
    }

    public class FColor32
    {
        byte r;
        byte g;
        byte b;
        byte a;

        public FColor32() { }

        public FColor32(byte r, byte g, byte b, byte a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public Color32 ToValue()
        {
            return new Color(r, g, b, a);
        }
    }
}

