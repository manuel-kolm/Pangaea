using System.Numerics;
using System.Runtime.InteropServices;

namespace Pangaea.Common;

[StructLayout(LayoutKind.Explicit)]
public struct Vertex
{
    [FieldOffset(0 * sizeof(float))]
    public float X;
    
    [FieldOffset(1 * sizeof(float))]
    public float Y;
    
    [FieldOffset(2 * sizeof(float))]
    public float U;
    
    [FieldOffset(3 * sizeof(float))]
    public float V;

    public Vector2 Position => new(X, Y);

    public Vector2 TextureCoordinates => new(U, V);
    
    public Vertex() : this(0f, 0f, 0f, 0f) { }

    public Vertex(float x, float y, float u, float v)
    {
        X = x;
        Y = y;
        U = u;
        V = v;
    }
    
    public override string ToString()
    {
        string @base = base.ToString() ?? string.Empty;
        return @base + " [" + X + ", " + Y + ", " + U + ", " + V + "]";
    }
}
