using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;
    public delegate void QuestDelegate(int questNumber, QuestState state);
    public static QuestDelegate questDelegate;
    
    public QuestItemUIPrefab questItemUIPrefab;
    public QuestPopup questPopup;
    private void Awake()
    {
       
        
        if (instance == null)
        {
            instance = this;  
        }
        else
        {
            Destroy(gameObject);
        }

        questDelegate = XmlQuestDelegateFunc;
       

        using(var e = CSVManager.questData.GetInfoEnumerator())
        {
            while(e.MoveNext())
            {
                var data = e.Current.Value;
                QuestItemUIPrefab questItemUI = Instantiate(questItemUIPrefab);
                questItemUI.SetUIPrefab(data);
                questPopup.SetAsParentOfChild(questItemUI.transform);

                data.ObserveEveryValueChanged(x => x.GetQuestState())
                    .Subscribe(x => RefreshQuestItem()); 
            }
        }
       

        DontDestroyOnLoad(gameObject);
        
        
    }

    private void Start()
    {
       
    }

    void XmlQuestDelegateFunc(int questNumber, QuestState state)
    {
        string nickname = XMLManager.ins.database.userInfo.nickname;
        QuestData.Info info = CSVManager.questData.GetInfo(questNumber);
        if (info.GetQuestState() == QuestState.Wait && state == QuestState.OnProgress)//새로운 퀘스트 생성
        {
            info.SetQuestState(state);
            UserQuest userQuest = new UserQuest(nickname, questNumber, (int)state);
            XMLManager.ins.database.userQuests.Add(userQuest);
            //UserQuest Create
        }
        else if (info.GetQuestState() == QuestState.OnProgress && state == QuestState.Watched)//퀘스트 오픈 상태
        {
            info.SetQuestState(state);
            UserQuest userQuest = new UserQuest(nickname, questNumber, (int)state);
            XMLManager.ins.database.userQuests.Find(x => x.quest_number == questNumber).quest_state = (int)state;
            //UserQuest Update
        }
        else if ((info.GetQuestState() == QuestState.OnProgress || info.GetQuestState() == QuestState.Watched)
            && state == QuestState.Clear)//클리어
        {
            info.SetQuestState(state);
            UserQuest userQuest = new UserQuest(nickname, questNumber, (int)state);
            XMLManager.ins.database.userQuests.Find(x => x.quest_number == questNumber).quest_state = (int)state;

            if(info.GetUnLockQuestIdx() != null)
            {
                foreach (var unLockIdx in info.GetUnLockQuestIdx())
                {
                    questDelegate(unLockIdx, QuestState.OnProgress);
                }
            }
            
            //UserQuest Update
        }
        else if (info.GetQuestState() == QuestState.Clear && state == QuestState.Rewarded)//보상 획득
        {
            info.SetQuestState(state);
            UserQuest userQuest = new UserQuest(nickname, questNumber, (int)state);


            UserInfo userInfo = XMLManager.ins.database.userInfo;
            UserHistory userHistory = XMLManager.ins.database.userHistory;
           

            userInfo.boong += info.GetBoongReward();
            userInfo.heart += info.GetHeartReward();
            userHistory.boong_get += info.GetBoongReward();
            userHistory.heart_get += info.GetHeartReward();

            XMLManager.ins.database.userQuests.Find(x => x.quest_number == questNumber).quest_state = (int)state;
        }
        else
        {
            //do nothing...
        }
    }

    



    void RefreshQuestItem()
    {
        if (questPopup != null)
        {
            Debug.Log("Refresh quest popup");
            questPopup.RefreshQuestItemParent();
        }
            
    }
    
}
