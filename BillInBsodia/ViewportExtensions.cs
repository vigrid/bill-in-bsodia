using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public static class ViewportExtensions
	{
		public static Vector2 Center2(this Viewport viewport)
		{
			return new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
		}

		public static Vector3 Center3(this Viewport viewport)
		{
			return new Vector3(viewport.Width / 2.0f, viewport.Height / 2.0f, 0.0f);
		}
	}
}