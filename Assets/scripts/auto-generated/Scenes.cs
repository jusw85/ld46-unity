//This class is auto-generated do not modify
namespace k
{
	public static class Scenes
	{
		public const string START = "Start";
		public const string DYNAMIC_PLATFORMING = "DynamicPlatforming";
		public const string HUD = "HUD";

		public const int TOTAL_SCENES = 3;

		public static int nextSceneIndex()
		{
			var currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
			if( currentSceneIndex + 1 == TOTAL_SCENES )
				return 0;
			return currentSceneIndex + 1;
		}
	}
}