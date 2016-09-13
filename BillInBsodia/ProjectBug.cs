using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class ProjectBug : Mob
	{
		public const float ShotInterval = 0.5f;

		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																				{
																					Rectangle = new Rectangle(88, 32, 16, 16),
																					Origin = new Vector2(8, 16),
																					CustomScale = 1.5f,
																				};

		public ProjectBug(Vector3 position)
			: base(position)
		{
			Health = 5;
			Position.Z = 1.0f;
		}

		public override float Size
		{
			get { return 1.0f; }
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override float Acceleration
		{
			get { return 9.0f; }
		}

		public override float MaxSpeed
		{
			get { return 3.0f; }
		}


		private float _timeUntilShot = ShotInterval;

		public override void Update(VoxelWorld world, float time)
		{
			base.Update(world, time);

			_timeUntilShot -= time;

			if (_timeUntilShot < 0.0f)
			{
				_timeUntilShot += ShotInterval;
				world.RegisterEntity(new EnemyShot(Position + Vector3.UnitZ * 0.5f, world.Bill.Position + Vector3.UnitZ * 0.25f));
				BillGame.Instance.PlaySound("Sounds/EnemyShot");
			}
		}

		public override void PerformCollission(VoxelWorld world, float time)
		{
			// will not collide. ever. just shoots
		}
	}
}