using System.Collections.Generic;

public class PanglePackageConfig : PackageConfig
{
    public override string Name
    {
        get { return "Pangle"; }
    }

    public override string Version
    {
        get { return /*UNITY_PACKAGE_VERSION*/"1.2.11"; }
    }

    public override Dictionary<Platform, string> NetworkSdkVersions
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, /*ANDROID_SDK_VERSION*/"3.9.0.5" },
                { Platform.IOS, /*IOS_SDK_VERSION*/"3.9.0.4" }
            };
        }
    }

    public override Dictionary<Platform, string> AdapterClassNames
    {
        get {
            return new Dictionary<Platform, string> {
                { Platform.ANDROID, "com.mopub.mobileads.Pangle" },
                { Platform.IOS, "Pangle" }
            };
        }
    }
}
