namespace Pangaea.Rendering.OpenGL.Shaders;

public static class ShaderSource
{
    public static string VERTEX_SHADER_SOURCE = @"
        #version 330 core
        layout (location = 0) in vec2 vPos;
        layout (location = 1) in vec2 vUv;

        uniform vec2 uViewSize;

        out vec2 fPos;
        out vec2 fUv;

        void main()
        {
            gl_Position = vec4(2.0 * vPos.x / uViewSize.x - 1.0, 1.0 - 2.0 * vPos.y / uViewSize.y, 0, 1);
            fPos = vPos;
            fUv = vUv;
        }
    ";

    public static string FRAGMENT_SHADER_SOURCE = @"
        #version 330 core
        in vec2 fPos;
        in vec2 fUv;

        out vec4 FragColor;

        uniform int type;
        uniform vec2 center;
        uniform float radius;

        float magnitude(vec2 a, vec2 b)
        {
            float x = a.x - b.x;
            float y = a.x - b.y;
            return sqrt(x * x + y * y);
        }

        void main()
        {
            FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
        }
    ";
}
