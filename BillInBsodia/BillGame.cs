using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class BillGame : Game
	{
		public static readonly Random Random = new Random();

		#region Game Settings

		public const float MaxHeight = 5.0f;

		#endregion

		private readonly GraphicsDeviceManager _graphics;

		private readonly InputComponent _inputComponent;

		private readonly HudComponent _hudComponent;
		private readonly FlashComponent _flashComponent;
		private readonly IntroComponent _introComponent;
		private readonly IntroPlayerComponent _introPlayerComponent;
		private readonly MouseCursorComponent _mouseCursorComponent;
		private readonly ChatComponent _chatComponent;
		private readonly WorldComponent _worldComponent;
		private readonly Dictionary<string, SoundEffect> _sounds = new Dictionary<string, SoundEffect>();

		private SpriteBatch _sharedSpriteBatch;
		private readonly GameOverComponent _gameOverComponent;
		private Won _wonComponent;

		public BillGame()
		{
			Instance = this;

			_graphics = new GraphicsDeviceManager(this);
			_graphics.PreferredBackBufferWidth = 1280;
			_graphics.PreferredBackBufferHeight = 720;
			_graphics.SynchronizeWithVerticalRetrace = false;
			_graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

			IsFixedTimeStep = false;

			Content.RootDirectory = "Content";

			Atlas = new CoreAtlas(this);

			_inputComponent = new InputComponent(this);
			_introComponent = new IntroComponent(this, InputComponent);
			_introPlayerComponent = new IntroPlayerComponent(this, InputComponent);
			_mouseCursorComponent = new MouseCursorComponent(this);
			_chatComponent = new ChatComponent(this, InputComponent);
			_worldComponent = new WorldComponent(this);
			_hudComponent = new HudComponent(this);
			_flashComponent = new FlashComponent(this);
			_gameOverComponent = new GameOverComponent(this);
			_wonComponent = new Won(this);

			Components.Add(InputComponent);
			Components.Add(Atlas); // wtf is this a component??
			Components.Add(IntroComponent);
			Components.Add(IntroPlayerComponent);
			Components.Add(WorldComponent);
			Components.Add(MouseCursorComponent);
			Components.Add(ChatComponent);
			Components.Add(HudComponent);
			Components.Add(FlashComponent);

			Components.Add(GameOverComponent);
			Components.Add(WonComponent);

			GameOverComponent.Visible = false;
			GameOverComponent.Enabled = false;
			WonComponent.Visible = false;
			WonComponent.Enabled = false;
		}

		public static BillGame Instance { get; private set; }

		public ChatComponent ChatComponent
		{
			get { return _chatComponent; }
		}

		public SpriteBatch SharedSpriteBatch
		{
			get { return _sharedSpriteBatch; }
		}

		public CoreAtlas Atlas { get; set; }

		public bool Controllable { get; set; }
		public string Message { get; set; }

		public IntroComponent IntroComponent
		{
			get { return _introComponent; }
		}

		public IntroPlayerComponent IntroPlayerComponent
		{
			get { return _introPlayerComponent; }
		}

		public MouseCursorComponent MouseCursorComponent
		{
			get { return _mouseCursorComponent; }
		}

		public WorldComponent WorldComponent
		{
			get { return _worldComponent; }
		}

		public InputComponent InputComponent
		{
			get { return _inputComponent; }
		}

		public HudComponent HudComponent
		{
			get { return _hudComponent; }
		}

		public FlashComponent FlashComponent
		{
			get { return _flashComponent; }
		}

		public GameOverComponent GameOverComponent
		{
			get { return _gameOverComponent; }
		}

		public Won WonComponent
		{
			get { return _wonComponent; }
		}

		protected override void LoadContent()
		{
			_sharedSpriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(IntroComponent.BsodColor);


			base.Draw(gameTime);
		}

		protected override void Update(GameTime gameTime)
		{
			Window.Title = Message ?? String.Empty;

			base.Update(gameTime);
		}

		public void PlaySound(string soundAsset)
		{
			SoundEffect soundEffect;
			if (!_sounds.TryGetValue(soundAsset, out soundEffect))
			{
				soundEffect = Content.Load<SoundEffect>(soundAsset);
			}

			soundEffect.Play();
		}
	}
}