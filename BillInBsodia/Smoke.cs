using Microsoft.Xna.Framework;

namespace LD48_23
{
	public class Smoke : Entity
	{
		public virtual float MaxLife
		{
			get { return 2.0f; }
		}

		public const float MaxLifeDev = 0.5f;

		public static readonly EntityDrawInfo _template = new EntityDrawInfo
		                                                  	{
		                                                  		Rectangle = new Rectangle(0, 80, 8, 8),
		                                                  		Origin = new Vector2(4, 4)
		                                                  	};

		private readonly float _maxLife;
		public Vector3 _velocity;
		protected float _lived;

		public Smoke(Vector3 position)
		{
			_maxLife = (float) (BillGame.Random.NextDouble() * MaxLifeDev * 2.0 - MaxLifeDev + MaxLife);
			Position = position;
			_velocity = BillGame.Random.GetVector3() * 0.35f + Vector3.UnitZ * 0.7f;
		}

		public float LifePercentage
		{
			get { return _lived / _maxLife; }
		}

		public override float Size
		{
			get { return MathHelper.Lerp(0.25f, 1.0f, LifePercentage); }
		}

		public override EntityDrawInfo DrawInfo
		{
			get
			{
				EntityDrawInfo copy = _template;
				copy.CustomScale = 1.0f + LifePercentage * 1.5f;
				copy.CustomAlpha = 1.0f - LifePercentage;

				if (LifePercentage < 0.25f)
				{
					return copy;
				}
				if (LifePercentage < 0.5f)
				{
					copy.Rectangle.X += 8;
					return copy;
				}
				if (LifePercentage < 0.75f)
				{
					copy.Rectangle.X += 16;
					return copy;
				}

				copy.Rectangle.X += 24;
				return copy;
			}
		}

		public override bool HasShadow
		{
			get { return true; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			_lived += time;
			if (LifePercentage > 1.0f)
			{
				Destroyed = true;
				return;
			}

			Position += _velocity * time;
		}
	}
}