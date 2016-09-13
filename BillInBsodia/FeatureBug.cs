using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class FeatureBug : Mob
	{
		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																				{
																					Rectangle = new Rectangle(40, 32, 16, 16),
																					Origin = new Vector2(8, 14),
																					CustomScale = 0.5f
																				};

		public FeatureBug(Vector3 position)
			: base(position)
		{
			Health = 1;
		}

		protected override bool DestructibleOnCollission
		{
			get { return true; }
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override float Size
		{
			get { return 0.25f; }
		}

		public override float Acceleration
		{
			get { return 5.0f; }
		}

		public override float MaxSpeed
		{
			get { return 5.0f; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			if (BillGame.Random.NextDouble() < time * 2.0f)
			{
				Velocity += BillGame.Random.GetVector3() * 3.0f;
				Velocity.Z = 0.0f;
			}

			base.Update(world, time);
		}

		public override void PerformCollission(VoxelWorld world, float time)
		{
			// wallbanged
			Position -= 2.0f * Velocity * time;
			Position.Z = 0.0f; // feature bugs can't fly
			Velocity.X *= -0.5f;
			Velocity.Y *= -0.5f;
		}
	}
}