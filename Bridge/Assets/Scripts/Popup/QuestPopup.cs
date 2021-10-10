using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Data;
public class QuestPopup : UIPrefab
{

    [SerializeField]
    Transform onProgressTab;//진행중이거나 보상을 받아야하는 퀘스트
    [SerializeField]
    Transform rewardedTab;//이미 보상까지 받은 퀘스트
    [SerializeField]
    Transform progressContent;
    [SerializeField]
    Transform rewardContent;
    [SerializeField]
    Transform waitTab;//그 외 퀘스트 // Active false

    [SerializeField]
    Button progressTabButton;
    [SerializeField]
    Button rewardedTabButton;


    [SerializeField]
    Button[] filterButtons;

    List<QuestItemUIPrefab> questItemUIs = new List<QuestItemUIPrefab>();
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        

        progressTabButton.onClick.AddListener(() => ProgressButtonClicked());
        rewardedTabButton.onClick.AddListener(() => RewardedButtonClicked());


        foreach (var button in filterButtons.Select((value,index)=>(value,index)))
        {
            button.value.onClick.AddListener(delegate { AllButtonClicked(button.index); });
        }

        ProgressButtonClicked();
    }

    public void ActivatePopup()
    {
        RefreshQuestItemParent();
        gameObject.SetActive(true);
    }

    public override void SetAsParentOfChild(Transform child)
    {
        base.SetAsParentOfChild(child);
        QuestItemUIPrefab questItemUI = child.GetComponent<QuestItemUIPrefab>();
        QuestState state = questItemUI.GetQuestState();
        questItemUIs.Add(questItemUI);

        switch (state)
        {
            case QuestState.Wait:
                child.SetParent(waitTab);
                break;
            case QuestState.OnProgress:
            case QuestState.Watched:
            case QuestState.Clear:
                child.SetParent(progressContent);
                break;
            case QuestState.Rewarded:
                child.SetParent(rewardContent);
                break;
        }
    }

    public void RefreshQuestItemParent()
    {
        foreach(var questItemUI in questItemUIs)
        {
            QuestState state = questItemUI.GetQuestState();
            switch (state)
            {
                case QuestState.Wait:
                    questItemUI.transform.SetParent(waitTab);
                    break;
                case QuestState.OnProgress:
                case QuestState.Watched:
                case QuestState.Clear:
                    questItemUI.transform.SetParent(progressContent);
                    break;
                case QuestState.Rewarded:
                    questItemUI.transform.SetParent(rewardContent);
                    break;
            }
        }
    }

    void AllButtonClicked(int idx)
    {
        if(idx == 0)
        {
            foreach (var questItemUI in questItemUIs)
            {
                questItemUI.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var questItemUI in questItemUIs)
            {
                if (idx == questItemUI.GetQuestType())
                {
                    questItemUI.gameObject.SetActive(true);
                }
                else
                {
                    questItemUI.gameObject.SetActive(false);
                }
            }
        }
       
    }

    void ProgressButtonClicked()
    {
        progressTabButton.interactable = false;
        rewardedTabButton.interactable = true;
        rewardedTab.gameObject.SetActive(false);
        onProgressTab.gameObject.SetActive(true);
    }

    void RewardedButtonClicked()
    {
        progressTabButton.interactable = true;
        rewardedTabButton.interactable = false;

        onProgressTab.gameObject.SetActive(false);
        rewardedTab.gameObject.SetActive(true);
    }
    

}
