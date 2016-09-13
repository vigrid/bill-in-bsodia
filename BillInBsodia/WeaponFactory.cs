using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	public static class WeaponFactory
	{
		private static readonly List<Func<Vector3, Weapon>> Functions = new List<Func<Vector3, Weapon>>();

		static WeaponFactory()
		{
			Functions.Add(v => new Blaster(v));
			Functions.Add(v => new AutoBlaster(v));
			Functions.Add(v => new Launcher(v));
			Functions.Add(v => new AutoLauncher(v));
		}

		public static Weapon Next(Vector3 position)
		{
			return Functions[BillGame.Random.Next(Functions.Count)](position);
		}
	}
}