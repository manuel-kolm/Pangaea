using Silk.NET.OpenGL;
using Shader = Pangaea.Rendering.OpenGL.Shaders.Shader;

namespace Pangaea.Rendering.OpenGL.Calls;

public class TriangleDrawCall : DrawCall
{
    private readonly GL _gl;
    private readonly Shader _shader;
    private readonly int _offset;
    private readonly uint _count;

    public TriangleDrawCall(GL gl, Shader shader, int offset, uint count)
    {
        _gl = gl;
        _shader = shader;
        _offset = offset;
        _count = count;
    }
    
    public override void Run()
    {
        _gl.DrawArrays(PrimitiveType.Triangles, _offset, _count);
    }
}
