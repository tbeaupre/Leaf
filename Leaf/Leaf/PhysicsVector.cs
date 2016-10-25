using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Leaf
{
	public class PhysicsVector
	{
		public Radian direction;
		public double magnitude;

		public PhysicsVector(Radian direction, double magnitude)
		{
			this.direction = direction;
			this.magnitude = magnitude;
		}

		static public PhysicsVector operator + (PhysicsVector vec1, PhysicsVector vec2)
		{
			CartesianVector cartRes = vec1.ConvertToCartesian() + vec2.ConvertToCartesian();
			double hypotenuse = Math.Sqrt(Math.Pow(cartRes.x, 2) + Math.Pow(cartRes.y, 2));
			Radian angle = Math.Atan2(cartRes.y, cartRes.x);
			return new PhysicsVector(angle, hypotenuse);
		}

		public CartesianVector ConvertToCartesian()
		{
			return new CartesianVector((magnitude * Math.Cos(direction.val)), (magnitude * Math.Sin(direction.val))); 
		}

		public void Invert()
		{
			this.direction += Math.PI;
		}
	}
}
