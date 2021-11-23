using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class Store : MonoBehaviour
{
    public Button[] tapButton; // skin block boong heart
    public GameObject[] storeView;
    public GameObject[] powders;

    public Button rewardAdsButton;
    public Text rewardAdsText;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        if(GoogleAdsManager.instance.canRewarded)
        {
            rewardAdsButton.interactable = true;
            rewardAdsText.gameObject.SetActive(false);
        }
        else
        {
            rewardAdsButton.interactable = false;
            rewardAdsText.gameObject.SetActive(true);
            rewardAdsText.text = IntToTime(GoogleAdsManager.instance.rewardAdsTime);
            
        }
    }

    public void TapButtonClicked(int num)
    {
        for(int i = 0; i < tapButton.Length; i++)
        {
            if (i == num)
            {
                tapButton[i].interactable = false;
                storeView[i].SetActive(true);
                powders[i].SetActive(true);
            }                
            else
            {
                tapButton[i].interactable = true;
                storeView[i].SetActive(false);
                powders[i].SetActive(false);
            }
                


        }
    }

    public void BuySkinForBoong(string name)
    {

    }

    public void BuySkinForPowder(string name)
    {

    }

    public void BuyBlockForPowder(string name)
    {

    }

    /// <summary>
    /// 확정아님
    /// </summary>
    public void BuyBoongForCash()
    {

    }

    public void BuyHeartForCash()
    {

    }

    public void BackToLobby()
    {
        gameObject.SetActive(false);
    }

    string IntToTime(int time)
    {
        string time_string = "";
        int min = 0;
        int sec = 0;
        while(time != 0)
        {
            if(time >= 60)
            {
                time -= 60;
                min++;
            }
            else
            {
                sec = time;
                time = 0;
            }
        }
        time_string = min + ":" + sec;
        return time_string;
    }
}
