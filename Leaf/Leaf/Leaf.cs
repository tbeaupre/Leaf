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
		public CartesianVector pos;
		public Radian angle { get; set; }
		public PhysicsVector acc;
		public PhysicsVector vel;
		public PhysicsVector gravity;
		public CartesianVector anchor;
		bool tangentMode = false;
		public KeyHandler keyHandler;

		double radius = 300; // The radius of the pendulum
		double prevRadius = 0; // the previous radius
		
		public Leaf()
		{
			this.pos = new CartesianVector((ScreenData.Get().GetFullScreenWidth() / 2) - 300, ScreenData.Get().GetFullScreenHeight() / 4);
			this.anchor = new CartesianVector(pos.x + 300, pos.y);
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
				//vel.magnitude = Math.Sqrt(2 * gravity.magnitude * radius * (1 - Math.Cos((angle.val))));
				//vel.direction = angle;
			}

			CartesianVector velCart = vel.ConvertToCartesian();
			pos += velCart;

			if (tangentMode)
			{
				anchor += velCart;
			}

			UpdateAngle();
			RadiusCheck();
			ReduceVelocity();
		}

		void KeyHandling()
		{
			keyHandler = KeyHandler.Get();

			// Logic for determining the distance of the anchor point
			radius = 300; // If no keys are pressed or both keys are pressed
			if (keyHandler.IsKeyHeld(Keys.Up) && !keyHandler.IsKeyHeld(Keys.Down)) // Moves the pendulum anchor point away from the leaf.
			{
				radius = 500;
			}
			if (keyHandler.IsKeyHeld(Keys.Down) && !keyHandler.IsKeyHeld(Keys.Up)) // Moves the pendulum anchor point towards the leaf.
			{
				radius = 100;
			}
			if (radius != prevRadius) // Only move the anchor point if you must
			{
				anchor.x = pos.x - (radius * Math.Cos(angle.val + (Math.PI / 2)));
				anchor.y = pos.y - (radius * Math.Sin(angle.val + (Math.PI / 2)));
			}

			prevRadius = radius;

			if (keyHandler.IsKeyJustPressed (Keys.Space)) // Provides a bit of a speed boost.
			{
				vel.magnitude += 3;
			}
			if (keyHandler.IsKeyJustPressed(Keys.Z)) // Flips the leaf.
			{
				double deltaX = anchor.x - pos.x;
				double deltaY = anchor.y - pos.y;
				anchor.x -= (deltaX * 2);
				anchor.y -= (deltaY * 2);
			}
		}

		void UpdateAcceleration()
		{
			PhysicsVector gPerp = new PhysicsVector((Math.PI / 2) + angle.val, gravity.magnitude * Math.Cos(angle.val));

			PhysicsVector tension = new PhysicsVector(gPerp.direction + Math.PI, gPerp.magnitude + (Math.Pow(vel.magnitude, 2) / radius)); // Tension counters the perpendicular vector of gravity
			acc = tension + gravity;
		}

		void RadiusCheck()
		{
			double checkRadius = CartesianVector.Distance(pos, anchor);
			double tol = .1;
			while (checkRadius > radius + tol)
			{
				PhysicsVector move = new PhysicsVector(angle + Math.PI / 2, -0.1);
				pos += move.ConvertToCartesian();
				checkRadius = CartesianVector.Distance(pos, anchor);
			}
			while (checkRadius < radius - tol)
			{
				PhysicsVector move = new PhysicsVector(angle + Math.PI / 2, 0.1);
				pos += move.ConvertToCartesian();
				checkRadius = CartesianVector.Distance(pos, anchor);
			}
		}

		void UpdateAngle()
		{
			angle = Math.Atan2(anchor.y - pos.y, anchor.x - pos.x) + (Math.PI / 2);
		}

		void ReduceVelocity()
		{
			if (vel.magnitude > 0)
				vel.magnitude -= .001;
		}
	}
}
