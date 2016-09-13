using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class LauncherAmmo : PickableEntity
	{
		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
		                                                   	{
		                                                   		Rectangle = new Rectangle(40, 72, 8, 8),
		                                                   		Origin = new Vector2(4, 7)
		                                                   	};

		public LauncherAmmo(Vector3 originalPosition) : base(originalPosition)
		{
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override void Pick(Player player)
		{
			player.LauncherAmmo += 30;
		}
	}
}