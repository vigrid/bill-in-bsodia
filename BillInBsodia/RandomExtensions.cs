using System;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	public static class RandomExtensions
	{
		public static Vector3 GetVector3(this Random random)
		{
			Vector3 result;
			double z = random.NextDouble() * 2.0 - 1.0;
			double r = Math.Sqrt(1.0 - z * z);
			double angle = random.NextDouble() * Math.PI * 2.0;
			result.X = (float) (r * Math.Cos(angle));
			result.Y = (float) (r * Math.Sin(angle));
			result.Z = (float) z;
			return result;
		}
	}
}