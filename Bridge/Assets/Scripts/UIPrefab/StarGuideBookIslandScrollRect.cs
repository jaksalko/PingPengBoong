using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarGuideBookIslandScrollRect : UIPrefab
{
    public Image backgroundImage;
    public Transform content;
    public List<StageButtonUIPrefab> stageButtonUIs;
    public void Init(int islandNumber)
    {
        backgroundImage.sprite = Resources.Load<Sprite>(Constants.StarGuidePath + islandNumber);

    }

    public void AddChild(StageButtonUIPrefab go)
    {
        go.SetParentAsLastSibling(content);
        stageButtonUIs.Add(go);
    }

    public void InActiveClearStage()
    {
        foreach(var stageButton in stageButtonUIs)
        {
            if(stageButton.isOpenStage() && !stageButton.IsPerfectClear())
            {
                stageButton.gameObject.SetActive(true);
            }
            else
            {
                stageButton.gameObject.SetActive(false);
            }
           
        }


    }

    public void ActiveAllStage()
    {
        foreach (var stageButton in stageButtonUIs)
        {
            stageButton.gameObject.SetActive(true);
        }


    }
}
