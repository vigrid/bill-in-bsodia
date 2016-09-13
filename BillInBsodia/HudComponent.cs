using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class HudComponent : DrawableGameComponent
	{
		private readonly BillGame _game;

		public HudComponent(BillGame game) : base(game)
		{
			_game = game;
		}

		public override void Draw(GameTime gameTime)
		{
			Player bill = _game.WorldComponent.Bill;

			SpriteBatch sb = _game.SharedSpriteBatch;

			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
			         RasterizerState.CullNone);

			if (bill.HasBlaster || bill.HasAutoBlaster)
			{
				sb.Draw(_game.ChatComponent.Dot, new Rectangle(32, 620, 88, 80), Color.Black * 0.75f);
				sb.DrawString(_game.ChatComponent.Font, bill.BlasterAmmo.ToString(), new Vector2(60, 668), Color.Yellow);
			}
			if (bill.HasBlaster)
			{
				sb.Draw(_game.Atlas.Texture, new Vector2(42, 632), _game.Atlas.Weapons[0], Color.Black * 0.75f, 0.0f, Vector2.Zero,
				        4.0f, SpriteEffects.None, 0.0f);
				sb.Draw(_game.Atlas.Texture, new Vector2(40, 628), _game.Atlas.Weapons[0], Color.White, 0.0f, Vector2.Zero, 4.0f,
				        SpriteEffects.None, 0.0f);
			}
			if (bill.HasAutoBlaster)
			{
				// + 32 + 8
				sb.Draw(_game.Atlas.Texture, new Vector2(82, 632), _game.Atlas.Weapons[1], Color.Black * 0.75f, 0.0f, Vector2.Zero,
				        4.0f, SpriteEffects.None, 0.0f);
				sb.Draw(_game.Atlas.Texture, new Vector2(80, 628), _game.Atlas.Weapons[1], Color.White, 0.0f, Vector2.Zero, 4.0f,
				        SpriteEffects.None, 0.0f);
			}

			if (bill.HasLauncher || bill.HasDoubleLauncher)
			{
				sb.Draw(_game.ChatComponent.Dot, new Rectangle(114, 620, 152, 80), Color.Black * 0.75f);
				sb.DrawString(_game.ChatComponent.Font, bill.LauncherAmmo.ToString(), new Vector2(156, 668), Color.Yellow);
			}
			if (bill.HasLauncher)
			{
				// + 32 + 8
				sb.Draw(_game.Atlas.Texture, new Vector2(122, 632), _game.Atlas.Weapons[2], Color.Black * 0.75f, 0.0f, Vector2.Zero,
				        4.0f, SpriteEffects.None, 0.0f);
				sb.Draw(_game.Atlas.Texture, new Vector2(120, 628), _game.Atlas.Weapons[2], Color.White, 0.0f, Vector2.Zero, 4.0f,
				        SpriteEffects.None, 0.0f);
			}
			if (bill.HasDoubleLauncher)
			{
				// + 64 + 8
				sb.Draw(_game.Atlas.Texture, new Vector2(194, 632), _game.Atlas.Weapons[3], Color.Black * 0.75f, 0.0f, Vector2.Zero,
				        4.0f, SpriteEffects.None, 0.0f);
				sb.Draw(_game.Atlas.Texture, new Vector2(192, 628), _game.Atlas.Weapons[3], Color.White, 0.0f, Vector2.Zero, 4.0f,
				        SpriteEffects.None, 0.0f);
			}

			for (int i = 0; i < Player.MaxHealth; i++)
			{
				var position = new Vector2(40 + i * 32, 632 - 48);
				sb.Draw(_game.Atlas.Texture, position, _game.Atlas.Hearts[i < bill.Health ? 0 : 1], Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 0.0f);
			}

			var bossHealthFraction = _game.WorldComponent.Steephen.HealthFraction;
			if (bossHealthFraction != 1.0f && bossHealthFraction > 0.0f)
			{
				sb.Draw(_game.ChatComponent.Dot, new Rectangle(200, 20, 880, 40), Color.Black * 0.75f);
				sb.Draw(_game.ChatComponent.Dot, new Rectangle(205, 25, (int) (870 * bossHealthFraction), 30), Color.Red * 0.75f);
			}

			sb.End();
		}
	}
}