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
        Advertisement.Initialize("4177737", true);
        ShowBannerAd();
        if (Random.Range(0, 3) == 2) Invoke("ShowInterstitialAd", 2f);
    }

    private void OnDestroy()
    {
        Advertisement.Banner.Hide();
    }

    private void ShowBannerAd()
    {
        if (Advertisement.IsReady("Banner_Android"))
        {
            Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
            Advertisement.Banner.Show("Banner_Android");
        }
        else
        {
            StartCoroutine(RepeatShowBanner());
        }
    }

    IEnumerator RepeatShowBanner()
    {
        yield return new WaitForSeconds(1);
        ShowBannerAd();
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
