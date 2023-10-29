using System.Numerics;
using Pangaea.Common;
using Pangaea.Rendering.OpenGL.Buffers;
using Pangaea.Rendering.OpenGL.Calls;
using Pangaea.Rendering.OpenGL.Shaders;
using Silk.NET.OpenGL;
using Color = System.Drawing.Color;
using Shader = Pangaea.Rendering.OpenGL.Shaders.Shader;
using Size = Pangaea.Common.Size;

namespace Pangaea.Rendering.OpenGL;

public class OpenGLRenderer : IDisposable
{ 
    private readonly GL _gl;
    private readonly VertexArrayObject _vao;
    private readonly BufferObject _vbo;
    private readonly Shader _shader;
    private readonly VertexBuffer _vertexBuffer;
    private readonly DrawCallQueue _drawCallQueue;
    private readonly Window _window;

    public OpenGLRenderer(GL gl)
    {
        _gl = gl;
        _vao = new VertexArrayObject(gl);
        _vbo = new BufferObject(gl);
        _shader = new Shader(gl, ShaderSource.VERTEX_SHADER_SOURCE, ShaderSource.FRAGMENT_SHADER_SOURCE);
        _vertexBuffer = new VertexBuffer();
        _drawCallQueue = new DrawCallQueue();
        _window = new Window();
    }
    
    public void Clear(Color color)
    {
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        _gl.ClearColor(color);
    }

    public void SetViewport(in Size windowSize)
    {
        _window.Size = windowSize;
        _window.HalfHeight = windowSize.Height * 0.5f;
        _window.HalfWidth = windowSize.Width * 0.5f;
        _gl.Viewport(0, 0, (uint) windowSize.Width, (uint) windowSize.Height);
    }

    public void Cancel()
    {
        _vertexBuffer.Clear();
    }

    public void Flush()
    {
        if (_drawCallQueue.IsEmpty())
            return;
        
        _shader.Use();
        _shader.SetUniform("viewSize", new Vector2(_window.Size.Width, _window.Size.Height));
        
        _vao.Bind();
        _vbo.Bind();
        
        _vbo.Update(_vertexBuffer.ConvertToSpan());
        
        _drawCallQueue.Execute();
        
        _vbo.Unbind();
        _vao.Unbind();
    }
    
    public void AddVertices(in Vertex[] vertices, DrawCallType drawCallType, Vector2 center, DrawCallParam param)
    {
        DrawCall drawCall = drawCallType switch
        {
            DrawCallType.Triangle => new TriangleDrawCall(_gl, _shader, _vertexBuffer.CurrentOffset(),
                (uint) vertices.Length, center),
            DrawCallType.TriangleFan => new CircleDrawCall(_gl, _shader, _vertexBuffer.CurrentOffset(),
                (uint) vertices.Length, center, param),
            _ => throw new NotImplementedException()
        };
        
        _drawCallQueue.Add(drawCall);
        _vertexBuffer.Add(vertices);
    }
    
    public void Dispose()
    {
        _shader.Dispose();
        _gl.Dispose();
    }
}
