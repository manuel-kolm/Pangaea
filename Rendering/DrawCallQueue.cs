namespace Pangaea.Rendering;

public class DrawCallQueue
{
    private List<DrawCall> _calls;

    public DrawCallQueue()
    {
        _calls = new List<DrawCall>();
    }

    public void Add(DrawCall drawCall)
    {
        _calls.Add(drawCall);
    }

    public void Clear()
    {
        _calls.Clear();
    }
    
    public void Execute()
    {
        for (int i = 0; i < _calls.Count; ++i)
        {
            _calls[i].Run();
        }
        _calls.Clear();
    }

    public bool IsEmpty()
    {
        return _calls.Count == 0;
    }
}
