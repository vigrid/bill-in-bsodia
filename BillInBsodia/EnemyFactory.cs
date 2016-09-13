using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	public static class EnemyFactory
	{
		private static readonly List<Func<Vector3, Mob>> Functions = new List<Func<Vector3, Mob>>();

		static EnemyFactory()
		{
			Functions.Add(p => new FeatureBug(p));
			Functions.Add(p => new FeatureBug(p));
			Functions.Add(p => new FeatureBug(p));
			Functions.Add(p => new FeatureBug(p));
			Functions.Add(p => new IterationBug(p));
			Functions.Add(p => new IterationBug(p));
			Functions.Add(p => new IterationBug(p));
			Functions.Add(p => new MilestoneBug(p));
			Functions.Add(p => new MilestoneBug(p));
			Functions.Add(p => new ProjectBug(p));
		}

		public static Mob Next(Vector3 position)
		{
			return Functions[BillGame.Random.Next(Functions.Count)](position);
		}
	}
}