using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class IntroPlayerComponent : DrawableGameComponent
	{
		private readonly BillGame _game;

		private readonly InputComponent _inputComponent;

		private Texture2D _dot;

		public IntroPlayerComponent(BillGame game, InputComponent inputComponent) : base(game)
		{
			_game = game;
			_inputComponent = inputComponent;
		}

		public Texture2D DotTexture
		{
			get { return _dot; }
		}

		public CoreAtlas Atlas { get; set; }

		protected override void LoadContent()
		{
			_dot = Game.Content.Load<Texture2D>("Textures/dot");

			Atlas = (CoreAtlas) Game.Components.Single(gc => gc is CoreAtlas);
		}

		public override void Update(GameTime gameTime)
		{
			if (!_game.Controllable)
			{
				return;
			}

			var time = (float) gameTime.ElapsedGameTime.TotalSeconds;

			float x = 0.0f;
			float y = 0.0f;
			float z = 0.0f;

			x += _inputComponent.Right ? 1.0f : 0.0f;
			x += _inputComponent.Left ? -1.0f : 0.0f;
			y += _inputComponent.Up ? -1.0f : 0.0f;
			y += _inputComponent.Down ? 1.0f : 0.0f;

			const float speed = 600.0f;

			Vector3 acceleration = new Vector3(x, y, z) * speed;

			//foreach (Player player in _entities.OfType<Player>())
			//{
			//   player.Accelerate(acceleration);
			//   if (_inputComponent.Jump)
			//   {
			//      player.Jump();
			//   }

			//   if (_inputComponent.Weapon1)
			//   {
			//      player.ActiveWeapon = 0;
			//   }
			//   if (_inputComponent.Weapon2)
			//   {
			//      player.ActiveWeapon = 1;
			//   }
			//   if (_inputComponent.Weapon3)
			//   {
			//      player.ActiveWeapon = 2;
			//   }
			//   if (_inputComponent.Weapon4)
			//   {
			//      player.ActiveWeapon = 3;
			//   }

			//   _game.Message = player.Position.Z.ToString();
			//}

			//foreach (Smoke entity in _entities)
			//{
			//   entity.AddWaitingEntities(time);
			//}
		}

		public override void Draw(GameTime gameTime)
		{
			//if (!_game.Controllable)
			//{
			//   return;
			//}

			//_game.SharedSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
			//                              DepthStencilState.Default, RasterizerState.CullNone);

			//foreach (Smoke entity in _entities)
			//{
			//   entity.Draw(_game.SharedSpriteBatch);
			//}

			//_game.SharedSpriteBatch.End();
		}
	}
}