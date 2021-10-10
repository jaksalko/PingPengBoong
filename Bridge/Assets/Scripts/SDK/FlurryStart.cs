using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using FlurrySDK;

public class FlurryStart : MonoBehaviour
{
#if UNITY_ANDROID
    private readonly string FLURRY_API_KEY = "3NKZ6ZVRS8F8WRD66WPR";
#elif UNITY_IPHONE
    private readonly string FLURRY_API_KEY = "Y95JSRGFK7D2CCFW4N4Z";
#else
    private readonly string FLURRY_API_KEY = null;
#endif


    void Start()
    {
        // Note: When enabling Messaging, Flurry Android should be initialized by using AndroidManifest.xml.
        // Initialize Flurry once.
        new Flurry.Builder()
                  .WithCrashReporting(true)
                  .WithLogEnabled(true)
                  .WithLogLevel(Flurry.LogLevel.VERBOSE)
                  .WithMessaging(true)
                  .Build(FLURRY_API_KEY);

        // Example to get Flurry versions and publisher segmentation.
        Debug.Log("AgentVersion: " + Flurry.GetAgentVersion());
        Debug.Log("ReleaseVersion: " + Flurry.GetReleaseVersion());
        Flurry.SetPublisherSegmentationListener(new MyPublisherSegmentationListener());
        Flurry.FetchPublisherSegmentation();

        // Set user preferences.
        Flurry.SetAge(36);
        Flurry.SetGender(Flurry.Gender.Female);
        Flurry.SetReportLocation(true);

        // Set user properties.
        Flurry.UserProperties.Set(Flurry.UserProperties.PROPERTY_REGISTERED_USER, "True");

        // Set Messaging listener
        Flurry.SetMessagingListener(new MyMessagingListener());

        // Log Flurry events.
        Flurry.EventRecordStatus status = Flurry.LogEvent("Unity Event");
        Debug.Log("Log Unity Event status: " + status);

        // Log Flurry timed events with parameters.
        IDictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("Author", "Flurry");
        parameters.Add("Status", "Registered");
        status = Flurry.LogEvent("Unity Event Params Timed", parameters, true);
        Debug.Log("Log Unity Event with parameters timed status: " + status);

        Flurry.EndTimedEvent("Unity Event Params Timed");

        // Log Flurry standard events.
        status = Flurry.LogEvent(Flurry.Event.APP_ACTIVATED);
        Debug.Log("Log Unity Standard Event status: " + status);

        Flurry.EventParams stdParams = new Flurry.EventParams()
            .PutDouble(Flurry.EventParam.TOTAL_AMOUNT, 34.99)
            .PutBoolean(Flurry.EventParam.SUCCESS, true)
            .PutString(Flurry.EventParam.ITEM_NAME, "book 1")
            .PutString("note", "This is an awesome book to purchase !!!");
        status = Flurry.LogEvent(Flurry.Event.PURCHASED, stdParams);
        Debug.Log("Log Unity Standard Event with parameters status: " + status);
    }

    
}

public class MyPublisherSegmentationListener : Flurry.IFlurryPublisherSegmentationListener
    {
        public void OnFetched(IDictionary<string, string> data)
        {
            string segments;
            data.TryGetValue("segments", out segments);
            Debug.Log("Flurry Publisher Segmentation Fetched: " + segments);
        }
    }

    public class MyMessagingListener : Flurry.IFlurryMessagingListener
    {
        // If you would like to handle the notification yourself, return true to notify Flurry
        // you've handled it, and Flurry will not show the notification.
        public bool OnNotificationReceived(Flurry.FlurryMessage message)
        {
            Debug.Log("Flurry Messaging Notification Received: " + message.Title);
            return false;
        }

        // If you would like to handle the notification yourself, return true to notify Flurry
        // you've handled it, and Flurry will not launch the app or "click_action" activity.
        public bool OnNotificationClicked(Flurry.FlurryMessage message)
        {
            Debug.Log("Flurry Messaging Notification Clicked: " + message.Title);
            return false;
        }

        public void OnNotificationCancelled(Flurry.FlurryMessage message)
        {
            Debug.Log("Flurry Messaging Notification Cancelled: " + message.Title);
        }

        public void OnTokenRefresh(string token)
        {
            Debug.Log("Flurry Messaging Token Refresh: " + token);
        }

        public void OnNonFlurryNotificationReceived(IDisposable nonFlurryMessage)
        {
            Debug.Log("Flurry Messaging Non-Flurry Notification.");
        }
    }