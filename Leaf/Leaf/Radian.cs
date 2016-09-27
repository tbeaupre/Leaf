using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Leaf
{
	public class Radian
	{
		public double val;

		public Radian(double val)
		{
			this.val = val % (2 * Math.PI);
		}
		public static implicit operator Radian(double val)
		{
			return new Radian(val);
		}

		static public Radian operator + (Radian rad1, Radian rad2)
		{
			return new Radian((rad1.val + rad2.val) % (2 * Math.PI));
		}

		static public Radian operator -(Radian rad1, Radian rad2)
		{
			return new Radian((rad1.val + rad2.val) % (2 * Math.PI));
		}
	}
}
