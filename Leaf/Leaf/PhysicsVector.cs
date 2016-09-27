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
			Vector2 cartRes = Vector2.Add(vec1.ConvertToCartesian(), vec2.ConvertToCartesian());
			double hypotenuse = Math.Sqrt(Math.Pow(cartRes.X, 2) + Math.Pow(cartRes.Y, 2));
			Radian angle = Math.Atan2(cartRes.Y, cartRes.X);
			return new PhysicsVector(angle, hypotenuse);
		} 

		public Vector2 ConvertToCartesian()
		{
			return new Vector2((float)(magnitude * Math.Cos(direction.val)), (float)(magnitude * Math.Sin(direction.val))); 
		}
	}
}
