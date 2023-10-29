using System.Numerics;
using Pangaea.Common;
using Pangaea.Rendering;
using Pangaea.Rendering.OpenGL;
using Pangaea.Rendering.OpenGL.Calls;
using Color = System.Drawing.Color;
using Size = Pangaea.Common.Size;

namespace Pangaea;

public class Canvas
{
    private readonly OpenGLRenderer _renderer;

    internal Canvas(OpenGLRenderer renderer)
    {
        _renderer = renderer;
    }
    
    public void BeginFrame(in Size windowSize)
    {
        _renderer.SetViewport(windowSize);
    }

    public void CancelFrame()
    {
        _renderer.Cancel();
    }

    public void EndFrame(Color? color = null)
    {
        _renderer.Clear(color ?? Color.White);
        _renderer.Flush();
    }

    public void DrawRect(in Rect rect, in Paint paint)
    {
        Vector2 p1 = new Vector2(rect.X, rect.Y);
        Vector2 p2 = new Vector2(rect.X + rect.W, rect.Y);
        Vector2 p3 = new Vector2(rect.X, rect.Y + rect.H);
        Vector2 p4 = new Vector2(rect.X + rect.W, rect.Y + rect.H);
        DrawRect(in p1, in p2, in p3, in p4, in paint);
    }

    public void DrawRect(in Rect rect, float degrees, in Paint paint)
    {
        Vector2 center = new Vector2(rect.X, rect.Y);
        
        Vector2 p1 = Rotate(new Vector2(rect.X, rect.Y), center, degrees);
        Vector2 p2 = Rotate(new Vector2(rect.X + rect.W, rect.Y), center, degrees);
        Vector2 p3 = Rotate(new Vector2(rect.X, rect.Y + rect.H), center, degrees);
        Vector2 p4 = Rotate(new Vector2(rect.X + rect.W, rect.Y + rect.H), center, degrees);
        
        DrawRect(in p1, in p2, in p3, in p4, in paint);
    }
    
    public void DrawRect(in Rect rect, in Vector2 center, float degrees, in Paint paint)
    {
        Vector2 p1 = Rotate(new Vector2(rect.X, rect.Y), center, degrees);
        Vector2 p2 = Rotate(new Vector2(rect.X + rect.W, rect.Y), center, degrees);
        Vector2 p3 = Rotate(new Vector2(rect.X, rect.Y + rect.H), center, degrees);
        Vector2 p4 = Rotate(new Vector2(rect.X + rect.W, rect.Y + rect.H), center, degrees);
        
        DrawRect(in p1, in p2, in p3, in p4, in paint);
    }

    public void DrawRect(in Vector2 p1, in Vector2 p2, in Vector2 p3, in Vector2 p4, in Paint paint)
    {
        Vertex[] vertices = new Vertex[]
        {
            new Vertex(p1.X, p1.Y, 0.0f, 0.0f),
            new Vertex(p2.X, p2.Y, 0.0f, 0.0f),
            new Vertex(p4.X, p4.Y, 0.0f, 0.0f),
            new Vertex(p1.X, p1.Y, 0.0f, 0.0f),
            new Vertex(p3.X, p3.Y, 0.0f, 0.0f),
            new Vertex(p4.X, p4.Y, 0.0f, 0.0f),
        };
        
        Vector2 center = new Vector2((p1.X + p2.X + p3.X + p4.X) * 0.25f, (p1.Y+ p2.Y + p3.Y + p4.Y) * 0.25f);

        _renderer.AddVertices(vertices, DrawCallType.Triangle, center, new TriangleDrawCallParam());
    }
    
    public void DrawCircle(in Vector2 center, float radius, in Paint paint)
    {
        DrawCircle(in center, radius, in paint, 60);
    }

    public void DrawCircle(in Vector2 center, float radius, in Paint paint, int segments)
    {
        float increment = 2.0f * MathF.PI / segments;
        
        float angle = 0f;
        Vertex[] vertices = new Vertex[segments];
        for (int i = 0; i < segments; ++i)
        {
            vertices[i] = new Vertex(radius * MathF.Cos(angle) + center.X, radius * MathF.Sin(angle) + center.Y, 0f, 0f);
            angle += increment;
        }

        CircleDrawCallParam param = new CircleDrawCallParam();
        
        if (paint.StrokeWidth.HasValue)
            param.InnerRadius = radius - paint.StrokeWidth.Value;
        
        _renderer.AddVertices(vertices, DrawCallType.TriangleFan, center, param);
    }

    private static Vector2 Rotate(Vector2 point, Vector2 center, float degrees)
    {
        float angle = degrees * MathF.PI / 180.0f;
        return new Vector2(
            MathF.Cos(angle) * (point.X - center.X) - MathF.Sin(angle) * (point.Y - center.Y) + center.X,
            MathF.Sin(angle) * (point.X - center.X) + MathF.Cos(angle) * (point.Y - center.Y) + center.Y
        );
    }
}
