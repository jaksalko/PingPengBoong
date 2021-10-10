using System.Collections.Generic;

public class VunglePackageConfig : PackageConfig
{
    public override string Name
    {
        get { return "Vungle"; }
    }

    public override string Version
    {
        get { return /*UNITY_PACKAGE_VERSION*/"1.3.16"; }
    }

    public override Dictionary<Platform, string> NetworkSdkVersions
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, /*ANDROID_SDK_VERSION*/"6.10.2" },
                { Platform.IOS, /*IOS_SDK_VERSION*/"6.10.3" }
            };
        }
    }

    public override Dictionary<Platform, string> AdapterClassNames
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "com.mopub.mobileads.Vungle" },
                { Platform.IOS, "Vungle" }
            };
        }
    }
}
