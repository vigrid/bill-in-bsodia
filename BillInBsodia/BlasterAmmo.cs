using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class BlasterAmmo : PickableEntity
	{
		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
		                                                   	{
		                                                   		Rectangle = new Rectangle(32, 72, 8, 8),
		                                                   		Origin = new Vector2(3, 3)
		                                                   	};

		public BlasterAmmo(Vector3 originalPosition) : base(originalPosition)
		{
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override void Pick(Player player)
		{
			player.BlasterAmmo += 30;
		}
	}
}