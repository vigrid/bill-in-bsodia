using System;

namespace LD48_23
{
	public static class MathUtil
	{
		public static float Wrap(float value, float min, float max)
		{
			if (value >= min && value <= max)
			{
				return value;
			}

			if (value < min)
			{
				return min + (min - value);
			}

			if (value > max)
			{
				return max - (value - max);
			}

			throw new InvalidOperationException("Should never happen");
		}
	}
}