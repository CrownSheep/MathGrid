using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using MonoGame.Extended;

namespace MathGrid;

public class Grid
{
    public const int CELL_SIZE = 175;
    private const int LINE_THICKNESS = 8;

    private readonly Cell[,] cells;
    private readonly SpriteFontBase font;

    public Vector2 Position { get; set; }

    public int Width { get; }
    public int Height { get; }

    public Grid(Vector2 position, int width, int height, SpriteFontBase font)
    {
        Position = position;
        Width = width;
        Height = height;

        this.font = font;

        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new Cell(y * width + x + 1);
            }
        }
    }

    public int Get(int x, int y)
    {
        return cells[x, y].Number;
    }

    public void Update(GameTime gameTime)
    {
        TouchCollection touches = TouchPanel.GetState();

        if (touches.Count > 0 && touches[0].State == TouchLocationState.Pressed)
        {
            TouchLocation touch = touches[0];

            int x = (int)((touch.Position.X - Position.X) / CELL_SIZE);
            int y = (int)((touch.Position.Y - Position.Y) / CELL_SIZE);

            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                cells[x, y].ToggleMarked();
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cell cell = cells[x, y];

                Rectangle bounds = new Rectangle((int)Position.X + x * CELL_SIZE, (int)Position.Y + y * CELL_SIZE, 
                    CELL_SIZE + LINE_THICKNESS, CELL_SIZE + LINE_THICKNESS);

                Color backgroundColor = cell.Marked ? Color.Yellow : new Color(235, 235, 235);

                spriteBatch.FillRectangle(bounds, backgroundColor);

                spriteBatch.DrawRectangle(bounds, Color.Black, LINE_THICKNESS);

                string text = cell.Number.ToString();

                Vector2 textSize = font.MeasureString(text);

                Vector2 position = new Vector2(bounds.Center.X - textSize.X / 2, bounds.Center.Y - textSize.Y / 2 - 7);

                spriteBatch.DrawString(font, text, position, Color.Black);
            }
        }
    }
}