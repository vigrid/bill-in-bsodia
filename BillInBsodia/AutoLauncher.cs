using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class AutoLauncher : Weapon
	{
		public static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																				{
																					Rectangle = new Rectangle(40, 64, 16, 8),
																					Origin = new Vector2(8, 4)
																				};

		public AutoLauncher(Vector3 originalPosition)
			: base(originalPosition)
		{
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override void Pick(Player player)
		{
			player.HasDoubleLauncher = true;
			player.LauncherAmmo += 10;
		}
	}
}