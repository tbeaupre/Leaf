using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Leaf
{
	public class KeySet
	{
		public Keys up;
		public Keys down;
		public Keys flip;
		public Keys boost;

		public KeySet(Keys up, Keys down, Keys flip, Keys boost)
		{
			this.up = up;
			this.down = down;
			this.flip = flip;
			this.boost = boost;
		}
	}
}
