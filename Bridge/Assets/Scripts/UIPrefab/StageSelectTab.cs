using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StageSelectTab : UIPrefab
{
    Dictionary<int, float> stageSelectScrollHeightDicitionary = new Dictionary<int, float>();

    public void SetHeightDictionary(int islandNumber,float size)
    {
        stageSelectScrollHeightDicitionary.Add(islandNumber, size);
        Debug.Log(islandNumber + " : " + size);
    }
    public void ActivateStageSelectTab(int islandNumber)
    {
        ScrollRect scrollRect = transform.GetChild(0).GetComponent<ScrollRect>();
        Transform scrollRectContent = scrollRect.content;

        float addHeight = 0;
        foreach(var height in stageSelectScrollHeightDicitionary)
        {
            if(height.Key < islandNumber)
            {
                addHeight += height.Value;
            }
        }
        Debug.Log(addHeight);
        Vector2 contentPosition = scrollRectContent.GetComponent<RectTransform>().anchoredPosition;
        contentPosition.y = -addHeight - 1920;
        scrollRectContent.GetComponent<RectTransform>().anchoredPosition = contentPosition;
        gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        QuestManager.questDelegate(2, Data.QuestState.OnProgress);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
