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

		const int numPlayers = 2;
		List<Leaf> leaves = new List<Leaf>(numPlayers);
		Texture2D leafTexture;
		Texture2D vectorTexture;
		Texture2D anchorTexture;

		const int borderY = 200;
		const int borderX = 100;
		int screenX = 0;
		int screenY = 0;
		int screenWidth;
		int maxScreenWidth;
		int screenHeight;
		int maxScreenHeight;
		double screenRatio;

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
			screenWidth = ScreenData.Get().GetFullScreenWidth();
			maxScreenWidth = screenWidth * 2;
			screenHeight = ScreenData.Get().GetFullScreenHeight();
			maxScreenHeight = screenHeight * 2;
			screenRatio = screenWidth / ScreenData.Get().GetFullScreenWidth();

			leaves.Add(new Leaf(new KeySet(Keys.Up, Keys.Down, Keys.Left, Keys.Right), Color.GreenYellow));
			leaves.Add(new Leaf(new KeySet(Keys.W, Keys.S, Keys.A, Keys.D), Color.DarkRed));
			leaves.RemoveRange(numPlayers, leaves.Count - numPlayers);
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
			foreach (Leaf leaf in leaves)
			{
				leaf.Update(); // Updates the player's leaf.
			}
			CheckMapBounds();

			base.Update(gameTime);
		}

		public void CheckMapBounds()
		{
			int minObjX = screenX + screenWidth/2;
			int maxObjX = screenX + screenWidth/2;
			int minObjY = screenY + screenHeight/2;
			int maxObjY = screenY + screenHeight/2;

			if (screenRatio > 1) // If the screen is larger than normal, then set it back to normal
			{
				int deltaScreen = screenWidth - ScreenData.Get().GetFullScreenWidth();
				if (deltaScreen > 0)
				{
					screenX += deltaScreen / 2;
					screenWidth = ScreenData.Get().GetFullScreenWidth();
				}
				deltaScreen = screenHeight - ScreenData.Get().GetFullScreenHeight();
				if (deltaScreen > 0)
				{
					screenY += deltaScreen / 2;
					screenHeight = ScreenData.Get().GetFullScreenHeight();
				}
			}

			foreach (Leaf leaf in leaves)
			{
				if (leaf.pos.x > maxObjX)
					maxObjX = (int)leaf.pos.x;
				else if (leaf.pos.x < minObjX)
					minObjX = (int)leaf.pos.x;
				if (leaf.pos.y > maxObjY)
					maxObjY = (int)leaf.pos.y;
				else if (leaf.pos.y < minObjY)
					minObjY = (int)leaf.pos.y;
			}

			// Hard bounds on max and min obj X;
			if (minObjX < -ScreenData.Get().GetFullScreenWidth())
				minObjX = -ScreenData.Get().GetFullScreenWidth();
			if (maxObjX > ScreenData.Get().GetFullScreenWidth() + screenWidth)
				maxObjX = ScreenData.Get().GetFullScreenWidth() + screenWidth;


			if (minObjX < screenX + borderX) // Check to see if there's an object OOB Left
				screenX = minObjX - borderX; // Correct OOB Left
			if (maxObjX > screenX + screenWidth - borderX) // Check to see if there's an object OOB Right
			{	// We need to move the screen to the right
				if (maxObjX - minObjX > screenWidth - (2 * borderX)) // Check to see if both objects can fit on the screen
				{   // No, so we need to scale up the screen
					screenWidth = maxObjX - screenX + borderX; // Correct OOB Right
				}
				else // Yes, we can move the screen to the right
				{
					screenX = maxObjX + borderX - screenWidth; // Correct OOB Right
				}
			}

			if (minObjY < screenY + borderY) // Check to see if there's an object OOB Top
				screenY = minObjY - borderY; // Correct OOB Top
			if (maxObjY > screenY + screenHeight - borderY) // Check to see if there's an object OOB Bottom
			{   // We need to move the screen down
				if (maxObjY - minObjY > screenHeight - (2 * borderY)) // Check to see if both objects can fit on the screen
				{   // No, so we need to scale up the screen
					screenHeight = maxObjY - screenY + borderY; // Correct OOB Bottom

					int delta = screenHeight - maxScreenHeight;
					if (delta > 0) // If we're already stretched too thin, drop the top guy.
					{
						screenHeight = maxScreenHeight;
						screenY += delta;
					}
				}
				else // Yes, we can move the screen down
				{
					screenY = maxObjY + borderY - screenHeight; // Correct OOB Bottom
				}
			}

			double widthRatio = screenWidth / (double)ScreenData.Get().GetFullScreenWidth();
			double heightRatio = screenHeight / (double)ScreenData.Get().GetFullScreenHeight();
			screenRatio = (widthRatio > heightRatio ? widthRatio : heightRatio); // Set to the greater ratio
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
			GraphicsDevice.Clear(Color.CornflowerBlue);

			foreach (Leaf leaf in leaves)
			{
				DrawLeaf(leaf);
			}

			base.Draw(gameTime);
			spriteBatch.End();
		}

		public void DrawLeaf(Leaf leaf)
		{
			spriteBatch.Draw(leafTexture, 
				new Rectangle(ScreenX(leaf.pos.x), ScreenY(leaf.pos.y), (int)(leafTexture.Width / screenRatio), (int)(leafTexture.Height / screenRatio)),
				null, leaf.color, (float)leaf.angle.val, new Vector2(leafTexture.Width / 2, 0), SpriteEffects.None, 0);

			spriteBatch.Draw(anchorTexture,
				new Rectangle(ScreenX(leaf.anchor.x), ScreenY(leaf.anchor.y), (int)(anchorTexture.Width / screenRatio), (int)(anchorTexture.Height / screenRatio)),
				leaf.color);

			//DrawVector(leaf.acc, Color.Red);
			//DrawVector(leaf.vel, Color.Blue);
			//DrawVector(leaf.gravity, Color.Black);
		}

		public void DrawVector(PhysicsVector vec, Color color, Leaf leaf)
		{
			//spriteBatch.Draw(vectorTexture, new Rectangle(leaf.pos.x, leaf.pos.y, (int)(vec.magnitude * 20), vectorTexture.Height/2), null, color, (float)vec.direction.val, new Vector2(0, 0), SpriteEffects.None, 0);
		}

		public int ScreenX(double val)
		{
			return (int)((val - screenX) / screenRatio);
		}

		public int ScreenY(double val)
		{
			return (int)((val - screenY) / screenRatio);
		}
	}
}
