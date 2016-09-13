using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace LD48_23
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Voxel
	{
		public CubeType Type;
		public Color Color;
	}
}