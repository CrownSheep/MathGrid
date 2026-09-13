namespace MathGrid;

public class Cell
{
    public int Number { get; }
    public bool Marked { get; set; }

    public Cell(int number)
    {
        Number = number;
        Marked = false;
    }

    public void ToggleMarked()
    {
        Marked = !Marked;
    }
}