using Microsoft.Xna.Framework;

namespace LD48_23
{
	public abstract class Entity
	{
		public Vector3 Position; // fuck you!

		protected Entity()
		{
			Active = true;
		}

		public abstract float Size { get; }
		public bool Destroyed { get; set; }

		public abstract EntityDrawInfo DrawInfo { get; }

		public bool Active { get; set; }
		public abstract bool HasShadow { get; }
		public abstract void Update(VoxelWorld world, float time);

		public virtual CollideResult Collide(VoxelWorld world, float time)
		{
			Voxel voxelUnderBill;
			var x = (int) Position.X;
			var y = (int) Position.Y;

			float height = world[x, y].Type != CubeType.Empty ? 1.0f : 0.0f;
			if (height > Position.Z)
			{
				var newPos = new Vector3(Position.X, Position.Y, MathHelper.Max(Position.Z, height));
				Position = newPos;
				return CollideResult.Collided;
			}
			if (height == Position.Z)
			{
				return CollideResult.Standing;
			}

			return CollideResult.Air;
		}
	}
}