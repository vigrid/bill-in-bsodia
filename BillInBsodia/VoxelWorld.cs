using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class VoxelWorld
	{
		private readonly Color _billStartColor = new Color(0, 255, 0, 255);
		private readonly Color _evilStartColor = new Color(255, 0, 0, 255);
		private readonly Color _ammoColor = new Color(0, 0, 255, 255);
		private readonly Color _weaponColor = new Color(0, 255, 255, 255);

		private readonly Color _indestructibleColor = new Color(255, 255, 0, 255);
		private readonly Color _wallColor = new Color(128, 128, 128, 255);
		private readonly Color _lightlyDamagedColor = new Color(255, 255, 255, 255);
		private readonly Color _heavilyDamagedColor = new Color(0, 0, 0, 255);

		private readonly int _width;
		private readonly int _height;

		private readonly List<Entity> _entities;

		private readonly Voxel[] _data;
		private readonly List<Entity> _entitiesToAdd = new List<Entity>();

		public VoxelWorld(Texture2D texture)
		{
			_width = texture.Width;
			_height = texture.Height;

			_entities = new List<Entity>();
			var buffer = new Color[Width * Height];
			_data = new Voxel[buffer.Length];

			texture.GetData(buffer);

			var voxel = new Voxel();

			for (int i = 0; i < buffer.Length; i++)
			{
				int x = i % texture.Width;
				int y = i / texture.Width;

				if (buffer[i] == _indestructibleColor)
				{
					voxel.Type = CubeType.Indestructible;
					voxel.Color = Color.DarkGray;
				}
				else if (buffer[i] == _wallColor)
				{
					voxel.Type = CubeType.Destructible1;
					voxel.Color = Color.White;
				}
				else if (buffer[i] == _lightlyDamagedColor)
				{
					voxel.Type = CubeType.Destructible2;
					voxel.Color = Color.White;
				}
				else if (buffer[i] == _heavilyDamagedColor)
				{
					voxel.Type = CubeType.Destructible3;
					voxel.Color = Color.White;
				}
				else if (buffer[i] == _ammoColor)
				{
					var coord = new Vector3(x + 0.5f, y + 0.5f, 0.0f);
					_entities.Add(AmmoFactory.Next(coord));
					voxel.Type = CubeType.Empty;
					voxel.Color = IntroComponent.BsodColor;
				}
				else if (buffer[i] == _weaponColor)
				{
					var coord = new Vector3(x + 0.5f, y + 0.5f, 0.0f);
					_entities.Add(WeaponFactory.Next(coord));
					voxel.Type = CubeType.Empty;
					voxel.Color = IntroComponent.BsodColor;
				}
				else if (buffer[i] == _billStartColor)
				{
					var coord = new Vector3(x + 0.5f, y + 0.5f, 0.0f);
					_entities.Add(new Player(coord));
					voxel.Type = CubeType.Empty;
					voxel.Color = IntroComponent.BsodColor;
				}
				else if (buffer[i] == _evilStartColor)
				{
					var coord = new Vector3(x + 0.5f, y + 0.5f, 0.0f);
					_entities.Add(new Boss(coord));
					voxel.Type = CubeType.Empty;
					voxel.Color = IntroComponent.BsodColor;
				}
				else
				{
					buffer[i].A = 255;
					voxel.Type = CubeType.Empty;
					voxel.Color = buffer[i];
				}

				this[x, y] = voxel;
			}
		}

		private Player _bill;

		public Player Bill
		{
			get { return _bill ?? (_bill = _entities.OfType<Player>().First()); }
		}

		private Boss _steephen;
		public Boss Steephen
		{
			get { return _steephen ?? (_steephen = _entities.OfType<Boss>().First()); }
		}

		public IEnumerable<Entity> Entities
		{
			get { return _entities.Where(entity => entity.Active); }
		}

		public int Width
		{
			get { return _width; }
		}

		public int Height
		{
			get { return _height; }
		}

		public Voxel this[int x, int y]
		{
			get { return _data[GetIndex(x, y)]; }
			set { _data[GetIndex(x, y)] = value; }
		}

		private int GetIndex(int x, int y)
		{
			return x % Width + (y % Height) * Width;
		}

		public void RemoveDestroyedEntities()
		{
			_entities.RemoveAll(entity => entity.Destroyed);
		}

		public void AddWaitingEntities()
		{
			_entities.AddRange(_entitiesToAdd);
			_entitiesToAdd.Clear();
		}

		public void RegisterEntity(Entity entity)
		{
			_entitiesToAdd.Add(entity);
		}

		public const float ExplosionRadius = 2.5f;

		public void Explode(Vector3 position)
		{
			for (int i = 0; i < 40; i++)
			{
				var smoke = new ExplosionSmoke(position);
				smoke._velocity = (BillGame.Random.GetVector3() + Vector3.UnitZ) * 5.0f;
				RegisterEntity(smoke);
			}

			BillGame.Instance.PlaySound("Sounds/Explosion");

			int cubeX = (int) position.X;
			int cubeY = (int) position.Y;

			for (int x = cubeX - 5; x <= cubeX + 5; x++)
			{
				for (int y = cubeY - 5; y <= cubeY + 5; y++)
				{
					Vector3 cubeCenter = new Vector3(x + 0.5f, y + 0.5f, 0.5f);
					var distance = Vector3.Distance(cubeCenter, position);
					if (distance < ExplosionRadius)
					{
						TryDamageAt(x, y);
					}
				}
			}

			foreach (var entity in _entities.OfType<Mob>())
			{
				var distanceToMob = Vector3.Distance(entity.Position, position);
				if (distanceToMob < ExplosionRadius)
				{
					entity.Damage(3);
				}
			}

			var distanceToSteephen = Vector3.Distance(Steephen.Position, position);
			if (distanceToSteephen < ExplosionRadius)
			{
				Steephen.Damage(3);
			}
		}

		private void TryDamageAt(int x, int y)
		{
			var cube = this[x, y];
			cube.Color = Color.Lerp(cube.Color, Color.Black, 0.15f);
			switch (cube.Type)
			{
				case CubeType.Destructible1:
					cube.Type = CubeType.Destructible2;
					break;
				case CubeType.Destructible2:
					cube.Type = CubeType.Destructible3;
					break;
				case CubeType.Destructible3:
					cube.Type = CubeType.Destructible4;
					break;
				case CubeType.Destructible4:
					cube.Type = CubeType.Empty;
					break;
			}
			this[x, y] = cube;
		}

		public void ClearMobs()
		{
			_entities.RemoveAll(e => e is Mob);
		}
	}

	public class ExplosionSmoke : Smoke
	{
		public ExplosionSmoke(Vector3 position) : base(position)
		{
		}

		public override float MaxLife
		{
			get { return 1.0f; }
		}

		public override EntityDrawInfo DrawInfo
		{
			get
			{
				var fromBase = base.DrawInfo;
				fromBase.CustomAlpha = Math.Min(1.0f, fromBase.CustomAlpha.Value);
				fromBase.CustomScale *= 2.0f;
				return fromBase;
			}
		}

		public override void Update(VoxelWorld world, float time)
		{
			_lived += time;
			if (LifePercentage > 1.0f || Position.Z < 0.0f)
			{
				Destroyed = true;
				return;
			}

			Position += _velocity * time;
			_velocity -= time * Vector3.UnitZ * Player.Gravity;
		}
	}
}