using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
public class StageSelectIslandUIPrefab : UIPrefab
{
    private Image islandBackgroundImage;
    private List<StageButtonUIPrefab> stageButtonUIPrefabs;


    private void Awake()
    {
        islandBackgroundImage = this.gameObject.GetComponent<Image>();    
    }

    public override void SetUIPrefab(BaseData.BaseDataInfo data)
    {
        base.SetUIPrefab(data);
        IslandData.Info islandData = (IslandData.Info)data;

        Sprite sprite = null;
        sprite = Resources.Load<Sprite>("LevelScene/Island/"+ islandData.GetIslandNumber());
        if(sprite == null)
        {
            sprite = Resources.Load<Sprite>("LevelScene/Island/" + islandData.GetIslandName());
        }

        if (sprite != null)
        {
            islandBackgroundImage.sprite = sprite;
        }

    }

    public void SetIslandBackgroundImage(Sprite sprite)
    {
        islandBackgroundImage.sprite = sprite;
    }
    public void SetStageUIPrefab(StageButtonUIPrefab stageButtonUI)
    {
        stageButtonUIPrefabs.Add(stageButtonUI);
    }
    public void SetStageUIPrefabs(List<StageButtonUIPrefab> stageButtonUIs)
    {
        stageButtonUIPrefabs = stageButtonUIs;
    }
    
}
