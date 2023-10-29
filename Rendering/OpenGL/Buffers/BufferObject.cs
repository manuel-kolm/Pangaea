using Pangaea.Common;
using Silk.NET.OpenGL;

namespace Pangaea.Rendering.OpenGL.Buffers;

internal class BufferObject : IDisposable
{
    private readonly GL _gl;
    private readonly uint _handle;

    internal BufferObject(GL gl)
    {
        _gl = gl;
        _handle = _gl.GenBuffer();
    }
    
    internal unsafe void Update(ReadOnlySpan<Vertex> vertices)
    {
        _gl.BufferData(BufferTargetARB.ArrayBuffer, (uint)(vertices.Length * sizeof(Vertex)), vertices, BufferUsageARB.StreamDraw);
        _gl.EnableVertexAttribArray(0);
        _gl.EnableVertexAttribArray(1);
        _gl.VertexAttribPointer(0, 2, GLEnum.Float, false, (uint)sizeof(Vertex), (void*)0);
        _gl.VertexAttribPointer(1, 2, GLEnum.Float, false, (uint)sizeof(Vertex), (void*)(0 + (2 * sizeof(float))));
    }
    
    internal void Bind()
    {
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _handle);
    }
    
    internal void Unbind()
    {
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
    }
    
    public void Dispose()
    {
        _gl.DeleteBuffer(_handle);
    }
}
