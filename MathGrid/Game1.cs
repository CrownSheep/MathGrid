using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace MathGrid;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    
    private FontSystem fontSystem;
    private SpriteFontBase font;
    private Grid grid;

    // Virtual resolution and render target for aspect ratio handling
    private RenderTarget2D renderTarget;
    private int virtualWidth = 1920;
    private int virtualHeight = 1080;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        graphics.PreferredBackBufferWidth = virtualWidth;
        graphics.PreferredBackBufferHeight = virtualHeight;
        Window.AllowUserResizing = true;
        graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        
        renderTarget = new RenderTarget2D(GraphicsDevice, virtualWidth, virtualHeight);

        fontSystem = new FontSystem();
        using (Stream stream = TitleContainer.OpenStream("Fonts/Nunito.ttf"))
        {
            fontSystem.AddFont(stream);
        }

        font = fontSystem.GetFont(Grid.CELL_SIZE - 20);
        grid = new Grid(new Vector2(200, 250), 3, 3, font);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        grid.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(renderTarget);
        GraphicsDevice.Clear(Color.White);
        
        spriteBatch.Begin();
        grid.Draw(spriteBatch, gameTime);
        spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
        
        GraphicsDevice.Clear(Color.White);

        Rectangle destinationRect = GetDestinationRectangle();

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp);
        spriteBatch.Draw(renderTarget, destinationRect, Color.White);
        spriteBatch.End();

        base.Draw(gameTime);
    }

    private Rectangle GetDestinationRectangle()
    {
        var windowWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
        var windowHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;

        float targetAspectRatio = (float)virtualWidth / virtualHeight;
        float windowAspectRatio = (float)windowWidth / windowHeight;

        int width, height, x, y;

        if (windowAspectRatio > targetAspectRatio)
        {
            height = windowHeight;
            width = (int)(height * targetAspectRatio);
            x = (windowWidth - width) / 2;
            y = 0;
        }
        else
        {
            width = windowWidth;
            height = (int)(width / targetAspectRatio);
            x = 0;
            y = (windowHeight - height) / 2;
        }

        return new Rectangle(x, y, width, height);
    }
}