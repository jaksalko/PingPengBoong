#if mopub_manager
using UnityEngine;

public class PangleNetworkConfig : MoPubNetworkConfig
{
    public override string AdapterConfigurationClassName
    {
        get { return Application.platform == RuntimePlatform.Android
                  ? "com.mopub.mobileads.PangleAdapterConfiguration"
                  : "PangleAdapterConfiguration"; }
    }

    [Tooltip("Enter your app ID to be used to initialize the Pangle SDK.")]
    [Config.Optional]
    public PlatformSpecificString app_id;
}
#endif
