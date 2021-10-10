## MoPub Unity Sample App

The MoPub Unity sample app is the `MoPubSample` scene in this directory; the previous app has been moved into a `Legacy/` directory in the same location and can still be used as well.

The scene can be ran within the Unity Editor with simulated ad lifecycle events, or deployed to an Android or iOS device to run with real ads.

![sample app demo](https://github.com/mopub/mopub-unity-sdk/blob/master/mopub-unity-sample.gif?raw=true)

### Sample App Troubleshooting

The MoPub Unity sample app is for demonstration, testing, and troubleshooting purposes only. If it causes any compilation issues,
the `unity-sample-app/Assets/MoPub/Sample/` directory can be safely removed, or the following can be done to address them:
* Ensuring TextMesh Pro 2.1.6 is installed (via the Unity Package Manager)
* Ensuring `DELIGHT_MODULE_TEXTMESHPRO` is present in the Scripting Define Symbols (via Project Settings > Player > Other Settings)
* Removing `TextMeshPro` as a Module from `unity-sample-app/Assets/MoPub/Sample/Content/Config.txt`, letting Unity compile, then re-adding it
* Ensuring `UI/Default` is listed under `Always Included Shaders` (via Project Settings > Graphics > Built-in Shader Settings)

If none of these address the problem, the `MoPubSample` scene and `AppIcons`, `Content` and `Delight` directories under `unity-sample-app/Assets/MoPub/Sample` can be safely removed to unblock using the legacy sample app.
