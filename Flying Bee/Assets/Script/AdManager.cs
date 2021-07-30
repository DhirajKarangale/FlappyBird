using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class ADManager : MonoBehaviour
{
    public static ADManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Advertisement.Initialize("4177737", false);
        if (Random.Range(0, 3) == 1) Invoke("ShowInterstitialAd", 2f);
    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show("Interstitial_Android");
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
}
