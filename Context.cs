using Pangaea.Rendering.OpenGL;
using Silk.NET.OpenGL;

namespace Pangaea;

public class Context
{
    private OpenGLRenderer _renderer;
    private Canvas _canvas;

    public Canvas Canvas => _canvas;
    
    private Context(GL gl)
    {
        _renderer = new OpenGLRenderer(gl);
        _canvas = new Canvas(_renderer);
    }
    
    public static Context Create(Func<string, IntPtr> getProcAddr)
    {
        return new Context(GL.GetApi(getProcAddr));
    }

    public void Flush()
    {
        _renderer.Flush();
    }
}
