using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
public class StageButtonUIPrefab : UIPrefab
{
    private Image stageTextImage;
    private Image stageStarImage;
    private Button stageButton;

    private void Awake()
    {
        stageButton = this.gameObject.GetComponent<Button>();
        stageButton.onClick.AddListener(()=>StageButtonClicked());

        stageTextImage = this.gameObject.GetComponent<Image>();
        stageStarImage = stageTextImage.transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    public override void SetUIPrefab(BaseData.BaseDataInfo data)
    {
        base.SetUIPrefab(data);
        var stageData = (StageData.Info)data;
        stageTextImage.sprite = Resources.Load<Sprite>("LevelScene/Stage/stage_number_" + stageData.GetStageText());
        stageStarImage.sprite = Resources.Load<Sprite>("Star/" + stageData.GetStarCount());

        if(stageData.GetIsOpen())
        {
            stageButton.interactable = true;
        }
        else
        {
            
            stageButton.interactable = false;
        }
       
    }

    public void SetStageButtonUIPrefab(Sprite stageSprite , Sprite starImage)
    {
        stageTextImage.sprite = stageSprite;
        stageStarImage.sprite = starImage;
    }

    public void SetStageButtonUIPrefab(int islandNumber,int stageNumber,bool isClear, int starCount)
    {
        stageTextImage.sprite = Resources.Load<Sprite>("LevelScene/Stage/stage_number_"+islandNumber+"_"+stageNumber);
        stageStarImage.sprite = Resources.Load<Sprite>("Star/"+starCount);
    }

    void StageButtonClicked()
    {
        GameObject.Find("Main Canvas").GetComponent<MainSceneUIScript>().stagePopup.SetPopup((StageData.Info)data);
    }

    public bool IsClearStage()
    {
        StageData.Info info = (StageData.Info)data;
        return info.GetIsClear();
    }

    public bool IsPerfectClear()
    {
        StageData.Info info = (StageData.Info)data;
        if (info.GetStarCount() == 3)
            return true;
        else
            return false;
    }

    public bool isOpenStage()
    {
        StageData.Info info = (StageData.Info)data;
        return info.GetIsOpen();
    }
}
