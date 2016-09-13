using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class RocketShot : Projectile
	{
		public const float MaxLife = 3.0f;

		public const float RocketShotInitialSpeed = 10.0f;
		public const float RocketShotAcceleration = 15.0f;
		public const float Scatter = 0.3f;
		public const float SmokesPerSecond = 50.0f;

		public static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
		                                                  	{
		                                                  		Rectangle = new Rectangle(48, 48, 8, 8),
		                                                  		Origin = new Vector2(4, 4)
		                                                  	};

		private readonly Vector3 _acceleration;
		private float _lived;
		private Vector3 _velocity;

		public RocketShot(Vector3 position, Vector3 target)
		{
			Position = position;
			Vector3 direction = Vector3.Normalize(target - position);
			_velocity = direction * RocketShotInitialSpeed;
			_acceleration = (direction + BillGame.Random.GetVector3() * Scatter) * RocketShotAcceleration;
		}

		public override float Size
		{
			get { return 0.2f; }
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override bool HasShadow
		{
			get { return true; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			if (CollideProjectile(world, time))
			{
				world.Explode(Position);
				Destroyed = true;
				return;
			}

			_lived += time;
			if (_lived >= MaxLife)
			{
				Destroyed = true;
				return;
			}

			if (BillGame.Random.NextDouble() < time * SmokesPerSecond)
			{
				world.RegisterEntity(new Smoke(Position));
			}

			_velocity += _acceleration * time;
			Position += _velocity * time;

			foreach (var mob in BillGame.Instance.WorldComponent.World.Entities.OfType<Mob>())
			{
				var distanceToMob = Vector3.Distance(mob.Position, Position);
				if (distanceToMob < Size + mob.Size)
				{
					world.Explode(Position);
					Destroyed = true;
				}
			}
		}
	}
}