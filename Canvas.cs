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

    public void DrawRoundedRect(in Rect rect, in Vector2 radius, float degrees, in Paint paint)
    {
        Vector2 center = new Vector2(rect.X, rect.Y);
        
        DrawRoundedRect(in rect, in center, in radius, degrees, in paint);
    }

    public void DrawRoundedRect(in Rect rect, in Vector2 center, in Vector2 radius, float degrees, in Paint paint)
    {
        Vector2 p1 = new Vector2(rect.X, rect.Y) + radius;
        Vector2 p2 = new Vector2(rect.X + rect.W, rect.Y) + new Vector2(-radius.X, radius.Y);
        Vector2 p3 = new Vector2(rect.X, rect.Y + rect.H) + new Vector2(radius.X, -radius.Y);
        Vector2 p4 = new Vector2(rect.X + rect.W, rect.Y + rect.H) - radius;

        Vector2 r1 = Rotate(p1, center, degrees);
        Vector2 r2 = Rotate(p2, center, degrees);
        Vector2 r3 = Rotate(p3, center, degrees);
        Vector2 r4 = Rotate(p4, center, degrees);

        Vector2 r5 = Vector2.Normalize(r3 - r1) * (rect.W - radius.X) + r1;
        Vector2 r6 = Vector2.Normalize(r4 - r2) * (rect.H - radius.Y) + r2;

        Vector2 r7 = Vector2.Normalize(r2 - r1) * (rect.W - radius.X) + r1;
        Vector2 r8 = Vector2.Normalize(r4 - r3) * (rect.W - radius.X) + r3;
        
        Vector2 r9 = Vector2.Normalize(r1 - r3) * (rect.H - radius.Y) + r3;
        Vector2 r10 = Vector2.Normalize(r2 - r4) * (rect.H - radius.Y) + r4;
        
        Vector2 r11 = Vector2.Normalize(r1 - r2) * (rect.W - radius.X) + r2;
        Vector2 r12 = Vector2.Normalize(r3 - r4) * (rect.W - radius.X) + r4;
        
        Vertex[] vertices = new Vertex[]
        {
            new Vertex(r1.X, r1.Y, 0.0f, 0.0f),
            new Vertex(r2.X, r2.Y, 0.0f, 0.0f),
            new Vertex(r4.X, r4.Y, 0.0f, 0.0f),
            new Vertex(r1.X, r1.Y, 0.0f, 0.0f),
            new Vertex(r3.X, r3.Y, 0.0f, 0.0f),
            new Vertex(r4.X, r4.Y, 0.0f, 0.0f),

            new Vertex(r1.X, r1.Y, 0.0f, 0.0f),
            new Vertex(r3.X, r3.Y, 0.0f, 0.0f),
            new Vertex(r12.X, r12.Y, 0.0f, 0.0f),
            new Vertex(r1.X, r1.Y, 0.0f, 0.0f),
            new Vertex(r11.X, r11.Y, 0.0f, 0.0f),
            new Vertex(r12.X, r12.Y, 0.0f, 0.0f),
            
            new Vertex(r1.X, r1.Y, 0.0f, 0.0f),
            new Vertex(r9.X, r9.Y, 0.0f, 0.0f),
            new Vertex(r10.X, r10.Y, 0.0f, 0.0f),
            new Vertex(r1.X, r1.Y, 0.0f, 0.0f),
            new Vertex(r2.X, r2.Y, 0.0f, 0.0f),
            new Vertex(r10.X, r10.Y, 0.0f, 0.0f),
            
            new Vertex(r2.X, r2.Y, 0.0f, 0.0f),
            new Vertex(r7.X, r7.Y, 0.0f, 0.0f),
            new Vertex(r8.X, r8.Y, 0.0f, 0.0f),
            new Vertex(r2.X, r2.Y, 0.0f, 0.0f),
            new Vertex(r4.X, r4.Y, 0.0f, 0.0f),
            new Vertex(r8.X, r8.Y, 0.0f, 0.0f),
            
            new Vertex(r3.X, r3.Y, 0.0f, 0.0f),
            new Vertex(r5.X, r5.Y, 0.0f, 0.0f),
            new Vertex(r6.X, r6.Y, 0.0f, 0.0f),
            new Vertex(r3.X, r3.Y, 0.0f, 0.0f),
            new Vertex(r4.X, r4.Y, 0.0f, 0.0f),
            new Vertex(r6.X, r6.Y, 0.0f, 0.0f),
        };
        
        _renderer.AddVertices(vertices, DrawCallType.Triangle, center, new TriangleDrawCallParam());
        
        float increment = 2.0f * MathF.PI / 60;
        float angleOffset = degrees * MathF.PI / 180f;
        
        List<Vertex> vertices2 = new List<Vertex>();
        
        float angle = 0f;
        vertices2.Add(new Vertex(r4.X, r4.Y, 0f, 0f));
        for (int i = 0; i < 16; ++i)
        {
            vertices2.Add(new Vertex(radius.X * MathF.Cos(angle + angleOffset) + r4.X,
                radius.Y * MathF.Sin(angle + angleOffset) + r4.Y, 0f, 0f));
            angle += increment;
        }

        angle = 15f * increment;
        vertices2.Add(new Vertex(r3.X, r3.Y, 0f, 0f));
        for (int i = 15; i < 31; ++i)
        {
            vertices2.Add(new Vertex(radius.X * MathF.Cos(angle + angleOffset) + r3.X,
                radius.Y * MathF.Sin(angle + angleOffset) + r3.Y, 0f, 0f));
            angle += increment;
        }
        
        angle = 30f * increment;
        vertices2.Add(new Vertex(r1.X, r1.Y, 0f, 0f));
        for (int i = 30; i < 46; ++i)
        {
            vertices2.Add(new Vertex(radius.X * MathF.Cos(angle + angleOffset) + r1.X,
                radius.Y * MathF.Sin(angle + angleOffset) + r1.Y, 0f, 0f));
            angle += increment;
        }
        
        angle = 45f * increment;
        vertices2.Add(new Vertex(r2.X, r2.Y, 0f, 0f));
        for (int i = 45; i < 61; ++i)
        {
            vertices2.Add(new Vertex(radius.X * MathF.Cos(angle + angleOffset) + r2.X,
                radius.Y * MathF.Sin(angle + angleOffset) + r2.Y, 0f, 0f));
            angle += increment;
        }

        _renderer.AddVertices(vertices2.ToArray(), DrawCallType.TriangleFan, center, new CircleDrawCallParam());
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
    
    private static Vector2 Rotate(Vector2 point, float degrees)
    {
        float angle = degrees * MathF.PI / 180.0f;
        return new Vector2(
            MathF.Cos(angle) * (point.X) - MathF.Sin(angle) * (point.Y),
            MathF.Sin(angle) * (point.X) + MathF.Cos(angle) * (point.Y)
        );
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
