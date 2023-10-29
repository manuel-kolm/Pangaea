using System.Drawing;
using System.Numerics;
using Pangaea.Common;
using Pangaea.Rendering.OpenGL.Buffers;
using Pangaea.Rendering.OpenGL.Calls;
using Pangaea.Rendering.OpenGL.Shaders;
using Silk.NET.OpenGL;
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

    public void Flush()
    {
        if (_drawCallQueue.IsEmpty())
            return;
        
        _shader.Use();
        _shader.SetUniform("uViewSize", new Vector2(_window.Size.Width, _window.Size.Height));
        
        _vao.Bind();
        _vbo.Bind();
        
        _vbo.Update(_vertexBuffer.ConvertToSpan());
        
        _drawCallQueue.Execute();
        
        _vbo.Unbind();
        _vao.Unbind();
    }
    
    public void DrawRect(in Rect rect, in Paint paint)
    {
        float a = rect.X;
        float b = rect.Y;
        float c = rect.X + rect.W;
        float d = rect.Y + rect.H;
        
        Vertex[] vertices = new Vertex[]
        {
            new Vertex(a, b, 0.0f, 0.0f),
            new Vertex(c, b, 0.0f, 0.0f),
            new Vertex(c, d, 0.0f, 0.0f),
            new Vertex(c, d, 0.0f, 0.0f),
            new Vertex(a, d, 0.0f, 0.0f),
            new Vertex(a, b, 0.0f, 0.0f),
        };
        
        DrawCall drawCall = new TriangleDrawCall(_gl, _shader, _vertexBuffer.CurrentOffset(), (uint) vertices.Length);
        
        _drawCallQueue.Add(drawCall);
        _vertexBuffer.Add(vertices);
    }

    public void DrawCircle(in Vector2 center, float radius)
    {
        
        float x = center.X;
        float y = center.Y;
        
        float increment = 2.0f * MathF.PI / 60; // segments
        List<Vertex> vertices = new List<Vertex>(61);
        for (float currAngle = 0.0f; currAngle <= 2.0f * MathF.PI; currAngle += increment)
        {
            vertices.Add(new Vertex(radius * MathF.Cos(currAngle) + x, radius * MathF.Sin(currAngle) + y, 0f, 0f));
        }
        
        DrawCall drawCall = new CircleDrawCall(_gl, _shader, _vertexBuffer.CurrentOffset(), (uint) vertices.Count);
        
        _vertexBuffer.Add(vertices.ToArray());
        _drawCallQueue.Add(drawCall);
    }
    
    public void Dispose()
    {
        _shader.Dispose();
        _gl.Dispose();
    }
}
