using Pangaea.Common;
using Silk.NET.OpenGL;

namespace Pangaea.Rendering.OpenGL.Buffers;

internal class VertexArrayObject : IDisposable
{
    private readonly GL _gl;
    private readonly uint _handle;
    
    internal unsafe VertexArrayObject(GL gl)
    {
        _gl = gl;
        _handle = _gl.GenVertexArray();
        
        Bind();
        
        _gl.EnableVertexAttribArray(0);
        _gl.EnableVertexAttribArray(1);
        _gl.VertexAttribPointer(0, 2, GLEnum.Float, false, (uint)sizeof(Vertex), (void*)0);
        _gl.VertexAttribPointer(1, 2, GLEnum.Float, false, (uint)sizeof(Vertex), (void*)(0 + 2 * sizeof(float)));
    }
    
    internal void Bind()
    {
        _gl.BindVertexArray(_handle);
    }
    
    internal void Unbind()
    {
        _gl.BindVertexArray(0);
    }

    public void Dispose()
    {
        _gl.DeleteVertexArray(_handle);
    }
}
