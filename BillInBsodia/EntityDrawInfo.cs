using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public struct EntityDrawInfo
	{
		public Rectangle Rectangle;
		public Vector2 Origin;

		public float? CustomScale;
		public float? CustomAlpha;
	}
}