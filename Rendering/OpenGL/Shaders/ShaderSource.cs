namespace Pangaea.Rendering.OpenGL.Shaders;

public static class ShaderSource
{
    public static string VERTEX_SHADER_SOURCE = @"
        #version 330 core
        uniform vec2 viewSize;

        layout (location = 0) in vec2 vertexPos;
        layout (location = 1) in vec2 texCoordinates;

        void main()
        {
            gl_Position = vec4(2.0 * vertexPos.x / viewSize.x - 1.0, 1.0 - 2.0 * vertexPos.y / viewSize.y, 0, 1);
        }
    ";

    public static string FRAGMENT_SHADER_SOURCE = @"
        #version 330 core

        out vec4 FragColor;

		void main(void) {
            FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
		}";
}
