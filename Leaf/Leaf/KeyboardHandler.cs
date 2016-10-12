using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Leaf
{
    public class KeyHandler
    {
        #region Singleton
        private static readonly KeyHandler INSTANCE = new KeyHandler();
        public static KeyHandler Get()
        {
            return INSTANCE;
        }
        #endregion

        bool gamePad;
        Microsoft.Xna.Framework.PlayerIndex index = Microsoft.Xna.Framework.PlayerIndex.One;

        KeyboardState oldKeys;
        KeyboardState newKeys;

        GamePadState oldPad;
        GamePadState newPad;

        public KeyHandler()
        {
            gamePad = true;
            if (GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One).IsConnected)
                this.index = Microsoft.Xna.Framework.PlayerIndex.One;
            else if (GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Two).IsConnected)
                this.index = Microsoft.Xna.Framework.PlayerIndex.Two;
            else if (GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Three).IsConnected)
                this.index = Microsoft.Xna.Framework.PlayerIndex.Three;
            else if (GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.Four).IsConnected)
                this.index = Microsoft.Xna.Framework.PlayerIndex.Four;
            else
                gamePad = false;
        }

        public void UpdateKeyboardHandler()
        {
            this.oldKeys = newKeys;
            this.oldPad = newPad;
            this.newKeys = Keyboard.GetState();
            if (GamePad.GetState(index).IsConnected)
            {
                this.newPad = GamePad.GetState(index);
                gamePad = true;
            }
            else
                gamePad = false;
        }

        public KeyboardState GetOldKeys()
        {
            return this.oldKeys;
        }

        public KeyboardState GetNewKeys()
        {
            return this.newKeys;
        }

        public GamePadState GetNewPad()
        {
            return this.newPad;
        }

        public bool IsKeyJustPressed(Keys key)
        {
            return (IsKeyDown(key, false) && !IsKeyDown(key, true));
        }

        public bool IsKeyHeld(Keys key)
        {
            return IsKeyDown(key, false);
        }

		public bool IsKeyDown(Keys key, bool old)
		{
			KeyboardState keys;
			if (old)
			{
				keys = oldKeys;
			}
			else
			{
				keys = newKeys;
			}
			return (keys.IsKeyDown(key));
		}
    }
}
