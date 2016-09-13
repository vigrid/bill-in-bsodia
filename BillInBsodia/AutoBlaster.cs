using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class AutoBlaster : Weapon
	{
		public static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
		                                                   	{
		                                                   		Rectangle = new Rectangle(32, 64, 8, 8),
		                                                   		Origin = new Vector2(4, 2)
		                                                   	};

		public AutoBlaster(Vector3 originalPosition) : base(originalPosition)
		{
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override bool HasShadow
		{
			get { return true; }
		}

		public override void Pick(Player player)
		{
			player.HasAutoBlaster = true;
			player.BlasterAmmo += 30;
		}
	}
}