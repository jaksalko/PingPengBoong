using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
using UniRx;
using UniRx.Triggers;
public class QuestItemUIPrefab : UIPrefab
{
    

    private Button foldedTransformTitle;
    private Button unFoldedTransformTitle;
    private Text questContentText;
    private Text questRewardText;
    private Button rewardButton;
    private Button replayButton;

    private Transform unFoldedTransform;
    private Transform foldedTransform;



    private void Awake()
    {
        foldedTransform = transform.GetChild(0).GetComponent<Transform>();
        unFoldedTransform = transform.GetChild(1).GetComponent<Transform>();

        foldedTransformTitle = foldedTransform.GetChild(0).GetComponent<Button>();
        unFoldedTransformTitle = unFoldedTransform.GetChild(0).GetComponent<Button>();

        foldedTransformTitle.onClick.AddListener(() => UnFoldQuestItem());
        unFoldedTransformTitle.onClick.AddListener(() => FoldQuestItem());

        questContentText = unFoldedTransform.GetChild(1).GetComponent<Text>();
        questRewardText = unFoldedTransform.GetChild(2).GetComponent<Text>();
        replayButton = unFoldedTransform.GetChild(3).GetChild(0).GetComponent<Button>();
        rewardButton = unFoldedTransform.GetChild(3).GetChild(1).GetComponent<Button>();

        replayButton.onClick.AddListener(() => ReplayQuestInfo());
        rewardButton.onClick.AddListener(() => GetReward());
    }

    protected override void Start()
    {
        base.Start();   
    }

    public override void SetUIPrefab(BaseData.BaseDataInfo data)
    {
        base.SetUIPrefab(data);
        QuestData.Info questData = (QuestData.Info)data;
        foldedTransformTitle.GetComponent<Text>().text = questData.GetQuestTitle();
        unFoldedTransformTitle.GetComponent<Text>().text = questData.GetQuestTitle();

        questContentText.text = questData.GetQuestContent();
        questRewardText.text = questData.GetRewardString();

        switch(questData.GetQuestCategory())
        {
            case 1:
                foldedTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Quest/mainBox_folded");
                unFoldedTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Quest/mainBox_unfolded");
                break;
            case 2:
                foldedTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Quest/generalBox_folded");
                unFoldedTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Quest/generalBox_unfolded");
                break;
            case 3:
                foldedTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Quest/epicBox_folded");
                unFoldedTransform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Quest/epicBox_unfolded");
                break;
        }


        SetRewardButtonInteractable((QuestData.Info)data);
        questData.stateChangeAction += SetRewardButtonInteractable;

        if(!questData.HasScene() && !questData.HasStory() && !questData.HasCartoon())
        {
            replayButton.gameObject.SetActive(false);
        }

        FoldQuestItem();

    }

    void FoldQuestItem()
    {
        unFoldedTransform.gameObject.SetActive(false);
        foldedTransform.gameObject.SetActive(true);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    void UnFoldQuestItem()
    {
        unFoldedTransform.gameObject.SetActive(true);
        foldedTransform.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        QuestData.Info questData = (QuestData.Info)data;
        if (questData.GetQuestState()==QuestState.OnProgress)
        {
            QuestManager.questDelegate(questData.GetQuestNumber(), QuestState.Watched);
            StartCoroutine(InfoIEnumerator(questData));
            //TutorialManager.instance.StartImageDialog(questData.GetImageDialogDatas());
        }
    }

    void SetRewardButtonInteractable(QuestData.Info questData)
    {
        if(rewardButton != null)
        {
            if (questData.GetQuestState() == QuestState.Clear)
            {
                rewardButton.gameObject.SetActive(true);
                rewardButton.interactable = true;
            }
            else if(questData.GetQuestState() == QuestState.Rewarded)
            {
                rewardButton.gameObject.SetActive(false);
            }
            else
            {
                rewardButton.gameObject.SetActive(true);
                rewardButton.interactable = false;
            }
        }
       
    }

    public int GetQuestType() { QuestData.Info questData = (QuestData.Info)data; return questData.GetQuestCategory(); }
    public QuestState GetQuestState() { QuestData.Info questData = (QuestData.Info)data; return questData.GetQuestState(); }
    void GetReward()
    {
        QuestData.Info questData = (QuestData.Info)data;
        QuestManager.questDelegate(1, QuestState.Clear);
        QuestManager.questDelegate(questData.GetQuestNumber(), QuestState.Rewarded);
        SetRewardButtonInteractable((QuestData.Info)data);
    }

    void ReplayQuestInfo()
    {
        QuestData.Info questData = (QuestData.Info)data;
        StartCoroutine(InfoIEnumerator(questData));
    }



    IEnumerator InfoIEnumerator(QuestData.Info questData)
    {
        TutorialManager.instance.tutorialCanvas.gameObject.SetActive(true);

        if (questData.HasCartoon())
        {
            yield return StartCoroutine(TutorialManager.instance.StartCartoonView(questData.GetCartoonSprite()));

        }
        if (questData.HasStory())
        {
            if(questData.HasCartoon())
            {
                TutorialManager.instance.imageDialog.SetCartoonImage(questData.GetCartoonSprite(), true);
            }
            else
            {
                TutorialManager.instance.imageDialog.SetCartoonImage(false);
            }


            yield return StartCoroutine(TutorialManager.instance.StartImageDialog(questData.GetImageDialogDatas()));
        }
        if(questData.HasScene())
        {
            yield return StartCoroutine(TutorialManager.instance.StartSummaryView(questData.GetSummarySprites()));
        }

        TutorialManager.instance.tutorialCanvas.gameObject.SetActive(false);
        yield break;
    }


}
