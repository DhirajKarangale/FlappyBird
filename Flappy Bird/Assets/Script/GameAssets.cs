using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public Transform pipe;
}
