using Pangaea.Common;

namespace Pangaea.Rendering;

public class VertexBuffer
{
    private List<Vertex> _vertices;

    public VertexBuffer()
    {
        _vertices = new List<Vertex>();
    }

    public int CurrentOffset()
    {
        return _vertices.Count;
    }
    
    public void Add(Vertex[] vertices)
    {
        _vertices.AddRange(vertices);
    }

    public Span<Vertex> ConvertToSpan()
    {
        Span<Vertex> result = new Span<Vertex>(_vertices.ToArray());
        Clear();
        return result;
    }

    public void Clear()
    {
        _vertices.Clear();
    }
}
