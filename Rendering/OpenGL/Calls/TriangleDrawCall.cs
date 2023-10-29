using System.Numerics;
using Silk.NET.OpenGL;
using Shader = Pangaea.Rendering.OpenGL.Shaders.Shader;

namespace Pangaea.Rendering.OpenGL.Calls;

public class TriangleDrawCall : DrawCall
{
    private readonly GL _gl;
    private readonly Shader _shader;
    private readonly int _offset;
    private readonly uint _count;
    private readonly Vector2 _center;

    public TriangleDrawCall(GL gl, Shader shader, int offset, uint count, Vector2 center)
    {
        _gl = gl;
        _shader = shader;
        _offset = offset;
        _count = count;
        _center = center;
    }
    
    public override void Run()
    {
        _shader.SetUniform("type", 1);
        _shader.SetUniform("center", _center);
        _gl.DrawArrays(PrimitiveType.Triangles, _offset, _count);
    }
}
