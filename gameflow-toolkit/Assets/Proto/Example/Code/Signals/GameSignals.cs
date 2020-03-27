using Proto;

namespace Proto.Example
{
	public static class GameSignals
	{
		// "global" "game-wide" signals
		//------------------------------------------------------

		public static Signal BulletHit = new Signal ();
		public static Signal BulletMiss = new Signal ();
	}
}