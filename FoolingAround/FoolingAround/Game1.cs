using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoolingAround
{
    internal class Game1 : Game
    {
        private Texture2D SpriteTexture;
        private Rectangle TitleSafe;
        private SpriteBatch spriteBatch;

        protected override void LoadContent()
        {
            
           // IGraphicsDeviceManager t = new GraphicsDeviceManager(this);
           //t.CreateDevice();
            //GraphicsDeviceManager manag  = new GraphicsDeviceManager(this);
            IGraphicsDeviceService t = new GraphicsDeviceManager(this);
            base.Initialize();
            base.LoadContent();
            
            
            
            
            
            // Create a new SpriteBatch, which can be used to draw textures.
            
            
            
        }
        

        protected override void Draw(GameTime gameTime)
        {
            Initialize();
            LoadContent();
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();
            var pos = new Vector2(TitleSafe.Left, TitleSafe.Top);
            spriteBatch.Draw(SpriteTexture, pos, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected Rectangle GetTitleSafeArea(float percent)
        {
            var retval = new Rectangle(
                GraphicsDevice.Viewport.X,
                GraphicsDevice.Viewport.Y,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height);
            return retval;
        }
    }
}