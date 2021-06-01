using UnityEngine.SceneManagement;

public static class Loader 
{
   public enum Scene
   {
        Game,
   }

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }
}
