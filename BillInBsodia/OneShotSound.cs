using Microsoft.Xna.Framework.Audio;

namespace LD48_23
{
	public class OneShotSound
	{
		private readonly string _soundPath;
		private bool _played;

		private SoundEffect _soundEffect;

		public OneShotSound(string soundPath)
		{
			_soundPath = soundPath;
		}

		public void Play(BillGame game)
		{
			Play(game, null);
		}

		public void Reset()
		{
			_played = false;
		}

		public void Play(BillGame game, float? pitch)
		{
			if (_played)
			{
				return;
			}

			_played = true;

			if (_soundEffect == null)
			{
				_soundEffect = game.Content.Load<SoundEffect>(_soundPath);
			}

			if (pitch.HasValue)
			{
				_soundEffect.Play(1.0f, pitch.Value, 0.0f);
			}
			else
			{
				_soundEffect.Play();
			}
		}
	}
}