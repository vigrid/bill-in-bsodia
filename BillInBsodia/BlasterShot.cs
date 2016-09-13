using System.Linq;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class BlasterShot : Projectile
	{
		public virtual float BlasterShotSpeed { get { return 20.0f; } }
		public const float MaxLife = 2.0f;

		public static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																			{
																				Rectangle = new Rectangle(40, 48, 8, 8),
																				Origin = new Vector2(4, 4)
																			};

		private readonly Vector3 _velocity;
		private float _lived;

		public BlasterShot(Vector3 position, Vector3 target)
		{
			Position = position;
			_velocity = Vector3.Normalize(target - position) * BlasterShotSpeed;
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override bool HasShadow
		{
			get { return false; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			if (CollideProjectile(world, time))
			{
				Destroyed = true;
				return;
			}

			_lived += time;
			if (_lived >= MaxLife)
			{
				Destroyed = true;
				return;
			}

			Position += _velocity * time;

			foreach (var mob in BillGame.Instance.WorldComponent.World.Entities.OfType<Mob>())
			{
				var distanceToMob = Vector3.Distance(mob.Position, Position);
				if (distanceToMob < Size + mob.Size)
				{
					mob.Damage(1);
					Destroyed = true;
				}
			}
		}
	}
}