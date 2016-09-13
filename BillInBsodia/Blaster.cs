using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class Blaster : Weapon
	{
		public static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
		                                                   	{
		                                                   		Rectangle = new Rectangle(32, 56, 8, 8),
		                                                   		Origin = new Vector2(4, 2)
		                                                   	};

		public Blaster(Vector3 originalPosition) : base(originalPosition)
		{
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override void Pick(Player player)
		{
			player.HasBlaster = true;
			player.BlasterAmmo += 30;
		}
	}
}