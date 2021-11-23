using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Data;
public class StageSceneResultPopup : UIPrefab
{
    public TextMeshProUGUI moveCount;
    public TextMeshProUGUI snowCount;
    public Text stageText;

    public Image[] starImage;

    public GameObject failEffect;
    public GameObject successEffect;
    public GameObject successPopup; //star image //move text //3 buttons //stage text 
    public GameObject failPopup;    //remain snow text //2 buttons // stage text

    public Transform buttons;
    public Button homeBtn;
    public Button retryButton;
    public Button nextButton;

    public AudioSource successAudio;

    public Text boongAmountText;
    public GameObject rewardAdButton;
    bool clicked = false;
    public void ShowResultPopup(bool isSuccess,int remain_snow,int move_count , int star_count, int getBoongAmount)
    {
        gameObject.SetActive(true);
        int beforeUserBoongAmount = XMLManager.ins.database.userInfo.boong;
        boongAmountText.text = beforeUserBoongAmount.ToString();

        stageText.text = "STAGE " + GameManager.instance.stageDataOnPlay.GetStageText();

        rewardAdButton.SetActive(false);

        if (isSuccess)
        {
            //rewardAdButton.SetActive(true);

            stageText.transform.SetParent(successPopup.transform, false);
            buttons.SetParent(successPopup.transform, false);

            if(GameManager.instance.stageDataOnPlay.GetIslandNumber() == 1 && GameManager.instance.stageDataOnPlay.GetStageNumber() != 6)
            {
                homeBtn.gameObject.SetActive(false);
            }

            successEffect.SetActive(true);
            successPopup.SetActive(true);

            int key = GameManager.instance.stageDataOnPlay.key;
            int nextKey = GameManager.instance.stageDataOnPlay.key + 1;
            if(CSVManager.stageData.GetInfo(nextKey).GetIslandNumber() != CSVManager.stageData.GetInfo(key).GetIslandNumber())
            {
                nextButton.gameObject.SetActive(false);
            }
            
            /*
            for(int i = 0 ; i < csvManager.islandData.island_last.Length ; i++)
            {
                
                if(gameManager.nowLevel == csvManager.islandData.island_last[i])
                {
                    nextButton.gameObject.SetActive(false);
                }
            }
            */
        }
        else
        {
            //rewardAdButton.SetActive(false);
            stageText.transform.SetParent(failPopup.transform, false);
            buttons.SetParent(failPopup.transform, false);

            failEffect.SetActive(true);
            failPopup.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }

        moveCount.text = move_count.ToString();
        snowCount.text = remain_snow.ToString();

        for(int i = 0; i < starImage.Length; i++)
        {
            if(i < star_count)
            {
                starImage[i].sprite = Resources.Load<Sprite>("Popup/star_clear_" + (i + 1));
            }
            else
            {
                starImage[i].sprite = Resources.Load<Sprite>("Popup/star_fail_" + (i + 1));
            }
        }
       
        

    }

    protected override void Start()
    {
        base.Start();
        successAudio.Play();
    }

    public void GoLobbyButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ReplayButtonClicked()
    {
        int islandNumber = gameManager.stageDataOnPlay.GetIslandNumber();
        string islandName = CSVManager.islandData.GetInfo(islandNumber).GetIslandName();
        LoadingSceneManager.instance.LoadScene(islandName, true);
        /*
        if(awsManager.userInfo.heart > 0)
        {
            UserInfo copyInfo = awsManager.userInfo.DeepCopy();
            UserHistory copyHistory = awsManager.userHistory.DeepCopy();

            copyInfo.heart--;
            copyHistory.heart_use++;
            InfoHistory startStage = new InfoHistory(copyInfo, copyHistory);

            var request = jsonAdapter.POST_DATA(startStage, "infoHistory/update", (isConnect) => {
                LoadingSceneManager.instance.LoadScene(gameManager.playIslandName, true);
            });


            jsonAdapter.ReadyRequest(request);
        }
        */
    }
    public void NextStageButtonClicked()
    {
        if(!clicked)
        {
            clicked = true;
            int nextStageKey = gameManager.stageDataOnPlay.key + 1;
            gameManager.stageDataOnPlay = CSVManager.stageData.GetInfo(nextStageKey);
            int nextIslandKey = gameManager.stageDataOnPlay.GetIslandNumber();
            string nextIslandName = CSVManager.islandData.GetInfo(nextIslandKey).GetIslandName();

            LoadingSceneManager.instance.LoadScene(nextIslandName, true);
        }
    }
       
    public void RewardAdClicked()
    {
        GoogleAdsManager.instance.RewardEvent.AddListener(GetAdReward);
        GoogleAdsManager.instance.ShowRewardAd();
        rewardAdButton.SetActive(false);
    }

    void GetAdReward()
    {
        int getBoongAmount = 200 + gameManager.stageDataOnPlay.GetIslandNumber() * 50;
        getBoongAmount = getBoongAmount / 2;
        XMLManager.ins.database.userInfo.boong += getBoongAmount;
        XMLManager.ins.database.userHistory.boong_get += getBoongAmount;
        boongAmountText.text = XMLManager.ins.database.userInfo.boong.ToString();


    }

    private void OnDestroy()
    {
        GoogleAdsManager.instance.RewardEvent.RemoveListener(GetAdReward);
    }

}
