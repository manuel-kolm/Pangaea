namespace Pangaea.Common;

public struct Rect
{
    public int X;
    public int Y;
    public int W;
    public int H;

    public Rect() : this(0, 0, 0, 0) { }

    public Rect(int x, int y, int w, int h)
    {
        X = x;
        Y = y;
        W = w;
        H = h;
    }
}
