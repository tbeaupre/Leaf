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
		Vector2 anchor;
		
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
			UpdateAcceleration();

			vel += acc;
			x += vel.ConvertToCartesian().X;
			y += vel.ConvertToCartesian().Y;

			UpdateAngle();

			// Old Shit
			//speedFactor = Math.Pow(Math.Cos(angle), 2);
			////speedFactor = Math.Cos(angle);

			//if (angle <= -Math.PI / 2 + Math.PI / 6 || angle >= Math.PI / 2 - Math.PI / 6)
			//{
			//	speed *= -1;
			//}
			//x += speedFactor * speed * Math.Cos(angle);
			//double deltaY = speedFactor * speed * Math.Sin(angle);
			//y += deltaY;	
			
			//angle -= angleDelta * Math.Sign(speed); // Update the angle
		}

		void KeyHandling()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Up))
			{
				float distance = 10;
				anchor.X -= (float)(distance * Math.Cos(angle.val + (Math.PI / 2)));
				anchor.Y -= (float)(distance * Math.Sin(angle.val + (Math.PI / 2)));
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Down))
			{
				float distance = 10;
				anchor.X += (float)(distance * Math.Cos(angle.val + (Math.PI / 2)));
				anchor.Y += (float)(distance * Math.Sin(angle.val + (Math.PI / 2)));
			}
			if (Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				vel.magnitude += .1;
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
	}
}
