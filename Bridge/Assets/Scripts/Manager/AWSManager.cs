using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

using UniRx;
using UniRx.Triggers;

public class AWSManager : MonoBehaviour
{
    bool paused = false;
    public static AWSManager instance = null;

    JsonAdapter jsonAdapter;

    private CognitoAWSCredentials _credentials;
    private CognitoAWSCredentials Credentials
    {
        get
        {
            if (_credentials == null)
                _credentials = new CognitoAWSCredentials(PrivateData.identitiy_pool_id, RegionEndpoint.APNortheast2);
            return _credentials;
        }
    }//return CognitoAWSCredentials

    
    AmazonDynamoDBClient dbClient;
    DynamoDBContext dbContext;

    public delegate void LoadUserCallback(bool isLoad);
    public delegate void CreateUserCallback(bool success);

    public delegate void BooleanCallback(bool callback);


    public List<UserInfo> allUserInfo;

    public UserInfo userInfo;
    public UserHistory userHistory;

    
    public List<UserReward> userReward;
    public Dictionary<string,UserStage> userStage = new Dictionary<string, UserStage>();
    public List<UserInventory> userInventory;
    public List<UserFriend> userFriend;
    public List<CustomMapItem> editorMap;
    public List<Mailbox> mailbox;
    public List<UserQuest> userQuests;


    public Transform customMapList;
   

    void Awake()
    {
        
        if (instance == null)
        {
            Debug.Log("Single instance is null");
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Single instance is not Single.. Destroy gameobject!");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//Dont destroy this singleton gameobject :(

        UnityInitializer.AttachToGameObject(this.gameObject); // Amazon Initialize
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest; //Bug fix code

        Credentials.GetIdentityIdAsync(delegate (AmazonCognitoIdentityResult<string> result) {
            if (result.Exception != null)
            {
                Debug.Log(result.Exception);//Exception!!
            }


            else
            {
                Debug.Log("GetIdentityID" + result.Response);
            }
        });

        dbClient = new AmazonDynamoDBClient(Credentials, RegionEndpoint.APNortheast2);
        dbContext = new DynamoDBContext(dbClient);
        
       
        //credentials.ClearIdentityCache();
        //credentials.ClearCredentials();        
    }

    private void Start()
    {
        jsonAdapter = JsonAdapter.instance;
      
    }

    public void AddLogin_To_Credentials(string token)
    {
        Credentials.AddLogin ("graph.facebook.com", token);
    }

    public void Count_LogOut_Time()
    {
        Debug.Log(userInfo.log_out);
        DateTime log_out = DateTime.ParseExact(userInfo.log_out, "yyyy-MM-dd HH:mm:ss", null);
        long sec = (long)(DateTime.Now - log_out).TotalSeconds;
        Debug.Log("sec : " + sec);


        if (userInfo.heart < 5)
        {
            for (int i = 0; i < sec; i++)
            {

                userInfo.heart_time--;
                if (userInfo.heart_time <= 0)
                {
                    userInfo.heart++;
                    userInfo.heart_time = 600;

                    if (userInfo.heart == 5)
                    {
                        break;
                    }
                    var request = jsonAdapter.POST_DATA(new InfoHistory(userInfo,userHistory), "infoHistory/update", (isConnect) => {

                    });

                    jsonAdapter.ReadyRequest(request);
                }
            }
        }

        
    }

    public IEnumerator StartTimer()//하트 타이머
    {
        float wait_second = 1f;
        int play_sec = 0;
        while (true)
        {
            play_sec++;
            if (play_sec == 60)
            {
                play_sec = 0;
                userHistory.play_time++;
            }

            if (userInfo.heart < 5)
            {
                userInfo.heart_time -= 1;
                if (userInfo.heart_time == 0)
                {
                    userInfo.heart++;
                    userInfo.heart_time = 600;
                    var request = jsonAdapter.POST_DATA(new InfoHistory(userInfo, userHistory), "infoHistory/update", (isConnect) => {

                    });

                    jsonAdapter.ReadyRequest(request);
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

    void OnApplicationQuit()
    {
        SaveData();
        PlayerPrefs.Save();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            paused = true;
            SaveData();
            PlayerPrefs.Save();
        }
        else
        {
            if (paused)//앱 시작 시 불리는 것을 방지하기 위함
            {
                Count_LogOut_Time();
                paused = false;
            }
        }
    }

    void SaveData()
    {
        InfoHistory infoHistory = new InfoHistory(userInfo, userHistory);
        userInfo.log_out = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var request = jsonAdapter.POST_DATA(infoHistory, "infoHistory/update", (isConnect)=> {

        });

        jsonAdapter.ReadyRequest(request);
        
    }

    /*
    public void CreateEditorMap(Map map , int moveCount,int dif , BooleanCallback callback)
    {
        List<int> dynamoDB_datas = new List<int>();
        for (int i = 0; i < map.datas.Count; i++)
        {
            for(int j = 0; j < map.datas[i].Count; j++)
            {
                dynamoDB_datas.Add(map.datas[i][j]);
            }

           
        }

        List<int> dynamoDB_styles = new List<int>();
        for (int i = 0; i < map.styles.Count; i++)
        {
            for (int j = 0; j < map.styles[i].Count; j++)
            {
                dynamoDB_styles.Add(map.styles[i][j]);
            }


        }

        EditorStage editorMap = new EditorStage
        {
            

        

            map_id = GameManager.instance.user_aws.nickname + " " + DateTime.Now.ToString("yyyyMMddHHmmss"),
            //map_id = map.map_title,
            maker = GameManager.instance.user_aws.nickname,
            title = map.map_title,
            make_time = DateTime.Now.ToString("yyyyMMddHHmmss"),
            play_count = 0,
            like = 0,

            height = map.mapsizeH,
            width = map.mapsizeW,

            
            datas = Parser.ListToString(dynamoDB_datas),
            styles = Parser.ListToString(dynamoDB_styles),

            isParfait = map.parfait,

            step = moveCount,
            level = dif,
            star_limit = new List<int>() { moveCount, moveCount * 2, moveCount * 3 }

             

        };

        dbContext.SaveAsync(editorMap, (result) => {
            if (result.Exception == null)
            {
                //this = user;

                Debug.Log("Success saved editormap");
                callback(true);

            }
            else
            {
                Debug.LogWarning("EditorStage Save Exception : " + result.Exception);
                callback(false);
            }


        });



    }
    */






}
