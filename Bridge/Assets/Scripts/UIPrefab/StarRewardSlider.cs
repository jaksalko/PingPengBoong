using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using UnityEngine.UI;
using System;
public class StarRewardSlider : UIPrefab
{
    public Text sliderText;
    public Button rewardButton;
    public Slider starSlider;

    int reward_num = 0;

    public static Action action;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void SetUIPrefab(BaseData.BaseDataInfo data)
    {
        base.SetUIPrefab(data);
        this.data = (IslandData.Info)data;

        SetRewardUI();

        rewardButton.onClick.AddListener(() => GetReward());
    }

    void SetRewardUI()
    {
        IslandData.Info islandData = (IslandData.Info)data;
        
        starSlider.maxValue = islandData.GetMaxStar();
        rewardButton.interactable = false;


        for (int i = 0; i < 3; i++)
        {
            int reward_num = CSVManager.rewardData.GetInfoIdx(islandData.GetIslandNumber() - 1, i);
            int frequency = (islandData.GetMaxStar() / 3) * (i + 1);
            
            if (XMLManager.ins.database.userReward.Exists(x => x.reward_num == reward_num))//받은 보상
            {
                rewardButton.interactable = false;
            }
            else//받지 않은 보상
            {
                starSlider.maxValue = frequency;
                if(islandData.GetMyStar() >= frequency)
                {
                    rewardButton.interactable = true;
                }
                else
                {
                    rewardButton.interactable = false;
                }
               
                this.reward_num = reward_num;
                Debug.Log("RewardData Number : " + this.reward_num);
                break;
                
                
            }
        }

        starSlider.value = islandData.GetMyStar();
        sliderText.text = islandData.GetMyStar() + "/" + starSlider.maxValue;
    }

    void GetReward()
    {
        UserInfo userInfo = xmlManager.database.userInfo;
        UserHistory userHistory = xmlManager.database.userHistory;
        UserReward userReward = new UserReward(xmlManager.database.userInfo.nickname, reward_num);
        xmlManager.database.userReward.Add(userReward);

        RewardData.Info reward = CSVManager.rewardData.GetInfo(reward_num); 

        userInfo.boong += reward.boong;
        userInfo.heart += reward.heart;
        userInfo.block_powder += reward.block_powder;
        userInfo.skin_powder += reward.skin_powder;

        userHistory.boong_get += reward.boong;
        userHistory.heart_get += reward.heart;

        QuestManager.questDelegate(2, QuestState.Clear);
        SetRewardUI();

        /*
        RewardRequest rewardRequest = new RewardRequest(copy_user, copy_history, userReward, reward.userInventory);
        var request = JsonAdapter.instance.POST_DATA(rewardRequest, "userReward/insert", (isConnect) => {
            if(isConnect)
            {
                QuestManager.questDelegate(3, 4);
                SetRewardUI();
            }
            else
            {

            }
        });

        JsonAdapter.instance.ReadyRequest(request);
        */

    }

}
