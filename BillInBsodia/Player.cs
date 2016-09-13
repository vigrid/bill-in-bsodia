using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class Player : Entity
	{
		public const int MaxHealth = 10;

		public const float JumpStrength = 10.0f;
		public const float Acceleration = 28.0f;
		public const float MaxSpeed = 7.5f;
		public const float Gravity = 50.0f;
		public const float ShootHeight = 0.65f;
		public const float InvulnerabilityAfterHit = 1.0f;

		private static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
																				{
																					Rectangle = new Rectangle(32, 32, 8, 16),
																					Origin = new Vector2(4, 16),
																				};

		private Vector3 _velocity;
		private Vector3 _acceleration;
		private bool _jumped;
		private bool _canShoot = true;

		private TimeSpan _lastShotTime;

		public Player(Vector3 position)
		{
			Health = MaxHealth;

			ActiveWeapon = -1;

			Position = position;
		}

		public override float Size
		{
			get { return 0.5f; }
		}

		public int ActiveWeapon { get; set; }

		public int BlasterAmmo { get; set; }
		public bool HasBlaster { get; set; }
		public bool HasAutoBlaster { get; set; }

		public int LauncherAmmo { get; set; }
		public bool HasLauncher { get; set; }
		public bool HasDoubleLauncher { get; set; }

		public int Health { get; set; }

		public float _invulnerableTimeLeft;
		private bool _stopped;

		public override EntityDrawInfo DrawInfo
		{
			get { return _drawInfo; }
		}

		public override bool HasShadow
		{
			get { return true; }
		}

		public bool HasTeleporter { get; set; }

		public void Accelerate(Vector3 direction)
		{
			_acceleration += direction * Acceleration;
		}

		public bool Jump()
		{
			if (_stopped)
			{
				return false;
			}

			if (!_jumped)
			{
				_velocity.Z += JumpStrength;
				_jumped = true;
				return true;
			}

			return false;
		}

		public bool TryDamage()
		{
			if (_invulnerableTimeLeft > 0.0f)
			{
				return false;
			}

			_invulnerableTimeLeft = InvulnerabilityAfterHit;
			Health--;

			BillGame.Instance.PlaySound("Sounds/Damage");
			var spread = BillGame.Random.GetVector3();
			spread.Z *= 0.125f;
			_velocity += spread * 8.0f + Vector3.UnitZ * 8.0f;

			if (Health == 0)
			{
				Destroyed = true;
			}

			return true;
		}

		public override void Update(VoxelWorld world, float time)
		{
			if (_stopped)
			{
				return;
			}

			_invulnerableTimeLeft -= time;

			_velocity += _acceleration * time;
			var groundVelocity = new Vector2(_velocity.X, _velocity.Y);

			if (groundVelocity.Length() > MaxSpeed)
			{
				groundVelocity.Normalize();
				groundVelocity *= MaxSpeed;
			}
			_velocity.X = groundVelocity.X;
			_velocity.Y = groundVelocity.Y;

			Position += _velocity * time;

			if (_acceleration == Vector3.Zero)
			{
				_velocity.X *= (float)Math.Pow(0.005, time);
				_velocity.Y *= (float)Math.Pow(0.005, time);
			}

			_acceleration = Vector3.Zero;

			if (Position.Z > 0.0f)
			{
				_velocity.Z -= time * Gravity;
			}

			CollideResult collision = Collide(world, time);

			switch (collision)
			{
				case CollideResult.Air:
					break;
				case CollideResult.Standing:
					_jumped = false;
					break;
				case CollideResult.Collided:
					if (_velocity.Z < 0.0f) // landed
					{
						_velocity.Z = 0.0f;
					}
					else
					{
						// wallbanged
						Position -= 2.0f * _velocity * time;
						_velocity.X *= -1.0f;
						_velocity.Y *= -1.0f;
						BillGame.Instance.PlaySound("Sounds/Wall");
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void Shoot(VoxelWorld world, GameTime gameTime, Vector3 target)
		{
			if (_canShoot)
			{
				switch (ActiveWeapon)
				{
					case 0:
						TryFireBlaster(world, target, gameTime.TotalGameTime);
						break;
					case 1:
						TryFireAutoBlaster(world, target, gameTime.TotalGameTime);
						break;
					case 2:
						TryFireLauncher(world, target, gameTime.TotalGameTime);
						break;
					case 3:
						TryFireAutoLauncher(world, target, gameTime.TotalGameTime);
						break;
				}
			}
		}

		private void TryFireAutoLauncher(VoxelWorld world, Vector3 target, TimeSpan totalGameTime)
		{
			_canShoot = true;

			if (totalGameTime - _lastShotTime < TimeSpan.FromSeconds(0.4))
			{
				return;
			}
			_lastShotTime = totalGameTime;

			if (LauncherAmmo >= 1)
			{
				LauncherAmmo -= 1;
				BillGame.Instance.PlaySound("Sounds/LauncherShot");

				world.RegisterEntity(new RocketShot(new Vector3(Position.X, Position.Y, Position.Z + ShootHeight), target));
			}
			else
			{
				BillGame.Instance.PlaySound("Sounds/LauncherNoAmmo");
			}
		}

		private void TryFireLauncher(VoxelWorld world, Vector3 target, TimeSpan totalGameTime)
		{
			_canShoot = false;

			if (totalGameTime - _lastShotTime < TimeSpan.FromSeconds(0.6))
			{
				return;
			}
			_lastShotTime = totalGameTime;

			if (LauncherAmmo >= 1)
			{
				LauncherAmmo -= 1;
				BillGame.Instance.PlaySound("Sounds/LauncherShot");

				world.RegisterEntity(new RocketShot(new Vector3(Position.X, Position.Y, Position.Z + ShootHeight), target));
			}
			else
			{
				BillGame.Instance.PlaySound("Sounds/LauncherNoAmmo");
			}
		}

		private void TryFireAutoBlaster(VoxelWorld world, Vector3 target, TimeSpan totalGameTime)
		{
			_canShoot = true;

			if (totalGameTime - _lastShotTime < TimeSpan.FromSeconds(0.15))
			{
				return;
			}
			_lastShotTime = totalGameTime;

			if (BlasterAmmo >= 1)
			{
				BlasterAmmo -= 1;
				BillGame.Instance.PlaySound("Sounds/BlasterShot");

				world.RegisterEntity(new BlasterShot(new Vector3(Position.X, Position.Y, Position.Z + ShootHeight), target));
			}
			else
			{
				BillGame.Instance.PlaySound("Sounds/BlasterNoAmmo");
			}
		}

		private void TryFireBlaster(VoxelWorld world, Vector3 target, TimeSpan totalGameTime)
		{
			_canShoot = false;

			if (totalGameTime - _lastShotTime < TimeSpan.FromSeconds(0.1))
			{
				return;
			}
			_lastShotTime = totalGameTime;

			if (BlasterAmmo >= 1)
			{
				BlasterAmmo -= 1;
				BillGame.Instance.PlaySound("Sounds/BlasterShot");

				world.RegisterEntity(new BlasterShot(new Vector3(Position.X, Position.Y, Position.Z + ShootHeight), target));
			}
			else
			{
				BillGame.Instance.PlaySound("Sounds/BlasterNoAmmo");
			}
		}

		public void ReadyShoot()
		{
			_canShoot = true;
		}

		public void TrySwitchToWeapon(int weaponNumber)
		{
			if (_canShoot)
			{
				switch (weaponNumber)
				{
					case 0:
						if (HasBlaster)
						{
							ActiveWeapon = 0;
						}
						break;
					case 1:
						if (HasAutoBlaster)
						{
							ActiveWeapon = 1;
						}
						break;
					case 2:
						if (HasLauncher)
						{
							ActiveWeapon = 2;
						}
						break;
					case 3:
						if (HasDoubleLauncher)
						{
							ActiveWeapon = 3;
						}
						break;
				}
			}
		}

		public void Stop()
		{
			_stopped = true;
			_velocity = Vector3.Zero;
			_acceleration = Vector3.Zero;
		}
	}
}