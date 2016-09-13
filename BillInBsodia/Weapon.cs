using Microsoft.Xna.Framework;

namespace LD48_23
{
	public abstract class Weapon : PickableEntity
	{
		protected Weapon(Vector3 originalPosition) : base(originalPosition)
		{
		}
	}

	public class Teleporter : PickableEntity
	{
		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																				{
																					Rectangle = new Rectangle(96, 0, 8, 8),
																					Origin = new Vector2(4, 16)
																				};

		public Teleporter(Vector3 position) : base(position)
		{
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override void Pick(Player player)
		{
			player.HasTeleporter = true;
		}

		public override float Size
		{
			get { return 20.0f; }
		}
	}
}