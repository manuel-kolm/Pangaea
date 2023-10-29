using System.Numerics;
using Silk.NET.OpenGL;
using Shader = Pangaea.Rendering.OpenGL.Shaders.Shader;

namespace Pangaea.Rendering.OpenGL.Calls;

public class CircleDrawCall : DrawCall
{
    private readonly GL _gl;
    private readonly Shader _shader;
    private readonly int _offset;
    private readonly uint _count;
    private readonly Vector2 _center;
    private readonly CircleDrawCallParam _param;
    
    public CircleDrawCall(GL gl, Shader shader, int offset, uint count, Vector2 center, DrawCallParam param)
    {
        _gl = gl;
        _shader = shader;
        _offset = offset;
        _count = count;
        _center = center;
        _param = (CircleDrawCallParam) param;
    }
    
    public override void Run()
    {
        _shader.SetUniform("type", 2);
        _shader.SetUniform("center", _center);
        _shader.SetUniform("innerRadius", _param.InnerRadius ?? -1);
        _gl.DrawArrays(PrimitiveType.TriangleFan, _offset, _count);
    }
}
