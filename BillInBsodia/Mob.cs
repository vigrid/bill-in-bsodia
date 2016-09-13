using Microsoft.Xna.Framework;

namespace LD48_23
{
	public abstract class Mob : Entity
	{
		public Vector3 Velocity;

		public abstract float Acceleration { get; }
		public abstract float MaxSpeed { get; }

		protected Mob(Vector3 position)
		{
			Position = position;
		}

		public override bool HasShadow
		{
			get { return true; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			var direction = Vector3.Normalize(world.Bill.Position - Position);
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

		protected void CollideWithBill()
		{
			var bill = BillGame.Instance.WorldComponent.Bill;
			var distanceToBill = Vector3.Distance(bill.Position, Position);
			if (distanceToBill < Size + bill.Size)
			{
				if (bill.TryDamage())
				{
					if (DestructibleOnCollission)
					{
						Destroyed = true;
					}
				}
			}
		}

		protected virtual bool DestructibleOnCollission
		{
			get { return false; }
		}

		public abstract void PerformCollission(VoxelWorld world, float time);

		public virtual string DamageSound
		{
			get { return "Sounds/MobDamaged"; }
		}

		public virtual void Damage(int amount)
		{
			BillGame.Instance.PlaySound(DamageSound);
			Health -= amount;
			if (Health <= 0)
			{
				Destroyed = true;
				for (int i = 0; i < 20; i++)
				{
					var smoke = new ExplosionSmoke(Position);
					smoke._velocity = (BillGame.Random.GetVector3() + Vector3.UnitZ) * 5.0f;
					BillGame.Instance.WorldComponent.World.RegisterEntity(smoke);
				}
			}
		}

		protected int Health;
	}
}