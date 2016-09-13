using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD48_23
{
	public class ChatComponent : DrawableGameComponent
	{
		private readonly BillGame _game;
		private readonly InputComponent _inputComponent;

		public List<ChatLine> _lines;
		public int _currentLine;

		private SpriteFont _font;

		private TimeSpan _lastPress;
		public bool CanAdvance;

		public ChatComponent(BillGame game, InputComponent inputComponent) : base(game)
		{
			CanAdvance = true;

			_game = game;
			_inputComponent = inputComponent;
			string[] textLines = File.ReadAllLines("script.txt");
			_lines = new List<ChatLine>();
			foreach (string textLine in textLines)
			{
				if (!String.IsNullOrWhiteSpace(textLine))
				{
					int commaIndex = textLine.IndexOf(',');
					string who = textLine.Substring(0, commaIndex);
					string what = textLine.Substring(commaIndex + 1).Trim('"').Replace('|', '\n');

					_lines.Add(new ChatLine {Who = who, What = what});
				}
			}
		}

		public bool Completed { get; set; }

		public SpriteFont Font
		{
			get { return _font; }
		}

		public Texture2D Dot { get; set; }

		protected override void LoadContent()
		{
			_font = Game.Content.Load<SpriteFont>("Fonts/Normal");
			Dot = Game.Content.Load<Texture2D>("Textures/dot");
		}

		public override void Update(GameTime gameTime)
		{
			if (Visible)
			{
				if (_inputComponent.Jump && (gameTime.TotalGameTime - _lastPress).TotalSeconds > 0.15 && CanAdvance)
				{
					_currentLine++;
					_lastPress = gameTime.TotalGameTime;
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (_currentLine >= _lines.Count)
			{
				Enabled = false;
				Visible = false;
				Completed = true;
				return;
			}

			bool bill = _lines[_currentLine].Who.Contains("William");

			Rectangle face = bill ? _game.Atlas.BillFace : _game.Atlas.EvilFace;
			Color color = bill ? Color.Green : Color.Red;

			SpriteBatch sb = _game.SharedSpriteBatch;

			sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default,
			         RasterizerState.CullNone);

			sb.Draw(Dot, new Rectangle(30, 30, Game.GraphicsDevice.Viewport.Width - 60, 160), null, Color.Black * 0.85f);
			sb.DrawString(Font, _lines[_currentLine].Who, new Vector2(41 + 140, 41), Color.Yellow);
			sb.DrawString(Font, _lines[_currentLine].What, new Vector2(40 + 140, 40 + 30), color);

			sb.Draw(_game.Atlas.Texture, new Vector2(40, 40), face, Color.White, 0.0f, Vector2.Zero, 4.0f, SpriteEffects.None,
			        0.0f);
			sb.End();
		}

		#region Nested type: ChatLine

		public class ChatLine
		{
			public string Who { get; set; }
			public string What { get; set; }
		}

		#endregion
	}
}