namespace LD48_23
{
#if WINDOWS || XBOX
	internal static class Program
	{
		private static void Main()
		{
			using (var game = new BillGame())
			{
				game.Run();
			}
		}
	}
#endif
}