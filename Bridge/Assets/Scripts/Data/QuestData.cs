using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
namespace Data
{
    public enum QuestState
    {
        Wait = 1,
        OnProgress,
        Watched,
        Clear,
        Rewarded
    }
    public class ImageDialogData : BaseData
    {
        public class Info : BaseDataInfo
        {
            public int questKey;
            public string speaker;
            public string emotion;
            public string line;

            public Info(int key,int questKey ,string speaker, string emotion, string line) : base(key)
            {
                this.questKey = questKey;
                this.speaker = speaker;
                this.emotion = emotion;
                this.line = line;
            }
        }

        readonly private Dictionary<int, Info> infos = new Dictionary<int, Info>();

        public Info GetInfo(int key)
        {
            return infos[key];
        }

        public List<Info> GetInfos(int questKey)
        {
            List<Info> returnInfo = new List<Info>();
            foreach(var info in infos.Values)
            {
                if(info.questKey == questKey)
                {
                    returnInfo.Add(info);
                }
            }
            return returnInfo;
        }


        public override void ClearData()
        {
            infos.Clear();
        }

        public override void Parsing(List<Dictionary<string, string>> csvDatas)
        {
            for (int idx = 1; idx < csvDatas.Count; idx++)
            {
                Info info = null;
                if (!infos.TryGetValue(idx, out info))
                {

                    int number = ParseInt(csvDatas[idx]["number"]);
                    string speaker = csvDatas[idx]["speaker"];
                    string emotion = csvDatas[idx]["emotion"];
                    string line = csvDatas[idx]["line"];

                    info = new Info(idx, number,speaker, emotion, line);
                    infos.Add(idx, info);
                }
                else
                {
                    Debug.LogErrorFormat("already exist data, key: {0}", idx);
                }
            }
        }
    }

    public struct DialogData 
    {
        public string speaker;
        public string line;

        public DialogData(string speaker, string line)
        {
            this.speaker = speaker;
            this.line = line;
        }
    }

    

    public class QuestData : BaseData
    {
        public class Info : BaseDataInfo
        {
            QuestState State;

            readonly private string questTitle;
            readonly private string questContent;
            readonly private int questCategory; // 1: main 2: general 3: epic
          
            readonly bool story = false;
            readonly bool cartoon = false;
            readonly bool scene = false;

            readonly int rewardBoong;
            readonly int rewardHeart;

            public Action<Info> stateChangeAction;

            List<Sprite> summarySprites = new List<Sprite>();
            Sprite cartoonSprite = null;

            public Info(int key, int category, string title, string content, bool story, bool cartoon, bool scene, int boong, int heart) : base(key)
            {
                questCategory = category;
                State = QuestState.Wait;
                questTitle = title;
                questContent = content;

                this.story = story;
                this.cartoon = cartoon;
                this.scene = scene;

                rewardBoong = boong;
                rewardHeart = heart;

                if (scene)
                {
                    summarySprites.AddRange(Resources.LoadAll<Sprite>("Quest/Summary/" + key));
                }

                if (cartoon)
                {
                    cartoonSprite = Resources.Load<Sprite>("Quest/Cartoon/" + key);
                }
            }


            public bool HasStory() { return story; }
            public bool HasCartoon() { return cartoon; }
            public bool HasScene() { return scene; }

            public int GetQuestNumber() { return key; }
            public void SetQuestState(QuestState state)
            {
                State = state;
                if (stateChangeAction != null)
                    stateChangeAction.Invoke(this);
            }
            public int GetQuestCategory() { return questCategory; }
            public QuestState GetQuestState()
            {
                return State;
            }
            public string GetQuestTitle() { return questTitle; }
            public string GetQuestContent() { return questContent; }
            public string GetRewardString()
            {
                string text = "";
                if (rewardBoong != 0 && rewardHeart == 0) text = "보상 : " + rewardBoong + "붕";
                else if (rewardBoong == 0 && rewardHeart != 0) text = "보상 : " + "하트" + rewardHeart + "개";
                else if (rewardHeart != 0 && rewardBoong != 0) text = "보상 : " + rewardBoong + "붕, 하트 " + rewardHeart + "개";
                else text = "보상 없음";

                return text;
            }

            public int GetBoongReward() { return rewardBoong; }
            public int GetHeartReward() { return rewardHeart; }

 
            public List<ImageDialogData.Info> GetImageDialogDatas() { return CSVManager.imageDialogData.GetInfos(key); }
            public List<Sprite> GetSummarySprites() { return summarySprites; }
            public Sprite GetCartoonSprite() { return cartoonSprite; }


        }

        readonly private Dictionary<int, Info> infos = new Dictionary<int, Info>();

        public Info GetInfo(int key)
        {
            return infos[key];
        }
        public Dictionary<int, Info>.Enumerator GetInfoEnumerator()
        {
            return infos.GetEnumerator();
        }
        public override void Parsing(List<Dictionary<string, string>> csvDatas)
        {
            for(int idx = 1; idx < csvDatas.Count; idx++)
            {
                Info info = null;
                if (!infos.TryGetValue(idx, out info))
                {
                    
                    //category int
                    int category = ParseInt(csvDatas[idx]["category"]);
                    //title string
                    string title = csvDatas[idx]["title"];
                    //content string
                    string content = csvDatas[idx]["content"];
                    //story string to bool
                    bool story = csvDatas[idx]["story"] == "t" ? true : false;
                    //cartoon string to bool
                    bool cartoon = csvDatas[idx]["cartoon"] == "t" ? true : false;
                    //scene string to bool
                    bool scene = csvDatas[idx]["scene"] == "t" ? true : false;
                    //boong int
                    int boong = ParseInt(csvDatas[idx]["boong"]);
                    //heart int
                    int heart = ParseInt(csvDatas[idx]["heart"]);
                    //skin_powder int
                    int skin_powder = ParseInt(csvDatas[idx]["skin_powder"]);
                    //block_powder int
                    int block_powder = ParseInt(csvDatas[idx]["block_powder"]);
                    //item string
                    string item = csvDatas[idx]["item"];

                   
                    info = new Info(idx, category, title, content, story, cartoon, scene, boong, heart);

                    infos.Add(idx, info);
                }
                else
                {
                    Debug.LogErrorFormat("already exist data, key: {0}", idx);
                }
            }
        }

        public override void ClearData()
        {
            infos.Clear();
        }
    }

}
