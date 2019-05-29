using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Class
{

    public float x;
    public float y;
    public float z;
    // Use this for initialization
    public Vector3Class()
    {
        x = 0;
        y = 0;
        z = 0;
    }
    public Vector3Class(float a, float b, float c)
    {
        x = a;
        y = b;
        z = c;
    }
    public Vector3Class(Vector3 vec)
    {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }
    public void SetValues(Vector3 vec)
    {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }
    public Vector3 GetValues()
    {
        return new Vector3(x, y, z);
    }
    public float DotProduct(Vector3Class v1, Vector3Class v2)
    {
        return (v1.x * v2.x) + (v1.y + v2.y) + (v1.z + v2.z);
    }
    public Vector3Class CrossProduct(Vector3Class v1, Vector3Class v2)
    {
        Vector3Class v3 = new Vector3Class();
        //sarrus
        v3.x = (v1.y * v2.z) - (v1.z * v2.y);
        v3.y = (-1 * (v1.x * v2.z)) + (v1.z * v2.x);
        v3.z = (v1.x * v2.y) - (v1.y * v2.z);
        return v3;
    }
    public float Size()
    {
        return Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2) + Mathf.Pow(z, 2));
    }
    public float Distance(Vector3Class v1, Vector3Class v2)
    {
        Vector3Class v3 = new Vector3Class();
        v3 = v1 - v2;

        return v3.Size();
    }
    public Vector3Class Normalize(Vector3Class v)
    {
        float sizeOfV = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2) + Mathf.Pow(v.z, 2));
        Vector3Class normalizedV = new Vector3Class(v.x / sizeOfV, v.y / sizeOfV, v.z / sizeOfV);

        return normalizedV;
    }
    public static Vector3Class operator +(Vector3Class v1, Vector3Class v2)
    {
        Vector3Class v3 = new Vector3Class();
        v3.x = v1.x + v2.x;
        v3.y = v1.y + v2.y;
        v3.z = v1.z + v2.z;
        return v3;
    }

    public static Vector3Class operator -(Vector3Class v1, Vector3Class v2)
    {
        Vector3Class v3 = new Vector3Class();
        v3.x = v1.x - v2.x;
        v3.y = v1.y - v2.y;
        v3.z = v1.z - v2.z;
        return v3;
    }
    public static Vector3Class operator *(Vector3Class v, float scalar)
    {
        Vector3Class vr = new Vector3Class(v.x * scalar, v.y * scalar, v.z * scalar);
        return vr;
    }
    public static Vector3Class operator /(Vector3Class v, float scalar)
    {
        Vector3Class vr = new Vector3Class(v.x / scalar, v.y / scalar, v.z / scalar);
        return vr;
    }
}