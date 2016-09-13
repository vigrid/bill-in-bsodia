using Microsoft.Xna.Framework;

namespace LD48_23
{
	public static class TileCoord
	{
		public static Vector2 For(int x, int y)
		{
			return new Vector2(x * 32, y * 32);
		}
	}
}