using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class GameOverComponent : DrawableGameComponent
	{
		private readonly BillGame _game;
		private Texture2D _texture;

		public GameOverComponent(BillGame game) : base(game)
		{
			_game = game;
		}

		protected override void LoadContent()
		{
			_texture = Game.Content.Load<Texture2D>("Textures/GameOver");
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch sb = _game.SharedSpriteBatch;

			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			sb.Draw(_texture, Vector2.Zero, Color.White);
			sb.End();
		}
	}
}
