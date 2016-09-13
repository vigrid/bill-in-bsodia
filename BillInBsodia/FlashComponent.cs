using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class FlashComponent : DrawableGameComponent
	{
		private readonly BillGame _game;

		private bool _justFlashed;
		private Color _color;
		private TimeSpan _flashed;
		private float _halfLife;
		private Color _neutralColor = Color.Transparent;

		public FlashComponent(BillGame game) : base(game)
		{
			_game = game;
		}

		public void Flash(Color color, float halfLife)
		{
			_color = color;
			_halfLife = halfLife;
			_justFlashed = true;
		}

		public override void Draw(GameTime gameTime)
		{
			if (_halfLife <= 0.0f)
			{
				return;
			}

			if (_justFlashed)
			{
				_flashed = gameTime.TotalGameTime;
				_justFlashed = false;
			}

			var elapsedSinceFlash = (float) (gameTime.TotalGameTime.TotalSeconds - _flashed.TotalSeconds);
			float halfTimesTimesTimes = elapsedSinceFlash / _halfLife;
			var f = (float) Math.Pow(0.5, halfTimesTimesTimes) * 1.05f - 0.05f;

			if (f > 0.0f && _neutralColor != Color.Black)
			{
				f = Math.Max(0.0f, f);
				SpriteBatch sb = _game.SharedSpriteBatch;

				sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None,
				         RasterizerState.CullNone);
				sb.Draw(_game.ChatComponent.Dot, Game.GraphicsDevice.Viewport.Bounds, Color.Lerp(_neutralColor, _color, f));
				sb.End();
			}
		}

		public void FadeToBlack()
		{
			_neutralColor = Color.Black;
			_color = Color.Transparent;
			_halfLife = 2.0f;
			_justFlashed = true;
		}
	}
}