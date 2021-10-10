using System.Collections.Generic;

public class ChartboostPackageConfig : PackageConfig
{
    public override string Name
    {
        get { return "Chartboost"; }
    }

    public override string Version
    {
        get { return /*UNITY_PACKAGE_VERSION*/"1.2.13"; }
    }

    public override Dictionary<Platform, string> NetworkSdkVersions
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, /*ANDROID_SDK_VERSION*/"8.2.1" },
                { Platform.IOS, /*IOS_SDK_VERSION*/"8.4.2" }
            };
        }
    }

    public override Dictionary<Platform, string> AdapterClassNames
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "com.mopub.mobileads.Chartboost" },
                { Platform.IOS, "Chartboost" }
            };
        }
    }
}
