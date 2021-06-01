using UnityEngine;
using System;

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

   
   
}
