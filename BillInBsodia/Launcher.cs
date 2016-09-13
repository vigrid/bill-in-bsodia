using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class Launcher : Weapon
	{
		public static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
		                                                   	{
		                                                   		Rectangle = new Rectangle(40, 56, 16, 8),
		                                                   		Origin = new Vector2(8, 4)
		                                                   	};

		public Launcher(Vector3 originalPosition) : base(originalPosition)
		{
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override void Pick(Player player)
		{
			player.HasLauncher = true;
			player.LauncherAmmo += 10;
		}
	}
}