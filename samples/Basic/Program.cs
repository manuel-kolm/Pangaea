using System.Numerics;
using Pangaea;
using Pangaea.Common;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Size = Pangaea.Common.Size;

namespace Basic;

internal static class Program
{
    private static IWindow window;
    private static Context context;
    private static Canvas canvas;
    private static float degrees;
    
    private static void Main(string[] args)
    {
        var options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "Pangaea.Simple";
        options.FramesPerSecond = 60;
        window = Window.Create(options);

        window.Load += OnLoad;
        window.Render += OnRender;

        window.Run();
        window.Dispose();
    }

    private static void OnLoad()
    {
        context = Context.Create(name => window.GLContext!.TryGetProcAddress(name, out var addr) ? addr : 0);
        canvas = context.Canvas;
    }

    private static void OnRender(double delta)
    {
        degrees += (float) delta * 10f;
        
        Size windowSize = new Size()
        {
            Width = window.Size.X,
            Height = window.Size.Y
        };
        canvas.BeginFrame(in windowSize);

        Rect rect = new Rect()
        {
            X = 100,
            Y = 100,
            W = 150,
            H = 150,
        };
        Paint paint = new Paint();
        canvas.DrawRect(in rect, in paint);
        
        Rect rect2 = new Rect()
        {
            X = 250,
            Y = 250,
            W = 150,
            H = 150,
        };
        Vector2 center = new Vector2(325, 325);
        Paint paint2 = new Paint();
        canvas.DrawRoundedRect(in rect2, in center, new Vector2(10, 10), degrees, in paint2);

        Rect rect3 = new Rect()
        {
            X = 400,
            Y = 400,
            W = 150,
            H = 150,
        };
        Paint paint3 = new Paint();
        canvas.DrawRect(in rect3, in paint3);
        
        Rect rect4 = new Rect()
        {
            X = 550,
            Y = 250,
            W = 150,
            H = 150,
        };
        Paint paint4 = new Paint();
        canvas.DrawRect(in rect4, degrees, in paint4);
        
        Rect rect5 = new Rect()
        {
            X = 400,
            Y = 100,
            W = 150,
            H = 150,
        };
        Paint paint5 = new Paint();
        canvas.DrawRect(in rect5, in paint5);
        
        Rect rect6 = new Rect()
        {
            X = 600,
            Y = 400,
            W = 150,
            H = 150,
        };
        Paint paint6 = new Paint();
        canvas.DrawRoundedRect(in rect6, new Vector2(675, 475), new Vector2(40, 40), degrees, in paint6);
        
        canvas.DrawCircle(new Vector2(150, 400), 100f, new Paint()
        {
            StrokeWidth = 5f
        });
        
        canvas.EndFrame();
    }
}
