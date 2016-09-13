using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	public static class AmmoFactory
	{
		private static readonly List<Func<Vector3, PickableEntity>> Functions = new List<Func<Vector3, PickableEntity>>();

		static AmmoFactory()
		{
			Functions.Add(v => new BlasterAmmo(v));
			Functions.Add(v => new LauncherAmmo(v));
		}

		public static PickableEntity Next(Vector3 position)
		{
			return Functions[BillGame.Random.Next(Functions.Count)](position);
		}
	}
}