using System;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class BeamEntity : Entity
	{
		public static readonly EntityDrawInfo _drawInfo = new EntityDrawInfo
		{
			Rectangle = new Rectangle(104, 24, 8, 16),
			Origin = new Vector2(4, 16)
		};

		public BeamEntity(Vector3 position)
		{
			Position = position;
			BillGame.Instance.PlaySound("Sounds/Teleport");
		}

		public Vector2[] _offsets = new[]
		                            	{
		                            		new Vector2(0, 0),
		                            		new Vector2(8, 0),
		                            		new Vector2(16, 0),
		                            		new Vector2(0, 16),
		                            		new Vector2(8, 16),
		                            		new Vector2(16, 16),
		                            	};

		public float _elapsedTime;

		public const float TotalTime = 2.0f;

		public override float Size
		{
			get { return 0.0f; }
		}

		public override EntityDrawInfo DrawInfo
		{
			get
			{
				var currentPhase = (int)(_elapsedTime / TotalTime * 6);
				currentPhase = Math.Min(currentPhase, 5);
				var di = _drawInfo;
				di.Rectangle.X += (int)_offsets[currentPhase].X;
				di.Rectangle.Y += (int)_offsets[currentPhase].Y;
				di.CustomAlpha = (float)Math.Sin(_elapsedTime * 15.0f) * 0.25f + MathHelper.Lerp(0.25f, 0.75f, _elapsedTime / TotalTime);
				di.CustomScale = 1.0f + di.CustomAlpha * 0.25f;
				return di;
			}
		}

		public override bool HasShadow
		{
			get { return false; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			_elapsedTime += time;
		}
	}
}
