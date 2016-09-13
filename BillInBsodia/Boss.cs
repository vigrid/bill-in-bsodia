using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class Boss : Mob
	{
		public const int MaxHealth = 200;

		public int Health { get; set; }
		public float HealthFraction { get { return Health / (float)MaxHealth; } }

		public const float SpawnIntervalNear = 0.75f;
		public const float SpawnIntervalFar = 10.0f;
		public const float SpawnRangeNear = 10.0f;
		public const float SpawnRangeFar = 100.0f;

		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																				{
																					Rectangle = new Rectangle(56, 48, 32, 32),
																					Origin = new Vector2(16, 28),
																					CustomScale = 1.5f
																				};

		private float _spawnTimer;

		public override string DamageSound
		{
			get { return "Sounds/BossDamaged"; }
		}

		public Boss(Vector3 position) : base(position)
		{
			Health = MaxHealth;
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
			get { return 0.0f; }
		}

		public override float MaxSpeed
		{
			get { return 0.0f; }
		}

		public override bool HasShadow
		{
			get { return true; }
		}

		public override CollideResult Collide(VoxelWorld world, float time)
		{
			return CollideResult.Air;
		}

		public override void Update(VoxelWorld world, float time)
		{
			ProcessSpawning(world, time);
		}

		private void ProcessSpawning(VoxelWorld world, float time)
		{
			float distance = Vector3.Distance(Position, world.Bill.Position);

			float range = (SpawnRangeFar - SpawnRangeNear);
			float fraction = MathHelper.Clamp((distance - SpawnRangeNear) / range, 0.0f, 1.0f);

			float currentSpawnInterval = MathHelper.Lerp(SpawnIntervalNear, SpawnIntervalFar, fraction);

			currentSpawnInterval *= MathHelper.Lerp(0.25f, 1.0f, HealthFraction);

			if (BillGame.Instance.InputComponent.Debug)
			{
				currentSpawnInterval *= 0.1f;
			}

			_spawnTimer += time;

			if (_spawnTimer > currentSpawnInterval)
			{
				_spawnTimer -= currentSpawnInterval;
				SpawnEnemy(world);
			}
		}

		private void SpawnEnemy(VoxelWorld world)
		{
			world.RegisterEntity(EnemyFactory.Next(Position));
			BillGame.Instance.PlaySound("Sounds/Spawn");
		}

		public override void PerformCollission(VoxelWorld world, float time)
		{
		}

		private bool _spawnedTeleporter;

		public override void Damage(int amount)
		{
			Health -= amount;
			if (Health <= 0)
			{
				for (int i = 0; i < 500; i++)
				{
					var smoke = new ExplosionSmoke(Position);
					smoke._velocity = (BillGame.Random.GetVector3() + Vector3.UnitZ) * 15.0f;
					BillGame.Instance.WorldComponent.World.RegisterEntity(smoke);
				}

				BillGame.Instance.PlaySound("Sounds/BossDeath");

				if (!_spawnedTeleporter)
				{
					_spawnedTeleporter = true;
					BillGame.Instance.WorldComponent.World.RegisterEntity(new Teleporter(Position));
				}

				Destroyed = true;
			}
		}
	}
}