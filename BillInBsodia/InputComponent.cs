using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LD48_23
{
	public class InputComponent : GameComponent
	{
		private readonly BillGame _game;

		public InputComponent(BillGame game) : base(game)
		{
			_game = game;
		}

		public bool Debug { get; set; }

		public bool Up { get; set; }
		public bool Down { get; set; }
		public bool Left { get; set; }
		public bool Right { get; set; }
		public bool Shoot { get; set; }
		public bool Exit { get; set; }
		public bool Jump { get; set; }

		public bool Teleport { get; set; }

		public bool Weapon1 { get; set; }
		public bool Weapon2 { get; set; }
		public bool Weapon3 { get; set; }
		public bool Weapon4 { get; set; }

		public Vector2 MousePosition { get; set; }

		public override void Update(GameTime gameTime)
		{
			KeyboardState keyboard = Keyboard.GetState();
			MouseState mouse = Mouse.GetState();

			MousePosition = new Vector2(mouse.X, mouse.Y);

			Exit = keyboard.IsKeyDown(Keys.Escape);

			if (_game.Controllable)
			{
				Up = keyboard.IsKeyDown(Keys.W);
				Down = keyboard.IsKeyDown(Keys.S);
				Left = keyboard.IsKeyDown(Keys.A);
				Right = keyboard.IsKeyDown(Keys.D);
				Shoot = mouse.LeftButton == ButtonState.Pressed;
				Jump = keyboard.IsKeyDown(Keys.Space);
				Weapon1 = keyboard.IsKeyDown(Keys.D1);
				Weapon2 = keyboard.IsKeyDown(Keys.D2);
				Weapon3 = keyboard.IsKeyDown(Keys.D3);
				Weapon4 = keyboard.IsKeyDown(Keys.D4);

				Teleport = keyboard.IsKeyDown(Keys.T);

//#if DEBUG
//            if (keyboard.IsKeyDown(Keys.G)) // GODMODE!
//            {
//               Player bill = _game.WorldComponent.Bill;
//               bill.HasAutoBlaster = true;
//               bill.HasBlaster = true;
//               bill.HasLauncher = true;
//               bill.HasDoubleLauncher = true;
//               bill.BlasterAmmo = 1000;
//               bill.LauncherAmmo = 1000;
//               bill.Position.X = 162;
//               bill.Position.Y = 206;
//            }
//#endif
			}

			//Debug = keyboard.IsKeyDown(Keys.F1);
		}
	}
}