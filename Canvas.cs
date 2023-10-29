using System.Drawing;
using System.Numerics;
using Pangaea.Common;
using Pangaea.Rendering.OpenGL;
using Size = Pangaea.Common.Size;

namespace Pangaea;

public class Canvas
{
    private readonly OpenGLRenderer _renderer;

    internal Canvas(OpenGLRenderer renderer)
    {
        _renderer = renderer;
    }
    
    public void BeginFrame(in Color color, in Size windowSize)
    {
        _renderer.Clear(color);
        _renderer.SetViewport(windowSize);
    }

    public void EndFrame()
    {
        _renderer.Flush();
    }

    public void DrawRect(in Rect rect, in Paint paint)
    {
        _renderer.DrawRect(in rect, in paint);
    }

    public void DrawCircle(in Vector2 center, float radius)
    {
        _renderer.DrawCircle(in center, radius);
    }
}
