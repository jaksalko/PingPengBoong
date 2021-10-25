using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using System.Linq;
using UnityEngine;

public class JsonData
{
    public virtual void CopyToLocal()
    {

    }
}

public class BuyItem : JsonData
{
    public UserInfo userInfo;
    public UserHistory userHistory;
    public UserInventory item;

    public BuyItem(UserInfo i, UserHistory h, UserInventory inventory)
    {
        userInfo = i;
        userHistory = h;
        item = inventory;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        AWSManager.instance.userInfo = userInfo;
        AWSManager.instance.userHistory = userHistory;
        AWSManager.instance.userInventory.Add(item);
    }
}

public class UserAccountCreate : JsonData
{
    public UserInfo userInfo;
    public UserHistory userHistory;
    public UserInventory item1;
    public UserInventory item2;
    public UserAccountCreate(UserInfo i, UserHistory h, UserInventory item1, UserInventory item2)
    {
        userInfo = i;
        userHistory = h;
        this.item1 = item1;
        this.item2 = item2;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        AWSManager.instance.userInfo = userInfo;
        AWSManager.instance.userHistory = userHistory;
        AWSManager.instance.userInventory.Add(item1);
        AWSManager.instance.userInventory.Add(item2);
    }
}
//친구 신청
public class FriendRequest : JsonData
{
    public UserFriend myRequest;
    public UserFriend friendRequest;

    public FriendRequest(UserFriend my, UserFriend friend)
    {
        myRequest = my;
        friendRequest = friend;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        if(AWSManager.instance.userFriend.Exists(x => x.nickname_friend == myRequest.nickname_friend))
        {
            AWSManager.instance.userFriend.Remove(AWSManager.instance.userFriend.Find(x => x.nickname_friend == myRequest.nickname_friend));
        }
        else
        {
            AWSManager.instance.userFriend.Add(myRequest);
        }
        
    }
}

public class MakeEditorMapData : JsonData
{
    public UserHistory userHistory;
    public EditorStage editorStage;

    public MakeEditorMapData(UserHistory userHistory , EditorStage editorStage)
    {
        this.userHistory = userHistory;
        this.editorStage = editorStage;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        AWSManager.instance.userHistory = userHistory;
    }
}
public class RewardRequest : JsonData
{
    public UserInfo info;
    public UserHistory history;
    public UserReward reward;
    public UserInventory inventory;

    public RewardRequest(UserInfo info, UserHistory history, UserReward reward, UserInventory inven)
    {
        this.info = info;
        this.history = history;
        this.reward = reward;
        inventory = inven;

    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();

        AWSManager.instance.userInfo = info;
        AWSManager.instance.userHistory = history;
        AWSManager.instance.userReward.Add(reward);
        if(inventory.item_name != "none")
            AWSManager.instance.userInventory.Add(inventory);
    }
}

public class HeartRequest : JsonData
{
    public Mailbox mailbox;
    public UserFriend myFriend;

    public HeartRequest(Mailbox mailbox_, UserFriend friend)
    {
        mailbox = mailbox_;
        myFriend = friend;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        var friend = AWSManager.instance.userFriend.Find(x => x.nickname_friend == myFriend.nickname_friend);
        friend = myFriend;
    }

}

public class ClearQuest : JsonData
{
    public UserInfo userInfo;
    public UserHistory userHistory;
    public UserQuest userQuest;


    public ClearQuest(UserInfo i, UserHistory h, UserQuest q)
    {
        userInfo = i;
        userHistory = h;
        userQuest = q;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        AWSManager.instance.userInfo = userInfo;
        AWSManager.instance.userHistory = userHistory;
        userQuest.CopyToLocal();
    }
}

public class InfoHistory : JsonData
{
    public UserInfo userInfo;
    public UserHistory userHistory;

    public InfoHistory(UserInfo i, UserHistory h)
    {
        userInfo = i;
        userHistory = h;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        AWSManager.instance.userInfo = userInfo;
        AWSManager.instance.userHistory = userHistory;
    }
}

public class EditorClearRequest : JsonData
{
    public UserInfo userInfo;//붕과 별사탕 업데이트
    public UserHistory userHistory;//에디터 클리어 정보와 재화 획득 기록
    public UserStage userStage;//클리어 한 에디터 맵 추가
    public EditorStage editorMap;//플레이 한 에디터 맵 업데이트(플레이 카운트 )

    public EditorClearRequest(UserInfo info, UserHistory history, UserStage stage, EditorStage editorStage)
    {
        userInfo = info;
        userHistory = history;
        userStage = stage;
        editorMap = editorStage;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        AWSManager.instance.userInfo = userInfo;
        AWSManager.instance.userHistory = userHistory;
        AWSManager.instance.userStage.Add(userStage.stage_name,userStage);
    }

}

public class StageClearRequest : JsonData
{
    public UserInfo userInfo;//붕과 별사탕 업데이트
    public UserHistory userHistory;//에디터 클리어 정보와 재화 획득 기록
    public UserStage userStage;//클리어 한 에디터 맵 추가


    public StageClearRequest(UserInfo info, UserHistory history, UserStage stage)
    {
        userInfo = info;
        userHistory = history;
        userStage = stage;

    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        AWSManager.instance.userInfo = userInfo;
        AWSManager.instance.userHistory = userHistory;
        AWSManager.instance.userStage.Add(userStage.stage_name, userStage);
    }

}

public class StartStage
{
    public UserInfo userInfo;
    public UserHistory userHistory;

    public StartStage(UserInfo i, UserHistory h)
    {
        userInfo = i;
        userHistory = h;
    }
}

[Serializable]
public class UserInfo
{
    public string nickname; //primary key
    public int boong;
    public int heart;
    public int candy;
    public int skin_powder;
    public int block_powder;
    public int heart_time;
    public string log_out;
    public int skin_a;
    public int skin_b;
    public int profile_skin;
    public string profile_style;
    public string facebook_token;

  

    public UserInfo()
    {

    }

    public UserInfo(string nick , string facebook)
    {
        nickname = nick;
        facebook_token = facebook;

        boong = 0;
        heart = 5;
        candy = 0;
        skin_powder = 0;
        block_powder = 0;
        heart_time = 600;
        log_out = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        skin_a = 1;
        skin_b = 2;
        profile_skin = 0;
        profile_style = ""; //null

      
    }

    public UserInfo DeepCopy()
    {
        UserInfo copy = new UserInfo(nickname, facebook_token);
        copy.boong = boong;
        copy.heart = heart;
        copy.candy = candy;
        copy.skin_powder = skin_powder;
        copy.block_powder = block_powder;
        copy.heart_time = heart_time;
        copy.log_out = log_out;
        copy.skin_a = skin_a;
        copy.skin_b = skin_b;
        copy.profile_skin = profile_skin;
        copy.profile_style = profile_style; //null

        return copy;
    }
}

[Serializable]
public class UserHistory
{
    public string nickname;
    public int play_time;
    
    public int boong_get;
    public int boong_use;
    public int heart_get;
    public int heart_use;


    public int editor_make;
    public int editor_clear;
    public int editor_fail;

    public int stage_clear;
    public int stage_fail;

    public int drops; //떨어짐
    public int crash; //캐릭터와 부딪힘
    public int carry; //업기
    public int reset; //무르기
    public int move; //움직임
    public int snow; //치운 눈
    public int parfait;// 파르페 완성
    public int crack;// 부순 크래커
    public int cloud;// 솜사탕을 탄 횟수

    public UserHistory()
    {
    }

    public UserHistory(string nick)
    {
        nickname = nick;
        play_time = 0;
        boong_get = 0;
        boong_use = 0;
        heart_get = 0;
        heart_use = 0;

        editor_make = 0;
        editor_clear = 0;
        editor_fail = 0;

        stage_clear = 0;
        stage_fail = 0;

        drops = 0;
        crash = 0;
        carry = 0;
        reset = 0;
        move = 0;
        snow = 0;
        parfait = 0;
        crack = 0;
        cloud = 0;
    }

    public UserHistory DeepCopy()
    {
        UserHistory copy = new UserHistory();
        copy.nickname = nickname;
        copy.play_time = play_time;
        copy.boong_get = boong_get;
        copy.boong_use = boong_use;
        copy.heart_get = heart_get;
        copy.heart_use = heart_use;

        copy.editor_make = editor_make;
        copy.editor_clear = editor_clear;
        copy.editor_fail = editor_fail;

        copy.stage_clear = stage_clear;
        copy.stage_fail = stage_fail;

        copy.drops = drops;
        copy.crash = crash;
        copy.carry = carry;
        copy.reset = reset;
        copy.move = move;
        copy.snow = snow;
        copy.parfait = parfait;
        copy.crack = crack;
        copy.cloud = cloud;

        return copy;
    }
}

[Serializable]
public class UserReward
{
    public string nickname;
    public int reward_num;
    public string time_get;

    public UserReward()
    {

    }

    public UserReward(string nick, int num)
    {
        nickname = nick;
        reward_num = num;
        time_get = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public UserReward DeepCopy()
    {
        UserReward copy = new UserReward();
        copy.nickname = nickname;
        copy.reward_num = reward_num;
        copy.time_get = time_get;
        return copy;
    }
}
[Serializable]
public class UserQuest : JsonData
{
    public string nickname;
    public int quest_number;
    public int quest_state;
    public string quest_clear_time;

    public UserQuest()
    {

    }

    public UserQuest(string nick, int number, int state)
    {
        nickname = nick;
        quest_number = number;
        quest_state = state;
        quest_clear_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public UserQuest(string nick, int number, int state, string clear_time)
    {
        nickname = nick;
        quest_number = number;
        quest_state = state;
        quest_clear_time = clear_time;
    }

    public UserQuest DeepCopy()
    {
        UserQuest copy = new UserQuest(nickname, quest_number, quest_state, quest_clear_time);
        return copy;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        if(AWSManager.instance.userQuests.Exists(x => x.quest_number == quest_number))
        {
            AWSManager.instance.userQuests.
                Find(x => x.quest_number == quest_number).quest_state = quest_state;
        }
        else
        {
            AWSManager.instance.userQuests.Add(this);
        }
       
    }
}

[Serializable]
public class UserStage//클리어한 스테이지
{
    //nickname + stage_name = Key
    public int islandNumber;
    public int stageNumber;
    public string nickname;
    public string stage_name;
    public int stage_star;
    public int stage_move;
    public string stage_clear_time;//최초 클리어 시간

    public UserStage()
    {

    }

    public UserStage(string nick,int islandNumber,int stageNumber , string stageName, int star, int move)
    {
        this.islandNumber = islandNumber;
        this.stageNumber = stageNumber;

        nickname = nick;
        stage_name = stageName;
        stage_star = star;
        stage_move = move;
        stage_clear_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public void UpdateClearStage(int star, int move)
    {
        if (stage_star < star) stage_star = star;
        if (stage_move > move) stage_move = move;
    }

    public UserStage DeepCopy()
    {
        UserStage copy = new UserStage(nickname,islandNumber,stageNumber, stage_name, stage_star, stage_move);
        return copy;
    }
}

[Serializable]
public class UserRoom
{
    public delegate void ChangeSlotData(int slot,int data);
    public static event ChangeSlotData changeSlotEvent;

    //islandNumber 와 동일
    public int roomIdx;
    public bool isDirty;
    public int slot_0;
    public int slot_1;
    public int slot_2;
    public int slot_3;
    public int slot_4;

    public UserRoom()
    {

    }

    public UserRoom(int idx,bool dirty, int slot_0, int slot_1, int slot_2, int slot_3, int slot_4)
    {
        roomIdx = idx;
        isDirty = dirty;
        this.slot_0 = slot_0;
        this.slot_1 = slot_1;
        this.slot_2 = slot_2;
        this.slot_3 = slot_3;
        this.slot_4 = slot_4;
    }

    public int GetSlotData(int idx)
    {
        int result = -1;
        switch(idx)
        {
            case 0:
                result = slot_0;
                break;
            case 1:
                result = slot_1;
                break;
            case 2:
                result = slot_2;
                break;
            case 3:
                result = slot_3;
                break;
            case 4:
                result = slot_4;
                break;
        }

        return result;
    }

    public void SetSlotData(int slot, int key)
    {
        switch (slot)
        {
            case 0:
                slot_0 = key;
                break;
            case 1:
                slot_1 = key;
                break;
            case 2:
                slot_2 = key;
                break;
            case 3:
                slot_3 = key;
                break;
            case 4:
                slot_4 = key;
                break;
        }

        if(changeSlotEvent != null)
            changeSlotEvent.Invoke(slot,key);
    }
    public UserRoom DeepCopy()
    {
        UserRoom copy = new UserRoom(roomIdx,isDirty, slot_0, slot_1, slot_2, slot_3, slot_4);
        return copy;
    }
}

[Serializable]
public class UserInventory // 
{
    public int itemIdx;
    public string nickname;
    public string item_name;
    public string time_get;

    public UserInventory()
    {

    }

    public UserInventory(string nick, int idx, string name)
    {
        itemIdx = idx;
        nickname = nick;
        item_name = name;
        time_get = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public UserInventory DeepCopy()
    {
        UserInventory copy = new UserInventory(nickname,itemIdx, item_name);
        return copy;
    }
}

[Serializable]
public class Mailbox : JsonData
{
    public string receiver;
    public string sender;
    public string time;
    public string item;
    public int quantity;

    public Mailbox()
    {

    }

    public Mailbox(string receiver_ , string sender_ , string item_ , int quantitiy_)
    {
        receiver = receiver_;
        sender = sender_;
        time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        item = item_;
        quantity = quantitiy_;
    }

    public Mailbox DeepCopy()
    {
        Mailbox copy = new Mailbox(receiver, sender, item, quantity);
        return copy;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();
        AWSManager.instance.mailbox.Remove(this);
        AWSManager.instance.userInfo.heart += quantity;
        AWSManager.instance.userHistory.heart_get += quantity;
    }
}

//Send mine to friend
[Serializable]
public class UserFriend : JsonData
{
    public string nickname_mine;
    public string nickname_friend;
    public int friendship;
    public int state; // 0 1 2
    public string time_request;
    public bool send;

    public UserFriend()
    {

    }

    public UserFriend(string mine , string friend,int state)
    {
        nickname_mine = mine;
        nickname_friend = friend;
        friendship = 0;
        this.state = state;
        send = false;
        time_request = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public UserFriend DeepCopy()
    {
        UserFriend copy = new UserFriend(nickname_mine, nickname_friend, state);
        return copy;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();

        var friend = AWSManager.instance.userFriend
               .Find(x => x.nickname_friend == nickname_friend);

        friend = this;
    }
}

[Serializable]
public class EditorStage : JsonData
{
    public string map_id;//nickname + title // 같은 닉네임으로 같은 타이틀 제작 금지
    public string maker;
    public string title;
    public string make_time;
    public int play_count;
    public int likes;
    public int height;
    public int width;
    public string datas;//list parsing;
    public string styles;//list parsing;
    public string star_limit;//list parsing
    public int move;
    public int level;

    public EditorStage()
    {

    }

    public EditorStage(StageData.Info stageData,string nick , int moveCount)
    {
        Map map = stageData.GetMap();

        map_id = nick + stageData.GetStageName();
        maker = nick;
        title = stageData.GetStageName();
        make_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        play_count = 0;
        likes = 0;
        height = map.mapSizeHeight;
        width = map.mapSizeWidth;
        datas = Parser.ListListToString(map.GetBlockDataList());
        styles = Parser.ListListToString(map.GetBlockStyleList());
        star_limit = Parser.ArrayToString(map.GetStepLimits());

        move = moveCount;

        level = move / 5 + 1;
        if (level > 5) level = 5;

    }

    public EditorStage DeepCopy()
    {
        EditorStage copy = new EditorStage();


        return copy;
    }

    public override void CopyToLocal()
    {
        base.CopyToLocal();

    }
}

