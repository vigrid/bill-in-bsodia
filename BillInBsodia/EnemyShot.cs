using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class EnemyShot : BlasterShot
	{
		public override float BlasterShotSpeed
		{
			get { return 10.0f; }
		}

		public static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																			{
																				Rectangle = new Rectangle(32, 48, 8, 8),
																				Origin = new Vector2(4, 4)
																			};

		public EnemyShot(Vector3 position, Vector3 target)
			: base(position, target)
		{
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			if (CollideProjectile(world, time))
			{
				Destroyed = true;
				return;
			}

			base.Update(world, time);

			var bill = BillGame.Instance.WorldComponent.Bill;
			var distanceToBill = Vector3.Distance(bill.Position, Position);
			if (distanceToBill < Size + bill.Size)
			{
				if (bill.TryDamage())
				{
					Destroyed = true;
				}
			}
		}
	}
}