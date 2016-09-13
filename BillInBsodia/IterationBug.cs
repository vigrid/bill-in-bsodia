using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class IterationBug : Mob
	{
		public static Vector3 AveragePositionOfAll;

		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
		                                                   	{
		                                                   		Rectangle = new Rectangle(56, 32, 16, 16),
		                                                   		Origin = new Vector2(8, 14)
		                                                   	};

		public IterationBug(Vector3 position) : base(position)
		{
			Health = 2;
		}

		public override float Size
		{
			get { return 0.5f; }
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override float Acceleration
		{
			get { return 2.0f; }
		}

		public override float MaxSpeed
		{
			get { return 7.0f; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			var target = (world.Bill.Position + AveragePositionOfAll) / 2.0f;
			var direction = Vector3.Normalize(target - Position);
			direction.Z = 0.0f;

			Velocity += Acceleration * direction * time;

			if (Velocity.Length() > MaxSpeed)
			{
				Velocity.Normalize();
				Velocity *= MaxSpeed;
			}

			Position += Velocity * time;

			var collided = Collide(world, time);
			if (collided == CollideResult.Collided)
			{
				PerformCollission(world, time);
			}

			CollideWithBill();
		}

		public override void PerformCollission(VoxelWorld world, float time)
		{
			// wallbanged
			Position -= 2.0f * Velocity * time;
			Position.Z = 0.0f; // iteration bugs can't fly
			Velocity.X *= -0.5f;
			Velocity.Y *= -0.5f;
		}
	}
}