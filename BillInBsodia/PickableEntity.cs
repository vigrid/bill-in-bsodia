using System;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	public abstract class PickableEntity : Entity
	{
		private float _timePassed;

		private Vector3 _originalPosition;

		protected PickableEntity(Vector3 position)
		{
			_timePassed = (float) (BillGame.Random.NextDouble() * Math.PI * 2.0f);
			Position = position;

			_originalPosition = position;
		}

		public override float Size
		{
			get { return 0.25f; }
		}

		public override bool HasShadow
		{
			get { return true; }
		}

		public override void Update(VoxelWorld world, float time)
		{
			_timePassed += time;

			var z = (float) (Math.Sin(_timePassed * 5.0f) * 0.15f);

			Position.Z = z + _originalPosition.Z;
		}

		public abstract void Pick(Player player);
	}
}