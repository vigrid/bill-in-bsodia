using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class MilestoneBug : Mob
	{
		public const float KamikazeTriggerDistance = 15.0f;
		public const float KamikazeAcceleration = 25.0f;
		public const float KamikazeLifeSpan = 5.0f;

		private Vector3 _acceleration;
		private float _kamikazeLifeLeft;

		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																				{
																					Rectangle = new Rectangle(72, 32, 16, 16),
																					Origin = new Vector2(8, 15),
																				};

		private bool _kamikaze;
		private bool _kamikazeLaunched;

		public MilestoneBug(Vector3 position)
			: base(position)
		{
			Health = 3;
		}

		public override float Size
		{
			get { return 0.75f; }
		}

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override float Acceleration
		{
			get { return 4.5f; }
		}

		public override float MaxSpeed
		{
			get { return 15.0f; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			if (!_kamikaze)
			{
				base.Update(world, time);

				if (Vector3.Distance(world.Bill.Position, Position) < KamikazeTriggerDistance)
				{
					_kamikaze = true;
					BillGame.Instance.PlaySound("Sounds/Kamikaze");
					Velocity.Z = 15.0f;
					_kamikazeLifeLeft = KamikazeLifeSpan;
					_acceleration = - Vector3.UnitZ * Player.Gravity;
				}
			}
			else
			{
				if (_kamikazeLaunched)
				{
					_kamikazeLifeLeft -= time;

					if (_kamikazeLifeLeft <= 0.0f)
					{
						Destroyed = true;
						return;
					}

					var collided = Collide(world, time);
					if (collided == CollideResult.Collided)
					{
						PerformCollission(world, time);
					}

					CollideWithBill();
				}
				else if (Velocity.Z <= 0.0f)
				{
					_kamikazeLaunched = true;
					Velocity = Vector3.Zero;
					_acceleration = Vector3.Normalize(world.Bill.Position - Position) * KamikazeAcceleration;
				}

				Velocity += _acceleration * time;
				Position += Velocity * time;
			}
		}

		public override void PerformCollission(VoxelWorld world, float time)
		{
			if (_kamikaze)
			{
				Destroyed = true;
				world.Explode(Position);
			}
		}
	}
}