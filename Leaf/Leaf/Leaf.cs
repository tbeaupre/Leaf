using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Leaf
{
	public class Leaf
	{
		public float x { get; set; }
		public float y { get; set; }

		public Radian angle { get; set; }
		public PhysicsVector acc;
		public PhysicsVector vel;
		public PhysicsVector gravity;
		public Vector2 anchor;
		bool tangentMode = false;
		public KeyHandler keyHandler;
		
		public Leaf()
		{
			this.x = (ScreenData.Get().GetFullScreenWidth() / 2) - 300;
			this.y = ScreenData.Get().GetFullScreenHeight() / 4;
			this.anchor = new Vector2(x + 300, y);
			this.vel = new PhysicsVector(0, 0);
			this.gravity = new PhysicsVector(Math.PI / 2, .1);
			UpdateAngle();
		}

		public void Update()
		{
			KeyHandling();

			if (tangentMode == false)
			{
				UpdateAcceleration();
				vel += acc;
			}

			Vector2 velCart = vel.ConvertToCartesian();
			x += velCart.X;
			y += velCart.Y;

			if (tangentMode)
			{
				anchor += vel.ConvertToCartesian();
			}

			UpdateAngle();
			ReduceVelocity();
		}

		void KeyHandling()
		{
			keyHandler = KeyHandler.Get();
			if (keyHandler.IsKeyHeld(Keys.Up))
			{
				// Moves the pendulum anchor point away from the leaf.
				float distance = 10;
				anchor.X -= (float)(distance * Math.Cos(angle.val + (Math.PI / 2)));
				anchor.Y -= (float)(distance * Math.Sin(angle.val + (Math.PI / 2)));
			}
			if (keyHandler.IsKeyHeld(Keys.Down))
			{
				// Moves the pendulum anchor point towards the leaf.
				Vector2 currentPos = new Vector2(x, y);
				double radius = Vector2.Distance(currentPos, anchor);

				if (radius > 50)
				{
					float distance = 10;
					anchor.X += (float)(distance * Math.Cos(angle.val + (Math.PI / 2)));
					anchor.Y += (float)(distance * Math.Sin(angle.val + (Math.PI / 2)));
				}
			}
			if (keyHandler.IsKeyHeld(Keys.Space))
			{
				// Provides a bit of a speed boost.
				vel.magnitude += .1;
			}
			if (keyHandler.IsKeyJustPressed(Keys.Z))
			{
				// Flips the leaf.
				float deltaX = anchor.X - x;
				float deltaY = anchor.Y - y;
				anchor.X -= (deltaX * 2);
				anchor.Y -= (deltaY * 2);
			}
		}

		void UpdateAcceleration()
		{
			Vector2 currentPos = new Vector2(x, y);
			double radius = Vector2.Distance(currentPos, anchor);

			PhysicsVector gPerp = new PhysicsVector((Math.PI / 2) + angle.val, gravity.magnitude * Math.Cos(angle.val));

			PhysicsVector tension = new PhysicsVector(gPerp.direction + Math.PI, gPerp.magnitude + (Math.Pow(vel.magnitude, 2) / radius)); // Tension counters the perpendicular vector of gravity
			acc = tension + gravity;
		}

		void UpdateAngle()
		{
			Vector2 currentPos = new Vector2(x, y);
			Vector2 delta = Vector2.Subtract(anchor, currentPos);
			angle = Math.Atan2(delta.Y, delta.X) + (Math.PI / 2);
		}

		void ReduceVelocity()
		{
			if (vel.magnitude > 0)
				vel.magnitude -= .001;
		}
	}
}
