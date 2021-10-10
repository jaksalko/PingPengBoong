using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UnityEngine.UI;
public class StarGuideBookPopup : UIPrefab
{
    
    
    public Button guideBookButton;
    public Button starRewardButton;

    public Button allStageTabButton;
    public Button nonclearStageTabButton;

    public GameObject totalStarTab;
    public GameObject nonCompleteStarTab;


    public Transform islandTabContent;
    public Transform starRewardTab;

    public Transform scrollRectContent;
    public StarRewardSlider[] rewardSliders;
    public StarGuideBookTagButtonUIPrefab[] islandTagButtons;
    Dictionary<int, float> tagHeightDictionary = new Dictionary<int, float>();
    float totalStarContentSize = 0;

    Dictionary<int, StarGuideBookIslandScrollRect> islandDictionary = new Dictionary<int, StarGuideBookIslandScrollRect>();

    // Start is called before the first frame update
    //protected override void Start()
    private void Awake()
    {
        //base.Start();
        CSVManager csvManager = CSVManager.instance;
        

        using (var e = CSVManager.islandData.GetInfoEnumerator())
        {
            while (e.MoveNext())
            {
                var data = e.Current.Value;
                StarGuideBookIslandScrollRect starGuideBookIslandScrollRect = Instantiate(Resources.Load<StarGuideBookIslandScrollRect>("Prefab/StarGuideBookIslandScrollRect"));
                starGuideBookIslandScrollRect.Init(data.key);
                starGuideBookIslandScrollRect.SetParentAsLastSibling(scrollRectContent);
                islandDictionary.Add(data.key, starGuideBookIslandScrollRect);
                rewardSliders[data.key - 1].SetUIPrefab(data);
            }
        }

        using (var e = CSVManager.stageData.GetInfoEnumerator())
        {
            while(e.MoveNext())
            {
                var data = e.Current.Value;
                int key = data.key;

                StageButtonUIPrefab stageButtonUI = Instantiate(Resources.Load<StageButtonUIPrefab>("Prefab/StageButtonUIPrefab"));
                stageButtonUI.SetUIPrefab(data);

                islandDictionary[data.GetIslandNumber()].AddChild(stageButtonUI);
                /*


                if (data.GetStarCount() != 3 && data.GetIsClear())
                {
                    nonclearStageDataLength++;
                }

                if (data.GetStarCount() != 3 && data.GetIsClear())
                {
                    if (nonclearStageDataCount % StageRoadUIPrefab.maxStageButtonCount == 0)
                    {
                        nonclearStageRoadUI = Instantiate(stageRoadUIPrefab);
                        nonclearStageRoadUI.SetParentAsLastSibling(nonCompleteStarTabContent);
                    }

                    StageButtonUIPrefab nonclaerStageButtonUI = Instantiate(stageButtonUIPrefab);
                    nonclaerStageButtonUI.SetUIPrefab(data);

                    if (nonclearStageRoadUI != null)
                    {
                        nonclearStageRoadUI.SetAsParentOfChild(nonclaerStageButtonUI.transform);
                    }

                    nonclearStageDataCount++;

                }




                if (stageDataCount % StageRoadUIPrefab.maxStageButtonCount == 0)
                {
                    stageRoadUI = Instantiate(stageRoadUIPrefab);
                    stageRoadUI.SetParentAsLastSibling(totalStarTabContent);
                    totalStarContentSize += stageRoadUIPrefab.GetComponent<RectTransform>().sizeDelta.y;
                }

                if (stageDataCount % StageRoadUIPrefab.maxHorizontalButtonCount == 0)
                {

                }


                StageButtonUIPrefab stageButtonUI = Instantiate(stageButtonUIPrefab);
                stageButtonUI.SetUIPrefab(data);
                if (stageRoadUI != null)
                {
                    stageRoadUI.SetAsParentOfChild(stageButtonUI.transform);
                }

                if (data.GetStageNumber() == 1)
                {
                   
                    tagHeightDictionary.Add(data.GetIslandNumber(), (stageDataCount / StageRoadUIPrefab.maxHorizontalButtonCount) * stageRoadUIPrefab.GetComponent<RectTransform>().sizeDelta.y * 0.5f);
                }


                stageDataCount++;
                */
            }

            
        }

        /*

        while(nonCompleteStarTabContent.childCount < 2)
        {
            nonclearStageRoadUI = Instantiate(stageRoadUIPrefab);
            nonclearStageRoadUI.SetParentAsFirstSibling(nonCompleteStarTabContent);
        }
        
        */

        guideBookButton.onClick.AddListener(() => ActiveTotalStarTab());
        starRewardButton.onClick.AddListener(() => ActiveStarRewardTab());
        allStageTabButton.onClick.AddListener(() => ActiveTotalStarTab());
        nonclearStageTabButton.onClick.AddListener(() => ActiveNonCompleteStarTab());

        foreach(var tagButton in islandTagButtons)
        {
            tagButton.GetComponent<Button>().onClick.AddListener(delegate {
                TagButtonClicked(tagButton.islandNumber);
            });
        }
        ActiveTotalStarTab();


        //TagButtonClicked(5);
    }
    /*
    public void TagButtonClicked(int islandNumber)
    {
        Debug.Log(islandNumber);
        ScrollRect scrollRect = totalStarTabContent.parent.parent.GetComponent<ScrollRect>();
       
        float scrollValue = tagHeightDictionary[islandNumber]/totalStarContentSize;
        Debug.Log(scrollValue);
        scrollRect.verticalScrollbar.value = scrollValue;
        if(islandNumber == 1)
            scrollRect.verticalScrollbar.value = 0.01f;


    }
    */
    public void TagButtonClicked(int islandNumber)
    {
        Debug.Log(islandNumber);
        foreach(var island in islandDictionary.Values)
        {
            island.gameObject.SetActive(false);
        }
        islandDictionary[islandNumber].gameObject.SetActive(true);
        /*
        ScrollRect scrollRect = totalStarTabContent.parent.parent.GetComponent<ScrollRect>();

        float scrollValue = tagHeightDictionary[islandNumber] / totalStarContentSize;
        Debug.Log(scrollValue);
        scrollRect.verticalScrollbar.value = scrollValue;
        if (islandNumber == 1)
            scrollRect.verticalScrollbar.value = 0.01f;

        */
    }

    void ActiveStarRewardTab()
    {
        guideBookButton.interactable = true;
        starRewardButton.interactable = false;

        islandTabContent.gameObject.SetActive(false);
        starRewardTab.gameObject.SetActive(true);
    }

    void ActiveTotalStarTab()
    {
        guideBookButton.interactable = false;
        starRewardButton.interactable = true;

        starRewardTab.gameObject.SetActive(false);
        islandTabContent.gameObject.SetActive(true);

        totalStarTab.gameObject.SetActive(true);
        nonCompleteStarTab.gameObject.SetActive(false);
        
        foreach(var islandScrollRect in islandDictionary.Values)
        {
            islandScrollRect.ActiveAllStage();
        }
        


    }

    void ActiveNonCompleteStarTab()
    {
        guideBookButton.interactable = false;
        starRewardButton.interactable = true;

        starRewardTab.gameObject.SetActive(false);
        islandTabContent.gameObject.SetActive(true);

        totalStarTab.gameObject.SetActive(false);
        nonCompleteStarTab.gameObject.SetActive(true);
        
        foreach (var islandScrollRect in islandDictionary.Values)
        {
            islandScrollRect.InActiveClearStage();
        }
        
    }

}
