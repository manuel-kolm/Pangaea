namespace Pangaea.Common;

public struct Size
{
    public int Width;
    public int Height;
    
    public Size() : this(0, 0) { }

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }
}
