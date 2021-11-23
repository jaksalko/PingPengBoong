using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Constants
{
    public const int mapHeight = 3;
    public const int WebRequestTryCount = 5;
    public const float WebRequestDelayTime = 1.0f;

    // Parse
    public const string ExceptionStrKey = "-1";
    public const int ExceptionIndex = -1;

    public const string StarGuidePath = "Sprite/StarGuide/";
    public const string SkinStorePath = "Sprite/Store/Skin/";
    public const string RoomDecoPath = "Sprite/RoomResource/";

    public const int StartRewardAdsTime = 300;

    public static string[] FULL_TEST = {
       "24534e1901884e398f1253216226017e"
    };

    public static string[] BANNER_TEST = {
        "b195f8dd8ded45fe847ad89ed1d016da"
    };

    public static string[] REWARD_TEST = new string[4]{
        "980016716",
        "920b6145fb1546cf8b5cf2ac34638bb7",
        //"1024389864990132_1037587783670340",
        "ANDROID_REWARD-8434429",
        "Rewarded_Android"
        //"ca-app-pub-8344778654571559/2905267968"
    };

    public static string[] FULL_ANDROID = new string[4]{
        "980016719",
        //"1024389864990132_1025263631569422",
        "FULL_AD-3358998",
        "f3ac2fe0759b4c8a9e6e6ded7a1d97a6",
        "Interstitial_iOS"
        //"ca-app-pub-8344778654571559/9493715282"
    };

    public static string[] BANNER_ANDROID = new string[4] {
        "0b8cce9a441b4f359004cab5ab543b2a",
        "980016712",
        //"1024389864990132_1025258774903241",
        //"ca-app-pub-8344778654571559/4432960296",
        "Banner_Android",
        "UPPER_BANNER-6829153"
    };

    public static string[] REWARD_ANDROID = new string[4] {
        "980016716",
        "b769335153204077b2b567936aa46f84",
        //"1024389864990132_1037587783670340",
        "ANDROID_REWARD-8434429",
        "Rewarded_Android"
        //"ca-app-pub-8344778654571559/2905267968"
    };


    public static string[] FULL_IOS = new string[4]{
        "980016718",
        //"1024389864990132_1025276584901460",
        "FULL_AD-9010350",
        "e95cbcc0313047ffbcf76660c85b19d4",
        "Interstitial_iOS"
        //"ca-app-pub-8344778654571559/7966022957"
    };

    public static string[] BANNER_IOS = new string[4]{
        "e9b1eb427c744ed8ab0870ba63d24a9f",
        "980019963",
        //"1024389864990132_1025276438234808",
        //"ca-app-pub-8344778654571559/9279104620",
        "UPPER_BANNER-7536258",
        "Banner_iOS"
    };

    public static string[] REWARD_IOS = new string[4]{
        "980016714",
        "1d25b31d12094a5c87fc1996778e2300",
        //"1024389864990132_1037588897003562",
        "IOS_REWARD-0752454",
        "Rewarded_iOS"
        //"ca-app-pub-8344778654571559/6652941280"
    };

    public static string GoogleAdsIOSBannerAdId = "ca-app-pub-8344778654571559/9279104620";
    public static string GoogleAdsAndroidBannerAdId = "ca-app-pub-8344778654571559/4432960296";

    public static string GoogleAdsIOSInterstitialAdId = "ca-app-pub-8344778654571559/7966022957";
    public static string GoogleAdsAndroidInterstitialAdId = "ca-app-pub-8344778654571559/9493715282";

    public static string GoogleAdsIOSRewardAdId = "ca-app-pub-8344778654571559/6652941280";
    public static string GoogleAdsAndroidRewardAdId = "ca-app-pub-8344778654571559/2905267968";

    public static string GoogleAdsAndroidTestBannerId = "ca-app-pub-3940256099942544/6300978111";
    public static string GoogleAdsAndroidTestInterstitialAdId = "ca-app-pub-3940256099942544/1033173712";

    public static string GoogleAdsIOSTestBannerId = "ca-app-pub-3940256099942544/2934735716";
    public static string GoogleAdsIOSTestInterstitialAdId = "ca-app-pub-3940256099942544/4411468910";
}
