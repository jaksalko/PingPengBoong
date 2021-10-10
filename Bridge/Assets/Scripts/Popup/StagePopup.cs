using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;
public class StagePopup : UIPrefab
{
    public Image starImage;
    public Image stageImage;

    public Text titleText;
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI snowText;
    public TextMeshProUGUI moveText;

    public Image fader;
    public GameObject heartAnimation;
    public float fadeOutTime;

    int islandNumber;
    int stageNumber;


    public void SetPopup(StageData.Info stageData)
    {
        gameObject.SetActive(true);

        GameManager.instance.stageDataOnPlay = stageData;

        Debug.Log("islandNumber : " + stageData.GetIslandNumber());
        stageNumber = stageData.GetStageNumber();
        islandNumber = stageData.GetIslandNumber();

        stageText.text = stageData.GetStageText();//스테이지 번호 텍스트
        starImage.sprite = Resources.Load<Sprite>("Star/" + stageData.GetStarCount());
        stageImage.sprite = Resources.Load<Sprite>("Stage/" + stageData.GetStageText());
        titleText.text = stageData.GetStageName();//스테이지 타이틀 텍스트
        moveText.text = stageData.GetMap().GetStepLimitsByIndex(2).ToString();//걷기 텍스트
        snowText.text = stageData.GetNumberOfSnow().ToString();//눈 갯수 텍스트

    
        

    }

    public void PlayButtonClicked()
    {

        gameManager.stageDataOnPlay = CSVManager.stageData.GetInfo(islandNumber, stageNumber);
        string islandName = CSVManager.islandData.GetInfo(gameManager.stageDataOnPlay.GetIslandNumber()).GetIslandName();
        LoadSceneBySceneName(islandName, true);

        /*
        if(awsManager.userInfo.heart > 0)
        {
            UserInfo copyInfo = awsManager.userInfo.DeepCopy();
            UserHistory copyHistory = awsManager.userHistory.DeepCopy();

            copyInfo.heart--;
            copyHistory.heart_use++;
            InfoHistory startStage = new InfoHistory(copyInfo, copyHistory);

            var request = jsonAdapter.POST_DATA(startStage, "infoHistory/update", (isConnect)=> {
                gameManager.playIslandName = csvManager.GetIslandNameByNumber(islandNumber);
                gameManager.islandDataOnPlay = csvManager.GetIslandDataByNumber(islandNumber);


                gameManager.playStageName = gameManager.islandDataOnPlay.GetStageByIndex(stageNumber).GetStageName();
                gameManager.stageDataOnPlay = gameManager.islandDataOnPlay.GetStageByIndex(stageNumber);

                Debug.Log("islandNumber : " + islandNumber + " " + gameManager.playIslandName);
                LoadSceneBySceneName(gameManager.playIslandName, true);
            });

            jsonAdapter.ReadyRequest(request);
            */
    }

       

    

}
