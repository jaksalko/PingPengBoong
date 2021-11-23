using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;
public class GoogleAdsManager : MonoBehaviour
{
    public static GoogleAdsManager instance = null;//terrible singleton pattern
    public int interstitialAdTimeLimit = 100;
    public bool isActivateInterstitialAd = false;
    Coroutine interstitialAdTimerCoroutine = null;
    public BannerView bannerView;
    public InterstitialAd interstitialAd;
    public RewardedAd rewardedAd;

    public bool canRewarded = false;
    public int rewardAdsTime = Constants.StartRewardAdsTime;

    public UnityEvent RewardEvent;
    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("instance is null");
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        /*
        List<string> deviceIds = new List<string>();
        deviceIds.Add("f8cc5f8f2452547c8517355a1b0d15a7");
        RequestConfiguration requestConfiguration = new RequestConfiguration
            .Builder()
            .SetTestDeviceIds(deviceIds)
            .build();
        */
        /*
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetSameAppKeyEnabled(true).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);
        
        MobileAds.Initialize(HandleInitCompleteAction);
        interstitialAdTimerCoroutine = StartCoroutine(InterstitialAdTimer());
        */
        //StartCoroutine(StartTimer());
    }

    void StartInterstitialAdTimer()
    {
        if (interstitialAdTimerCoroutine != null)
            StopCoroutine(interstitialAdTimerCoroutine);

        interstitialAdTimerCoroutine = StartCoroutine(InterstitialAdTimer());
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            Debug.Log("Initialization complete");
            RequestBannerAd();
            RequestRewardAds();
        });
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;

        Debug.Log(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);


    }

    void RequestRewardAds()
    {
#if UNITY_EDITOR
        Debug.Log("Editor not show reward ads");
        rewardedAd = new RewardedAd(Constants.GoogleAdsAndroidRewardAdId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.rewardedAd.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.rewardedAd.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.rewardedAd.OnAdClosed += this.HandleOnAdClosed;
        this.rewardedAd.OnUserEarnedReward += (sender, args) => RewardEvent.Invoke();
#elif UNITY_ANDROID
// Clean up banner before reusing
        rewardedAd = new RewardedAd(Constants.GoogleAdsAndroidRewardAdId);
        
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.rewardedAd.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.rewardedAd.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.rewardedAd.OnAdClosed += this.HandleOnAdClosed;
        this.rewardedAd.OnUserEarnedReward += (sender, args) => RewardEvent.Invoke();
#else
// Clean up banner before reusing
        rewardedAd = new RewardedAd(Constants.GoogleAdsIOSRewardAdId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.rewardedAd.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.rewardedAd.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.rewardedAd.OnAdClosed += this.HandleOnAdClosed;
        this.rewardedAd.OnUserEarnedReward += (sender, args) => RewardEvent.Invoke();
#endif
    }

    public void RequestBannerAd()
    {
#if UNITY_EDITOR
        Debug.Log("Editor not show banner");

#elif UNITY_ANDROID
// Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
        bannerView = new BannerView(Constants.GoogleAdsAndroidBannerAdId, AdSize.Banner, AdPosition.Top);
        //bannerView = new BannerView(Constants.GoogleAdsAndroidTestBannerId, AdSize.Banner, AdPosition.Top);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
#else
// Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
        //bannerView = new BannerView(Constants.GoogleAdsIOSTestBannerId, AdSize.Banner, AdPosition.Top);
        bannerView = new BannerView(Constants.GoogleAdsIOSBannerAdId, AdSize.Banner, AdPosition.Top);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
#endif


    }


    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: "
                            + args.LoadAdError);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosed event received");
    }

    

    void RequestInterstitialAd()
    {
        
#if UNITY_EDITOR
        Debug.Log("Editor not show interstitialAd");

#elif UNITY_ANDROID
        interstitialAd = new InterstitialAd(Constants.GoogleAdsAndroidInterstitialAdId);

        //interstitialAd = new InterstitialAd(Constants.GoogleAdsAndroidTestInterstitialAdId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialAd.LoadAd(request);
        Debug.Log("Load Interstitial Ad");
        
        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += HandleOnAdClosed;
#else
        //interstitialAd = new InterstitialAd(Constants.GoogleAdsIOSTestInterstitialAdId);
        interstitialAd = new InterstitialAd(Constants.GoogleAdsIOSInterstitialAdId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitialAd.LoadAd(request);
        Debug.Log("Load Interstitial Ad");

        // Called when an ad request has successfully loaded.
        this.interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitialAd.OnAdClosed += HandleOnAdClosed;
#endif
    }

    public void ShowInterstitialAd()
    {
        if(interstitialAd != null && interstitialAd.IsLoaded())
        {
            isActivateInterstitialAd = false;
            StartInterstitialAdTimer();
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("interstitial ad not loaded");
        }
    }

    public void ShowRewardAd()
    {
        if(rewardedAd != null && rewardedAd.IsLoaded())
        {
            rewardedAd.Show();
            RequestRewardAds();
        }
        else
        {
            RequestRewardAds();
        }
    }

    IEnumerator InterstitialAdTimer()
    {
        var waitTime = new WaitForSeconds(1f);
        float Timer = 0;
        while(Timer <= interstitialAdTimeLimit)
        {
            
            Timer += 1;
            Debug.Log(Timer);
            yield return waitTime;
        }
        Debug.Log("Activate interstitial ad");
        RequestInterstitialAd();
        isActivateInterstitialAd = true;
        interstitialAdTimerCoroutine = null;
        yield break;
    }

    IEnumerator StartTimer()
    {
        var waitForSeconds = new WaitForSeconds(1f);
        while (true)
        {
            if(rewardAdsTime == 0)
            {
                //Do nothing
                canRewarded = true;
            }
            else
            {
                rewardAdsTime--;
            }

            yield return waitForSeconds;
        }
    }

}
