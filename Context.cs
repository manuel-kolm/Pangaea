using Pangaea.Rendering.OpenGL;
using Silk.NET.OpenGL;

namespace Pangaea;

public class Context
{
    private Canvas _canvas;

    public Canvas Canvas => _canvas;
    
    private Context(GL gl)
    {
        _canvas = new Canvas(new OpenGLRenderer(gl));
    }
    
    public static Context Create(Func<string, IntPtr> getProcAddr)
    {
        return new Context(GL.GetApi(getProcAddr));
    }
}
