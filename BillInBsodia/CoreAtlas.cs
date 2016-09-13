using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class CoreAtlas : GameComponent
	{
		private const float Scale = 2.0f;
		private readonly BillGame _game;
		private readonly Rectangle[] _shadows = new Rectangle[12];

		private readonly Rectangle _billFace = new Rectangle(32, 0, 32, 32);
		private readonly Rectangle _evilFace = new Rectangle(64, 0, 32, 32);
		private readonly Vector2 _deltaX = new Vector2(16, -8) * Scale;
		private readonly Vector2 _deltaY = new Vector2(16, 8) * Scale;
		private readonly Vector2 _deltaZ = new Vector2(0, -16) * Scale;

		private readonly Vector2 _center = new Vector2(1280, 720) / 2.0f;

		private readonly Vector2 _cubeOffset = new Vector2(0, 8);
		private Texture2D _texture;

		private Dictionary<CubeType, Rectangle> _cubes;

		private Dictionary<int, Rectangle> _bugs;

		private Dictionary<int, Rectangle> _bullets;
		private Dictionary<int, Rectangle> _weapons;
		private Dictionary<int, Rectangle> _hearts;

		public CoreAtlas(BillGame game)
			: base(game)
		{
			_game = game;
		}

		public Rectangle BillFace
		{
			get { return _billFace; }
		}

		public Rectangle EvilFace
		{
			get { return _evilFace; }
		}

		public Texture2D Texture
		{
			get { return _texture; }
		}

		public Dictionary<int, Rectangle> Weapons
		{
			get { return _weapons; }
		}

		public Dictionary<int, Rectangle> Hearts
		{
			get { return _hearts; }
		}

		public void Initialize()
		{
			_hearts = new Dictionary<int, Rectangle>();
			_weapons = new Dictionary<int, Rectangle>();
			_texture = _game.Content.Load<Texture2D>("Textures/Core-Atlas");
			_bugs = new Dictionary<int, Rectangle>();

			_cubes = new Dictionary<CubeType, Rectangle>();

			RegisterShadows();

			_bugs[0] = new Rectangle(40, 32, 16, 16);
			_bugs[1] = new Rectangle(56, 32, 16, 16);
			_bugs[2] = new Rectangle(72, 32, 16, 16);
			_bugs[3] = new Rectangle(88, 32, 16, 16);

			_bullets = new Dictionary<int, Rectangle>();
			_bullets[0] = new Rectangle(32, 48, 8, 8);
			_bullets[1] = new Rectangle(40, 48, 8, 8);
			_bullets[2] = new Rectangle(48, 48, 8, 8);

			_cubes[CubeType.Empty] = new Rectangle(0, 96, 32, 32);
			_cubes[CubeType.Destructible1] = new Rectangle(0, 96, 32, 32);
			_cubes[CubeType.Destructible2] = new Rectangle(32, 96, 32, 32);
			_cubes[CubeType.Destructible3] = new Rectangle(64, 96, 32, 32);
			_cubes[CubeType.Destructible4] = new Rectangle(96, 96, 32, 32);
			_cubes[CubeType.Indestructible] = new Rectangle(96, 64, 32, 32);

			Weapons[0] = new Rectangle(32, 56, 8, 8);
			Weapons[1] = new Rectangle(32, 64, 8, 8);
			Weapons[2] = new Rectangle(40, 56, 16, 8);
			Weapons[3] = new Rectangle(40, 64, 16, 8);

			Hearts[0] = new Rectangle(96, 8, 16, 16);
			Hearts[1] = new Rectangle(112, 8, 16, 16);
		}

		private void RegisterShadows()
		{
			_shadows[0] = new Rectangle(0, 56, 8, 8);
			_shadows[1] = new Rectangle(8, 56, 8, 8);
			_shadows[2] = new Rectangle(16, 56, 8, 8);
			_shadows[3] = new Rectangle(24, 56, 8, 8);
			_shadows[4] = new Rectangle(0, 64, 8, 8);
			_shadows[5] = new Rectangle(8, 64, 8, 8);
			_shadows[6] = new Rectangle(16, 64, 8, 8);
			_shadows[7] = new Rectangle(24, 64, 8, 8);
			_shadows[8] = new Rectangle(0, 72, 8, 8);
			_shadows[9] = new Rectangle(8, 72, 8, 8);
			_shadows[10] = new Rectangle(16, 72, 8, 8);
			_shadows[11] = new Rectangle(24, 72, 8, 8);
		}

		public void DrawIntroCube(SpriteBatch spriteBatch, Vector3 position, Color color)
		{
			// NOTE: Left for intro only. I can't be arsed to reposition stuff again.

			spriteBatch.Draw(_texture, new Vector2(position.X, position.Y + position.Z / 2.0f), _cubes[0], color, 0.0f,
								  new Vector2(16, 16), 2.0f, SpriteEffects.None, 0.0f);
		}


		private class SmartEntity : Entity
		{
			private readonly Func<EntityDrawInfo> _getInfo;
			private Vector2 _origin;

			public SmartEntity(Func<EntityDrawInfo> getInfo, Vector2 origin)
			{
				_getInfo = getInfo;
				_origin = origin;
			}

			public override float Size
			{
				get { return 0.0f; }
			}

			public override EntityDrawInfo DrawInfo
			{
				get {
					var info = _getInfo();
					info.Origin = _origin;
					return info;
				}
			}

			public override bool HasShadow
			{
				get { return false; }
			}

			public override void Update(VoxelWorld world, float time)
			{
			}
		}

		private SmartEntity _blaster = new SmartEntity(() => Blaster._drawInfo, new Vector2(-4, 8));
		private SmartEntity _autoBlaster = new SmartEntity(() => AutoBlaster._drawInfo, new Vector2(-4, 8));
		private SmartEntity _launcher = new SmartEntity(() => Launcher._drawInfo, new Vector2(4, 12));
		private SmartEntity _autoLauncher = new SmartEntity(() => AutoLauncher._drawInfo, new Vector2(4, 12));

		private void DrawGunToo(SpriteBatch spriteBatch, Entity entity, Vector2 focus, float groundHeight)
		{
			Player player = (Player) entity;

			switch (player.ActiveWeapon)
			{
				case 0:
					_blaster.Position = player.Position;
					DrawWorldEntity(spriteBatch, _blaster, focus, groundHeight);
					break;
				case 1:
					_autoBlaster.Position = player.Position;
					DrawWorldEntity(spriteBatch, _autoBlaster, focus, groundHeight);
					break;
				case 2:
					_launcher.Position = player.Position;
					DrawWorldEntity(spriteBatch, _launcher, focus, groundHeight);
					break;
				case 3:
					_autoLauncher.Position = player.Position;
					DrawWorldEntity(spriteBatch, _autoLauncher, focus, groundHeight);
					break;
			}
		}

		public void DrawWorldCube(SpriteBatch spriteBatch, Vector3 position, Voxel cube, Vector2 focus)
		{
			Color color = cube.Color;
			Vector2 screenPosition = CalculateScreenPosition(position, focus);

			if (screenPosition.X >= -100 && screenPosition.X <= 1380)
			{
				if (screenPosition.Y >= -100 && screenPosition.Y <= 820)
				{
					spriteBatch.Draw(_texture, screenPosition, _cubes[cube.Type], color, 0.0f, _cubeOffset, Scale, SpriteEffects.None,
										  0.0f);
				}
			}
		}

		private Vector2 CalculateShadowScreenPosition(Vector3 position, Vector2 focus, float groundHeight)
		{
			Vector2 screenPosition = position.X * _deltaX + position.Y * _deltaY + groundHeight * _deltaZ;
			screenPosition -= focus.X * _deltaX + focus.Y * _deltaY;
			screenPosition += _center;
			return screenPosition;
		}

		private Vector2 CalculateScreenPosition(Vector3 position, Vector2 focus)
		{
			Vector2 screenPosition = position.X * _deltaX + position.Y * _deltaY + position.Z * _deltaZ;
			screenPosition -= focus.X * _deltaX + focus.Y * _deltaY;
			screenPosition += _center;
			return screenPosition;
		}

		public void DrawWorldEntity(SpriteBatch spriteBatch, Entity entity, Vector2 focus, float groundHeight)
		{
			EntityDrawInfo info = entity.DrawInfo;
			Vector2 screenPosition = CalculateScreenPosition(entity.Position, focus);

			float scale = (info.CustomScale ?? 1.0f) * Scale;
			float alpha = info.CustomAlpha ?? 1.0f;

			if (entity.HasShadow)
			{
				Vector2 shadowPosition = CalculateShadowScreenPosition(entity.Position, focus, groundHeight);
				int shadowKind = GetShadowKind(entity.Size, entity.Position.Z);
				spriteBatch.Draw(_texture, shadowPosition, _shadows[shadowKind], Color.White * alpha, 0.0f, Vector2.One * 4.0f,
									  scale, SpriteEffects.None, 0.0f);
			}
			spriteBatch.Draw(_texture, screenPosition, info.Rectangle, Color.White * alpha, 0.0f, info.Origin, scale,
								  SpriteEffects.None, 0.0f);

			if (entity is Player)
			{
				DrawGunToo(spriteBatch, entity, focus, groundHeight);
			}
		}

		private static int GetShadowKind(float size, float height)
		{
			int sizeKind = 0;
			if (size >= 0.3f)
			{
				sizeKind = 1;
			}
			if (size >= 0.6f)
			{
				sizeKind = 2;
			}
			if (size >= 0.9f)
			{
				sizeKind = 3;
			}

			int heightKind = 0;
			if (height >= 0.5f)
			{
				heightKind = 1;
			}
			if (height > 1.0f)
			{
				heightKind = 2;
			}

			return sizeKind + heightKind * 4;
		}
	}
}