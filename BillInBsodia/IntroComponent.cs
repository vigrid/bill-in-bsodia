using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class IntroComponent : DrawableGameComponent
	{
		public const double FlatCue = 2.0;
		public const double TransitionCue = 5.0;
		public const double Cue3 = 6.0;
		public const int StartX = 150;
		public const int StartY = 230;
		public static readonly Color BsodColor = new Color(0, 0, 128, 255);
		public static readonly Color BsodLightColor = new Color(16, 16, 144, 255);
		private readonly OneShotSound _transformSound = new OneShotSound("Sounds/Transform");

		private readonly Vector2 _deltaX = new Vector2(32, -16);
		private readonly Vector2 _deltaY = new Vector2(32, 16);

		private readonly BillGame _game;
		private readonly InputComponent _inputComponent;
		private Texture2D _bsod;
		private Vector2 _deltaZ = new Vector2(0, 32);

		private int _width;
		private int _height;
		private List<Vector3> _cubes;
		private Voxel[] _voxels;

		private Texture2D _grid;
		private Texture2D _dot;
		private bool _flashed;

		public IntroComponent(BillGame game, InputComponent inputComponent)
			: base(game)
		{
			_game = game;
			_inputComponent = inputComponent;
		}

		public override void Initialize()
		{
			_bsod = Game.Content.Load<Texture2D>("Textures/bsod");

			_width = _bsod.Width;
			_height = _bsod.Height;

			var bsodData = new Color[_width * _height];

			_bsod.GetData(bsodData);

			_voxels = new Voxel[bsodData.Length];

			for (int i = 0; i < bsodData.Length; i++)
			{
				_voxels[i].Type = bsodData[i] == BsodColor ? CubeType.Empty : CubeType.Destructible1;
			}

			_game.Atlas.Initialize();

			_grid = Game.Content.Load<Texture2D>("Textures/shadow-grid");
			_dot = Game.Content.Load<Texture2D>("Textures/dot");
		}

		public override void Draw(GameTime gameTime)
		{
			_game.HudComponent.Visible = false;
			_game.HudComponent.Enabled = false;
			_game.ChatComponent.Enabled = false;
			_game.ChatComponent.Visible = false;
			_game.WorldComponent.Enabled = false;
			_game.WorldComponent.Visible = false;

			double seconds = gameTime.TotalGameTime.TotalSeconds;

			if (seconds < FlatCue)
			{
				DrawFlat(gameTime);
			}
			else if (seconds < TransitionCue)
			{
				_transformSound.Play(_game);
				DrawTransition(gameTime);
			}
			else if (seconds < Cue3)
			{
				if (!_flashed) // ME IS A MORON!
				{
					_flashed = true;
					_game.FlashComponent.Flash(Color.White, 0.2f);
				}
				DrawIsometric(gameTime);
			}
			else
			{
				_game.Controllable = true;

				DrawIsometric(gameTime);
			}
		}


		private void DrawTransition(GameTime gameTime)
		{
			var maxScale = (float) (32.0f * Math.Sqrt(2.0));

			Vector2 center = new Vector2(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height) / 2.0f;

			SpriteBatch sb = _game.SharedSpriteBatch;

			var transitionTime = (float) (gameTime.TotalGameTime.TotalSeconds - FlatCue);

			const float firstRotationCue = 1.0f;
			const float secondRotationCue = 2.0f;

			// DON'T TOUCH THIS!
			var destination = new Vector2(StartX, StartY);
			destination += new Vector2(StartX + 39, StartY + 24);

			if (transitionTime < firstRotationCue)
			{
				sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
				         RasterizerState.CullNone);
				sb.Draw(_bsod, center, null, Color.White, MathHelper.SmoothStep(0.0f, -MathHelper.PiOver4, transitionTime),
				        center / 2.0f, 2.0f, SpriteEffects.None, 1.0f);
				sb.End();
			}
			else if (transitionTime < secondRotationCue)
			{
				sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
				         RasterizerState.CullNone);

				float time = transitionTime - firstRotationCue;
				Vector2 centerThis = Vector2.SmoothStep(center, destination, time);
				sb.Draw(_bsod, centerThis, null, Color.White, -MathHelper.PiOver4, centerThis / 2.0f,
				        MathHelper.SmoothStep(2.0f, maxScale, time), SpriteEffects.None, 1.0f);
				sb.End();
			}
			else
			{
				float time = transitionTime - secondRotationCue;
				float scale = MathHelper.SmoothStep(1.0f, 0.5f, time);
				Matrix transform = Matrix.CreateScale(1.0f, scale, 1.0f);
				sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
				         RasterizerState.CullNone, null, transform);

				Vector2 centerThis = destination;

				sb.Draw(_bsod, centerThis, null, Color.White, -MathHelper.PiOver4, centerThis / 2.0f, maxScale, SpriteEffects.None,
				        1.0f);
				sb.End();
			}
		}

		private void DrawFlat(GameTime gameTime)
		{
			SpriteBatch sb = _game.SharedSpriteBatch;
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
			         RasterizerState.CullNone);

			sb.Draw(_bsod, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 1.0f);

			sb.End();
		}

		private void DrawIsometric(GameTime gameTime)
		{
			if (_game.ChatComponent.Completed)
			{
				Enabled = false;
				Visible = false;

				_game.HudComponent.Visible = true;
				_game.HudComponent.Enabled = true;
				_game.WorldComponent.Enabled = true;
				_game.WorldComponent.Visible = true;

				_game.FlashComponent.Flash(BsodColor, 0.5f);

				return;
			}

			_game.ChatComponent.Enabled = true;
			_game.ChatComponent.Visible = true;

			float zOffset = MathHelper.SmoothStep(0.0f, 50.0f, (float) (gameTime.TotalGameTime.TotalSeconds - Cue3));

			int xo = StartX; //(int)(200 + Math.Cos(gameTime.TotalGameTime.TotalSeconds) * 50.0f);
			int yo = StartY; //(int)(200 + Math.Sin(gameTime.TotalGameTime.TotalSeconds) * 50.0f);

			SpriteBatch sb = _game.SharedSpriteBatch;
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
			         RasterizerState.CullNone);

			var screenCenter = new Vector3(-640, 400, 0);

			for (int x = 40; x >= 0; x--)
				for (int y = 0; y < 40; y++)
				{
					Vector2 position = x * _deltaX + y * _deltaY;

					var cubePosition = new Vector3(position.X, position.Y, 0.0f);

					int xx = x + xo;
					int yy = y + yo;

					CubeType type = _voxels[xx + yy * _width].Type;
					Color color = type == CubeType.Destructible1 ? Color.White : BsodLightColor;
					if (type == CubeType.Destructible1)
					{
						cubePosition.Z -= zOffset;
					}

					_game.Atlas.DrawIntroCube(sb, cubePosition + screenCenter, color);
				}

			sb.End();
		}
	}
}