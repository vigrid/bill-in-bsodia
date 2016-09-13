namespace LD48_23
{
	public abstract class Projectile : Entity
	{
		public override float Size
		{
			get { return 0.2f; }
		}

		protected bool CollideProjectile(VoxelWorld world, float time)
		{
			CollideResult collision = Collide(world, time);
			if (collision != CollideResult.Air)
			{
				Destroyed = true;
				return true;
			}
			return false;
		}
	}
}