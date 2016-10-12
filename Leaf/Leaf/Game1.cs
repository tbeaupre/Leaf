using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Leaf
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Leaf leaf = new Leaf();
		Texture2D leafTexture;
		Texture2D vectorTexture;
		Texture2D anchorTexture;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);

			graphics.PreferredBackBufferWidth = ScreenData.Get().GetFullScreenWidth();
			graphics.PreferredBackBufferHeight = ScreenData.Get().GetFullScreenHeight();
			//graphics.IsFullScreen = true;

			graphics.ApplyChanges();
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			leafTexture = Content.Load<Texture2D>("leaf");
			vectorTexture = Content.Load<Texture2D>("Vector");
			anchorTexture = Content.Load<Texture2D>("Anchor");
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			KeyHandler.Get().UpdateKeyboardHandler();
			leaf.Update(); // Updates the player's leaf.
			
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
			GraphicsDevice.Clear(Color.CornflowerBlue);

			//spriteBatch.Draw(leafTexture, new Rectangle((int)leaf.x, (int)leaf.y, leafTexture.Width, leafTexture.Height), Color.White);
			spriteBatch.Draw(leafTexture, new Rectangle((int)leaf.x, (int)leaf.y, leafTexture.Width, leafTexture.Height), null, Color.White, (float)leaf.angle.val, new Vector2(leafTexture.Width / 2, 0), SpriteEffects.None, 0);
			spriteBatch.Draw(anchorTexture, new Rectangle((int)leaf.anchor.X, (int)leaf.anchor.Y, anchorTexture.Width, anchorTexture.Height), Color.White);
			//DrawVector(leaf.acc, Color.Red);
			//DrawVector(leaf.vel, Color.Blue);
			//DrawVector(leaf.gravity, Color.Black);

			base.Draw(gameTime);
			spriteBatch.End();
		}

		public void DrawVector(PhysicsVector vec, Color color)
		{
			spriteBatch.Draw(vectorTexture, new Rectangle((int)leaf.x, (int)leaf.y, (int)(vec.magnitude * 20), vectorTexture.Height/2), null, color, (float)vec.direction.val, new Vector2(0, 0), SpriteEffects.None, 0);
		}
	}
}
