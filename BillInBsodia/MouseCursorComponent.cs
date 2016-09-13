using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD48_23
{
	public class MouseCursorComponent : DrawableGameComponent
	{
		private readonly BillGame _game;
		private Texture2D _texture;

		public MouseCursorComponent(BillGame game) : base(game)
		{
			_game = game;
		}

		protected override void LoadContent()
		{
			_texture = Game.Content.Load<Texture2D>("Textures/crosshair");
		}

		public override void Draw(GameTime gameTime)
		{
			MouseState mouse = Mouse.GetState();

			_game.SharedSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
			                              DepthStencilState.Default, RasterizerState.CullNone);
			_game.SharedSpriteBatch.Draw(_texture, new Vector2(mouse.X, mouse.Y), null, Color.White, 0.0f,
			                             new Vector2(7.0f, 7.0f), 2.0f, SpriteEffects.None, 0.0f);
			_game.SharedSpriteBatch.End();
		}
	}
}