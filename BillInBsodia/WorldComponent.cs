using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class WorldComponent : DrawableGameComponent
	{
		private readonly BillGame _game;

		private readonly Vector3 _isoUp = Vector3.Normalize(new Vector3(1.0f, -1.0f, 0.0f));
		private readonly Vector3 _isoDown = Vector3.Normalize(new Vector3(-1.0f, 1.0f, 0.0f));
		private readonly Vector3 _isoLeft = Vector3.Normalize(new Vector3(-1.0f, -1.0f, 0.0f));
		private readonly Vector3 _isoRight = Vector3.Normalize(new Vector3(1.0f, 1.0f, 0.0f));
		private VoxelWorld _world;

		private Dictionary<int, List<Entity>> _cache;
		private Vector2 _focus;
		private int _minX, _maxX, _minY, _maxY;

		public WorldComponent(BillGame game)
			: base(game)
		{
			_game = game;
		}

		public Player Bill
		{
			get { return _world.Bill; }
		}

		public Boss Steephen
		{
			get { return _world.Steephen; }
		}

		protected override void LoadContent()
		{
			var mapTexture = Game.Content.Load<Texture2D>("Textures/map");

			_world = new VoxelWorld(mapTexture);
		}

		public override void Update(GameTime gameTime)
		{
			if (CheckBillDead())
			{
				return;
			}

			UpdateBill(gameTime);
			UpdateWorld(gameTime);

			CheckTeleportable();
		}

		private void CheckTeleportable()
		{
			if (_world.Bill.HasTeleporter)
			{
				_game.ChatComponent._currentLine = 0;
				_game.ChatComponent._lines.Clear();
				_game.ChatComponent._lines.Add(new ChatComponent.ChatLine
				                               	{
				                               		Who = "William",
				                               		What = "Woohoo! I have the Teleporter\n\n[T] to Teleport and win!"
				                               	});
				_game.ChatComponent.Visible = true;
				_game.ChatComponent.Enabled = true;
				_game.ChatComponent.CanAdvance = false;

				if (_game.InputComponent.Teleport)
				{
					Bill.HasTeleporter = false;
					Bill.Stop();

					_world.RegisterEntity(new BeamEntity(Bill.Position));
					_world.ClearMobs();

					BillGame.Instance.FlashComponent.FadeToBlack();
					BillGame.Instance.FlashComponent.Enabled = true;
					BillGame.Instance.FlashComponent.Visible = true;
					BillGame.Instance.WonComponent.Enabled = true;
					BillGame.Instance.WonComponent.Visible = true;
				}
			}
		}

		private bool CheckBillDead()
		{
			if (_world.Bill.Destroyed)
			{
				_game.GameOverComponent.Visible = true;
				_game.GameOverComponent.Enabled = true;

				return true;
			}

			return false;
		}

		private void UpdateWorld(GameTime gameTime)
		{
			CalcAverageMilestonePosition();

			Player bill = _world.Bill;

			_world.AddWaitingEntities();

			foreach (Entity entity in _world.Entities)
			{
				if (entity != bill)
				{
					entity.Update(_world, (float) gameTime.ElapsedGameTime.TotalSeconds);
				}
			}
		}

		private void CalcAverageMilestonePosition()
		{
			IterationBug.AveragePositionOfAll = Vector3.Zero;

			int count = 0;
			foreach (var milestoneBug in _world.Entities.OfType<IterationBug>())
			{
				count++;
				IterationBug.AveragePositionOfAll += milestoneBug.Position;
			}
			if (count != 0)
			{
				IterationBug.AveragePositionOfAll /= count;
			}
		}


		private void UpdateBill(GameTime gameTime)
		{
			Player bill = _world.Bill;
			bill.Update(_world, (float) gameTime.ElapsedGameTime.TotalSeconds);

			_focus = new Vector2(bill.Position.X, bill.Position.Y);

			var focusX = (int) _focus.X;
			var focusY = (int) _focus.Y;

			const int range = 22;

			_minX = Math.Max(focusX - range, 0);
			_maxX = Math.Min(focusX + range, _world.Width - 1);
			_minY = Math.Max(focusY - range, 0);
			_maxY = Math.Min(focusY + range, _world.Height - 1);

			_world.RemoveDestroyedEntities();

			_cache = CacheEntities(_minX, _maxX, _minY, _maxY);

			List<Entity> entitiesWithBill;
			if (_cache.TryGetValue(MakeKey(focusX, focusY), out entitiesWithBill))
			{
				foreach (PickableEntity entity in entitiesWithBill.OfType<PickableEntity>())
				{
					entity.Pick(bill);
					entity.Destroyed = true;
					_game.PlaySound("Sounds/PickUp");
				}
			}

			Vector2 mousePosition = _game.InputComponent.MousePosition;

			var mouseFraction = new Vector2(mousePosition.X / _game.GraphicsDevice.Viewport.Width,
			                                mousePosition.Y / _game.GraphicsDevice.Viewport.Height);
			// 0.5, 0.5 is center

			Vector2 scale = new Vector2(21.0f, 23.0f) * 1.41f; // 21 cubes across, 23 down on the screen
			Vector2 revDeltaX = Vector2.Normalize(new Vector2(1.0f));
			Vector2 revDeltaY = revDeltaX;
			revDeltaY.X *= -1.0f;

			// AIMING IS "CLOSE ENOUGH". I GIVE UP :D

			Vector2 dv = mouseFraction - new Vector2(0.5f);
			Vector2 nv = revDeltaX * dv.X + revDeltaY * dv.Y;
			Vector2 target2D = new Vector2(bill.Position.X, bill.Position.Y) + nv * scale;
			var target = new Vector3(target2D, 0.25f);

			if (_game.InputComponent.Weapon1)
			{
				bill.TrySwitchToWeapon(0);
			}
			if (_game.InputComponent.Weapon2)
			{
				bill.TrySwitchToWeapon(1);
			}
			if (_game.InputComponent.Weapon3)
			{
				bill.TrySwitchToWeapon(2);
			}
			if (_game.InputComponent.Weapon4)
			{
				bill.TrySwitchToWeapon(3);
			}
			if (_game.InputComponent.Up)
			{
				bill.Accelerate(_isoUp);
			}
			if (_game.InputComponent.Down)
			{
				bill.Accelerate(_isoDown);
			}
			if (_game.InputComponent.Left)
			{
				bill.Accelerate(_isoLeft);
			}
			if (_game.InputComponent.Right)
			{
				bill.Accelerate(_isoRight);
			}
			if (_game.InputComponent.Jump)
			{
				if (bill.Jump())
				{
					_game.PlaySound("Sounds/Jump");
				}
			}
			if (_game.InputComponent.Shoot)
			{
				bill.Shoot(_world, gameTime, target);
			}
			else
			{
				bill.ReadyShoot();
			}
		}

		public override void Draw(GameTime gameTime)
		{
			CoreAtlas atlas = _game.Atlas;
			SpriteBatch spriteBatch = _game.SharedSpriteBatch;

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
			                  RasterizerState.CullNone);

			var cubePosition = new Vector3();

			for (int x = _maxX; x >= _minX; x--)
			{
				for (int y = _minY; y <= _maxY; y++)
				{
					cubePosition.X = x;
					cubePosition.Y = y;
					Voxel cube = _world[x, y];
					cubePosition.Z = cube.Type == CubeType.Empty ? 0.0f : 1.0f;

					atlas.DrawWorldCube(spriteBatch, cubePosition, cube, _focus);

					List<Entity> entities;
					if (_cache.TryGetValue(MakeKey(x, y), out entities))
					{
						foreach (Entity entity in entities)
						{
							atlas.DrawWorldEntity(spriteBatch, entity, _focus, cubePosition.Z);
						}
					}
				}
			}

			spriteBatch.End();
		}

		private Dictionary<int, List<Entity>> CacheEntities(int minX, int maxX, int minY, int maxY)
		{
			var result = new Dictionary<int, List<Entity>>();

			foreach (Entity entity in _world.Entities)
			{
				var x = (int) entity.Position.X;
				var y = (int) entity.Position.Y;

				int key = MakeKey(x, y);
				List<Entity> entitiesOnCube;

				if (!result.TryGetValue(key, out entitiesOnCube))
				{
					entitiesOnCube = new List<Entity>();
					result.Add(key, entitiesOnCube);
				}

				entitiesOnCube.Add(entity);
			}

			return result;
		}

		private static int MakeKey(int x, int y)
		{
			return y * 0x10000 + x;
		}

		private bool IsOnCube(Entity entity, Vector3 cubePosition)
		{
			Vector3 d = entity.Position - cubePosition;

			return d.X >= 0.0f && d.X < 1.0f && (d.Y >= 0.0f && d.Y < 1.0f);
		}

		public VoxelWorld World { get { return _world; } }
	}
}