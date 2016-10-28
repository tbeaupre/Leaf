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
		KeySet keySet;

		double maxSpeed;
		double radius = 300; // The radius of the pendulum
		double prevRadius = 0; // the previous radius
		
		public Leaf(KeySet keySet)
		{
			this.keySet = keySet;
			this.pos = new CartesianVector((ScreenData.Get().GetFullScreenWidth() / 2) - 300, ScreenData.Get().GetFullScreenHeight() / 4);
			this.anchor = new CartesianVector(pos.x + 300, pos.y);
			this.vel = new PhysicsVector(0, 0);
			this.gravity = new PhysicsVector(Math.PI / 2, .1);
			UpdateAngle();

			this.maxSpeed = 8;
		}

		public void Update()
		{
			KeyHandling();

			if (tangentMode == false)
			{
				UpdateAcceleration();
				vel += acc;
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

		void KeyHandling()	// Contains key handling code.
		{
			keyHandler = KeyHandler.Get();

			// Logic for determining the distance of the anchor point
			radius = 300; // If no keys are pressed or both keys are pressed
			if (keyHandler.IsKeyHeld(keySet.up) && !keyHandler.IsKeyHeld(keySet.down)) // Moves the pendulum anchor point away from the leaf.
			{
				radius = 500;
			}
			if (keyHandler.IsKeyHeld(keySet.down) && !keyHandler.IsKeyHeld(keySet.up)) // Moves the pendulum anchor point towards the leaf.
			{
				radius = 100;
			}
			if (radius != prevRadius) // Only move the anchor point if you must
			{
				anchor.x = pos.x - (radius * Math.Cos(angle.val + (Math.PI / 2)));
				anchor.y = pos.y - (radius * Math.Sin(angle.val + (Math.PI / 2)));
			}

			prevRadius = radius;

			if (keyHandler.IsKeyJustPressed (keySet.boost)) // Provides a bit of a speed boost.
			{
				vel.magnitude += 3;
			}
			if (keyHandler.IsKeyJustPressed(keySet.flip)) // Flips the leaf.
			{
				double deltaX = anchor.x - pos.x;
				double deltaY = anchor.y - pos.y;
				anchor.x -= (deltaX * 2);
				anchor.y -= (deltaY * 2);
			}
		}

		void UpdateAcceleration()	// Calculates the current acceleration of the leaf
		{
			PhysicsVector gPerp = new PhysicsVector((Math.PI / 2) + angle.val, gravity.magnitude * Math.Cos(angle.val));
			PhysicsVector tension = new PhysicsVector(gPerp.direction + Math.PI, gPerp.magnitude + (Math.Pow(vel.magnitude, 2) / radius)); // Tension counters the perpendicular vector of gravity
			acc = tension + gravity;
		}

		void UpdateAngle()	// Updates the rotation of the leaf based upon its position on the arc
		{
			angle = Math.Atan2(anchor.y - pos.y, anchor.x - pos.x) + (Math.PI / 2);
		}

		void RadiusCheck()	// Checks to see if the leaf is at the correct radius for the arc.
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

		void ReduceVelocity()	// Applies a bit of resistance so that the leaf slows down over time.
		{
			if (vel.magnitude > maxSpeed)
				vel.magnitude -= .005;
		}
	}
}
