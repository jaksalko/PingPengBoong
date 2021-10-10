using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

//섬 길 패널 프리
public class IslandRoadUIPrefab : UIPrefab
{
    [SerializeField]
    private StageButtonUIPrefab stageButtonUIPrefab;
    private Dictionary<int, Vector3> stageButtonPosition;
    private Image islandRoadBackgroundImage;
    private int maxStageButtonCount = 0;
    private void Awake()
    {
        islandRoadBackgroundImage = this.gameObject.GetComponent<Image>();
    }

    public override void SetUIPrefab(BaseData.BaseDataInfo data)
    {
        base.SetUIPrefab(data);
        this.data = (IslandData.Info)data;
        IslandData.Info islandData = (IslandData.Info)data;

        stageButtonPosition = new Dictionary<int, Vector3>();
        List<Vector3> islandStagePosition = IslandRoadUIStagePosition.IslandRoadUIPrefabStagePosition[islandData.GetIslandNumber()];
        int childCount = 1;
        foreach (Vector3 v in islandStagePosition)
        {
            stageButtonPosition.Add(childCount, v);
            childCount++;
        }

        
        islandRoadBackgroundImage.sprite = Resources.Load<Sprite>("LevelScene/StageBackground/" + islandData.GetIslandNumber());
        islandRoadBackgroundImage.rectTransform.sizeDelta = islandRoadBackgroundImage.sprite.rect.size;

        using(var e = CSVManager.stageData.GetInfoEnumerator())
        {
            while(e.MoveNext())
            {
                var value = e.Current.Value;
                if(value.GetIslandNumber() == data.key)
                {
                    StageButtonUIPrefab stageButtonUI = Instantiate(stageButtonUIPrefab);
                    stageButtonUI.SetUIPrefab(value);
                    SetAsParentOfChild(stageButtonUI.transform);
                }
            }
        }
        

        

    }
    public float GetIslandBackgroundSize() { return islandRoadBackgroundImage.rectTransform.sizeDelta.y; }
    public override void SetAsParentOfChild(Transform child)
    {
        base.SetAsParentOfChild(child);
        child.GetComponent<RectTransform>().localPosition = stageButtonPosition[transform.childCount];
    }
}
