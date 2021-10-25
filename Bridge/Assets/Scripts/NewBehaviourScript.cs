using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private string AndroidBannerID;
    [SerializeField] private string AndroidInterstitialID;
    [SerializeField] private string AndroidVideoID;

    private string[] _interstitialAdUnits;
    private string[] _bannerAdUnits;
    private string[] _rewardedVideoAdUnits;

    void Start()
    {
        _rewardedVideoAdUnits = new string[] { AndroidVideoID };
        MoPub.LoadRewardedVideoPluginsForAdUnits(_rewardedVideoAdUnits);
        MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
        MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;

        _interstitialAdUnits = new string[] { AndroidInterstitialID };
        MoPub.LoadInterstitialPluginsForAdUnits(_interstitialAdUnits);
        MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;

        _bannerAdUnits = new string[] { AndroidBannerID };
        MoPub.LoadBannerPluginsForAdUnits(_bannerAdUnits);
        
    }

    public void LoadInterstitial()
    {
        
       
        MoPub.RequestInterstitialAd(AndroidInterstitialID);
    }

    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        MoPub.ShowInterstitialAd(AndroidInterstitialID);
        // The ad has been loaded 
    }

    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        MoPub.DestroyInterstitialAd(AndroidInterstitialID);
    }

    public void LoadRewarded()
    {
        MoPub.RequestRewardedVideo(AndroidVideoID);
    }
   
    private void OnRewardedVideoLoadedEvent(string adUnitId)
    {
        MoPub.ShowRewardedVideo(AndroidVideoID);
        // Rewarded ad cached
    }

    private void OnRewardedVideoClosedEvent(string adUnitId)
    {
        
    }
   
    public void ShowBanner()
    {
        MoPub.RequestBanner(AndroidBannerID, MoPubBase.AdPosition.BottomCenter, MoPubBase.MaxAdSize.Width320Height50);
        MoPub.ShowBanner(AndroidBannerID, true);
    }
}
