using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class Won : DrawableGameComponent
	{
		private readonly BillGame _game;

		private double _elapsed;
		private Texture2D _texture;

		public Won(BillGame game) : base(game)
		{
			_game = game;
		}

		protected override void LoadContent()
		{
			_texture = Game.Content.Load<Texture2D>("Textures/Won");
		}

		public override void Draw(GameTime gameTime)
		{
			var sb = _game.SharedSpriteBatch;
			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
			sb.Draw(_texture, Vector2.Zero, Color.White * Alpha);
			sb.End();
		}

		protected float Alpha
		{
			get { return MathHelper.Lerp(0.0f, 1.0f, MathHelper.Min(1.0f, (float) (_elapsed / 5.0f))); }
		}

		public override void Update(GameTime gameTime)
		{
			_elapsed += gameTime.ElapsedGameTime.TotalSeconds;
		}
	}
}