using System.Collections;
using System.Collections.Generic;// let us use lists
using UnityEngine;
using System.Xml;               // basic xml attributes
using System.Xml.Serialization; // access xmlserializer
using System.IO;                //file management
using System.Text;
using UnityEngine.UI;
using System;
using Data;

public class XMLManager : MonoBehaviour
{
    bool paused = false;
    public delegate void IsExist(bool isExist);

    public static XMLManager ins = null;//terrible singleton pattern
                                        // Use this for initialization
    private void Awake()
    {
        Debug.Log("XMLManager awake");

        if (ins == null)
        {
            Debug.Log("instance is null");
            ins = this;
        }
        else if (ins != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    //list of items 
    public Database database;

    public void CreateXML()
    {

        /*
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "utf-8", "yes"));

        XmlNode rootNode = xmlDocument.CreateNode(XmlNodeType.Element , "Database", string.Empty);
        xmlDocument.AppendChild(rootNode);

        XmlNode childNode = xmlDocument.CreateNode(XmlNodeType.Element, "User", string.Empty);
        rootNode.AppendChild(childNode);
        */
        Database database = new Database(SystemInfo.deviceUniqueIdentifier);

        XmlSerializer serializer = new XmlSerializer(typeof(Database));
        using (FileStream fileStream = new FileStream(Application.dataPath + "/Resources/XML/Database.xml", FileMode.Create))
        {
            serializer.Serialize(fileStream, database);
            this.database = database;
        }
        //StreamWriter writer = new StreamWriter("/");

        /*
        [XmlElement("UserInfo")]
        public UserInfo userInfo;

        [XmlElement("UserHistory")]
        public UserHistory userHistory;

        [XmlArray("UserReward")]
        public List<UserReward> userReward;

        [XmlArray("UserStage")]
        public List<UserStage> userStage;

        [XmlArray("UserInventory")]
        public List<UserInventory> userInventory;

        [XmlArray("UserQuest")]
        public List<UserQuest> userQuests;

        [XmlArray("UserRoom")]
        public List<UserRoom> userRooms;
        */

    }
    public void LoadXML()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Database));
        using (FileStream fileStream = new FileStream(Application.dataPath + "/Resources/XML/Database.xml", FileMode.Open))
        {
            try
            {
                database = serializer.Deserialize(fileStream) as Database;
            }
            catch (Exception e)
            {
                throw e;
            }


        }

    }
    public void SaveXML()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Database));
        using (FileStream fileStream = new FileStream(Application.dataPath + "/Resources/XML/Database.xml", FileMode.Create))
        {
            if (database == null)
            {
                throw new System.Exception("Save Data Exception database is null..");
            }
            else
            {
                serializer.Serialize(fileStream, database);
            }

        }
    }



    void OnApplicationQuit()
    {
        SaveXML();
        PlayerPrefs.Save();
    }
    
    void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            paused = true;
            SaveXML();
            PlayerPrefs.Save();
        }
        else
        {
            if(paused)//앱 시작 시 불리는 것을 방지하기 위함
            {
                Count_LogOut_Time();
                paused = false;
            }
        }
    }

    public void Count_LogOut_Time()
    {
        /*
        Debug.Log(database.userInfo.log_out);
        DateTime log_out = DateTime.ParseExact(database.userInfo.log_out, "yyyy-MM-dd HH:mm:ss", null);
        long sec = (long)(DateTime.Now - log_out).TotalSeconds;
        Debug.Log("sec : " + sec);


        if (database.userInfo.heart < 5)
        {
            for (int i = 0; i < sec; i++)
            {

                database.userInfo.heart_time--;
                if (database.userInfo.heart_time <= 0)
                {
                    database.userInfo.heart++;
                    database.userInfo.heart_time = 600;

                    if(database.userInfo.heart == 5)
                    {
                        break;
                    }
                }
            }
        }

        SaveItems();
        PlayerPrefs.Save();
        */
    }
        
}
/*
[Serializable]
public class UserInfo
{
    public string nickname;
    public int boong = 0; // 유저의 붕 갯수
    public int heart = 5; // 유저의 하트 갯수
    public int heart_time = 600; // 하트 충전 타이머
    public int stage_current = 0; // 유저가 깨야하는 스테이지
    public string log_out; //로그 아웃 시간 yyyy/MM/dd HH:mm
    public List<int> star_list; // 유저의 닉네임 (UNIQUE)
    public List<int> move_list; // 유저의 닉네임 (UNIQUE)
    public List<int> reward_list; // 받은 보상 번호

    public int ping_skin_num = 0;//캐릭터 1 스킨착용 기본 검은색
    public int peng_skin_num = 1;//캐릭터 2 스킨착용 기본 분홍 

    public int profile_skin_num = 0;//대표이미지 번호
    public string profile_introduction = "";//자기소개
    public int profile_style_num = 0;//칭호 번호 //0은 없음.

    public List<int> mySkinList; // 보유 스킨 번호 리스트 --> 보유 대표이미지 번호 리스트
    public List<int> myStyleList; // 보유 칭호 번호 리스트

    public int drop_count = 0;
    public int crash_count = 0;
    public int carry_count = 0;
    public int reset_count = 0;
    public int move_count = 0;
    public int snow_count = 0;
    public int parfait_done_count = 0;
    public int crack_count = 0;
    public int cloud_count = 0;

    public int editor_make_count = 0;
    public int editor_clear_count = 0;

    public int boong_count = 0;
    public int heart_count = 0;
    
    public int skin_count = 0;

    public long playTime = 0;
    public int clear_count = 0;
    public int fail_count = 0;

    public bool facebook;
    

}
*/
[Serializable]
public class Database
{
   
    //List<Dictionary<string, object>> data;
    [XmlElement("UserInfo")]
    public UserInfo userInfo;

    [XmlElement("UserHistory")]
    public UserHistory userHistory;

    [XmlArray("UserReward")]
    public List<UserReward> userReward;

    [XmlArray("UserStage")]
    public List<UserStage> userStage;

    [XmlArray("UserInventory")]
    public List<UserInventory> userInventory;

    [XmlArray("UserQuest")]
    public List<UserQuest> userQuests;

    [XmlArray("UserRoom")]
    public List<UserRoom> userRooms;
    //[XmlArray("UserFriend")]
    //public List<UserFriend> userFriend = new List<UserFriend>();

    //EditorMap

    public Database()
    {

    }

    public Database(string deviceID)
    {
        userInfo = new UserInfo(deviceID, null);
        userHistory = new UserHistory(deviceID);

        userInventory = new List<UserInventory>();
        userInventory.Add(new UserInventory(deviceID, 1, CSVManager.skinData.GetInfo(1).skinName));
        userInventory.Add(new UserInventory(deviceID, 2, CSVManager.skinData.GetInfo(2).skinName));

        userRooms = new List<UserRoom>();
        userRooms.Add(new UserRoom(1, true, 0, 0, 0, 0, 0));
        userRooms.Add(new UserRoom(2, true, 0, 0, 0, 0, 0));
        userRooms.Add(new UserRoom(3, true, 0, 0, 0, 0, 0));
        userRooms.Add(new UserRoom(4, true, 0, 0, 0, 0, 0));
        userRooms.Add(new UserRoom(5, true, 0, 0, 0, 0, 0));

        userStage = new List<UserStage>();
        userQuests = new List<UserQuest>();
        userReward = new List<UserReward>();

    }

    //첫시작 또는 스테이지 클리어 
    public void SyncWithCSV()
    {
        foreach (var clearStage in userStage)
        {
            using(var e = CSVManager.stageData.GetInfoEnumerator())
            {
                while(e.MoveNext())
                {
                    var data = e.Current.Value;
                    if(data.GetStageName() == clearStage.stage_name)
                    {
                        data.ClearStage(clearStage.stage_star);
                        break;
                    }
                }
            }
            
        }

        using (var e = CSVManager.islandData.GetInfoEnumerator())
        {
            while (e.MoveNext())
            {
                var data = e.Current.Value;
                data.SetMyStar();
                
            }
        }

        

        for (int i = 0; i < userInventory.Count; i++)
        {
            using (var e = CSVManager.skinData.GetInfoEnumerator())
            {
                while (e.MoveNext())
                {
                    var data = e.Current.Value;
                    if (data.key == userInventory[i].itemIdx)
                    {
                        data.inPossession = true;
                        data.skin_get_time = DateTime.ParseExact(userInventory[i].time_get, "yyyy-MM-dd HH:mm:ss", null);
                    }
                }
            }

            
        }

        foreach (var userQuest in userQuests)
        {
            //QuestManager의 questDictionary 의 데이터를 유저의 퀘스트 데이터를 통해 업데이트
            QuestData.Info questData = CSVManager.questData.GetInfo(userQuest.quest_number);
            questData.SetQuestState(Parser.ParseEnum<QuestState>(userQuest.quest_state.ToString()));
        }
    }

    public void InitializeInfo()//NewGame
    {
        Debug.Log("New User");
        userInfo = new UserInfo(SystemInfo.deviceUniqueIdentifier , null);
        userHistory = new UserHistory(SystemInfo.deviceUniqueIdentifier);

        userInventory.Add(new UserInventory(SystemInfo.deviceUniqueIdentifier, 1,CSVManager.skinData.GetInfo(1).skinName));
        userInventory.Add(new UserInventory(SystemInfo.deviceUniqueIdentifier, 2,CSVManager.skinData.GetInfo(2).skinName));

        userRooms.Add(new UserRoom(1, true, 0, 0, 0, 0, 0));
        userRooms.Add(new UserRoom(2, true, 0, 0, 0, 0, 0));
        userRooms.Add(new UserRoom(3, true, 0, 0, 0, 0, 0));
        userRooms.Add(new UserRoom(4, true, 0, 0, 0, 0, 0));
        userRooms.Add(new UserRoom(5, true, 0, 0, 0, 0, 0));

        XMLManager.ins.SaveXML();
    }

    public void InitializeHistory(string nickname)
    {
        userHistory.nickname = nickname;
    }
    
    public IEnumerator StartTimer()
    {
        float wait_second = 1f;
        int play_sec = 0;
        while(true)
        {
            play_sec++;
            if(play_sec == 60)
            {
                play_sec = 0;
                userHistory.play_time++;
            }

            if(userInfo.heart < 5)
            {
                userInfo.heart_time -= 1;
                if(userInfo.heart_time == 0)
                {
                    userInfo.heart++;
                    userInfo.heart_time = 600;
                    XMLManager.ins.SaveXML();
                    
                }
            }
            else
            {
                userInfo.heart_time = 600;
            }
            //Debug.Log("heart time " + user.heart_time);
            

            yield return new WaitForSeconds(wait_second);
        }
    }
      
    

}