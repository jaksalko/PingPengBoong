#if mopub_manager
using UnityEngine;

public class InMobiNetworkConfig : MoPubNetworkConfig
{
    public override string AdapterConfigurationClassName
    {
        get { return Application.platform == RuntimePlatform.Android
                  ? "com.mopub.mobileads.InMobiAdapterConfiguration"
                  : "InMobiAdapterConfiguration"; }
    }

    [Tooltip("Enter your InMobi Account Id to be used to initialize the InMobi SDK.")]
    [Config.Optional]
    public PlatformSpecificString accountid;

}
#endif
