namespace Pangaea.Rendering.OpenGL.Shaders;

public static class ShaderSource
{
    public static string VERTEX_SHADER_SOURCE = @"
        #version 330 core
        uniform vec2 viewSize;

        layout (location = 0) in vec2 vertexPos;
        layout (location = 1) in vec2 texCoordinates;

        out vec2 fragVertexPos;

        void main()
        {
            gl_Position = vec4(2.0 * vertexPos.x / viewSize.x - 1.0, 1.0 - 2.0 * vertexPos.y / viewSize.y, 0, 1);
            fragVertexPos = vertexPos;
        }
    ";

    public static string FRAGMENT_SHADER_SOURCE = @"
        #version 330 core
        const int RECT      = 1;
        const int CIRCLE    = 2;

        uniform int type;
        uniform vec2 center;
        uniform float innerRadius;

        in vec2 fragVertexPos;

        out vec4 FragColor;

        vec4 getColorForRect()
        {
            return vec4(1.0f, 0.5f, 0.2f, 1.0f);
        }

        vec4 getColorForCircle()
        {
            if (innerRadius != -1 && distance(fragVertexPos, center) < innerRadius)
                discard;

            return vec4(1.0f, 0.5f, 0.2f, 1.0f);
        }

		void main(void) {
            if (type == RECT)
            {
                FragColor = getColorForRect();
            }
            else if (type == CIRCLE)
            {
                FragColor = getColorForCircle();
            }
            else
            {
                FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
            }
		}";
}
