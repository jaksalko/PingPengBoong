#if mopub_manager
using UnityEngine;

public class ChartboostNetworkConfig : MoPubNetworkConfig
{
    public override string AdapterConfigurationClassName
    {
        get { return Application.platform == RuntimePlatform.Android
                  ? "com.mopub.mobileads.ChartboostAdapterConfiguration"
                  : "ChartboostAdapterConfiguration"; }
    }

    public override string MediationSettingsClassName
    {
        get { return Application.platform == RuntimePlatform.Android
                  ? "com.mopub.mobileads.ChartboostRewardedVideo$ChartboostMediationSettings"
                  : null; }
    }

    [Tooltip("Enter your app ID to be used to initialize the Chartboost SDK.")]
    [Config.Optional]
    public PlatformSpecificString appId;

    [Tooltip("Enter your app signature to be used to initialize the Chartboost SDK.")]
    [Config.Optional]
    public PlatformSpecificString appSignature;
}
#endif
