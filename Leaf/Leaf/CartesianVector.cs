using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaf
{
	public class CartesianVector
	{
		public double x;
		public double y;

		public CartesianVector(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		static public CartesianVector operator +(CartesianVector vec1, CartesianVector vec2)
		{
			return new CartesianVector(vec1.x + vec2.x, vec1.y + vec2.y);
		}

		static public CartesianVector operator -(CartesianVector vec1, CartesianVector vec2)
		{
			return new CartesianVector(vec2.x - vec1.x, vec2.y - vec1.y);
		}

		static public double Distance (CartesianVector vec1, CartesianVector vec2)
		{
			return Math.Sqrt(Math.Pow(vec2.x - vec1.x, 2) + Math.Pow(vec2.y - vec1.y, 2));
		}
	}
}
