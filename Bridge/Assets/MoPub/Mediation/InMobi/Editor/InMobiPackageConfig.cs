using System.Collections.Generic;

public class InMobiPackageConfig : PackageConfig
{
    public override string Name
    {
        get { return "InMobi"; }
    }

    public override string Version
    {
        get { return /*UNITY_PACKAGE_VERSION*/"1.1.4"; }
    }

    public override Dictionary<Platform, string> NetworkSdkVersions
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, /*ANDROID_SDK_VERSION*/"9.2.1" },
                { Platform.IOS, /*IOS_SDK_VERSION*/"9.2.1" }
            };
        }
    }

    public override Dictionary<Platform, string> AdapterClassNames
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "com.mopub.mobileads.InMobi" },
                { Platform.IOS, "InMobi" }
            };
        }
    }
}
