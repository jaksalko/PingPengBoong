using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Linq;
using Data;
using UnityEngine.UI;

using UniRx;
using UniRx.Triggers;

using UnityEngine.SceneManagement;

public class JsonAdapter : MonoBehaviour
{

    IEnumerator webRequest = null;
    Queue<IEnumerator> webRequestQueue = new Queue<IEnumerator>();

    public static JsonAdapter instance = null;

    public GameObject loadingCanvas;
    public Text loadingText;
    
    public delegate void BooleanCallback(bool callback);


    public CustomMapItem customMapPrefab;

    AWSManager awsManager;
    CSVManager csvManager;


    private void Awake()
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

    }

    private void Start()
    {
        awsManager = AWSManager.instance;
        csvManager = CSVManager.instance;

    }
    public void ReadyRequest(IEnumerator webRequest)
    {
        webRequestQueue.Enqueue(webRequest);
        ProcessWebRequest();
    }
    void ProcessWebRequest()
    {
        if(webRequestQueue.Count != 0 && webRequest == null)
        {
            loadingCanvas.SetActive(true);
            webRequest = webRequestQueue.Dequeue();
            StartCoroutine(webRequest);
        }
        else
        {
            loadingCanvas.SetActive(false);
        }
    }


    IEnumerator API_GET(string url , Action<bool,string> callback)
    {
        

        int tryCount = 0;
        var waitSecs = new WaitForSeconds(Constants.WebRequestDelayTime);
        UnityWebRequest www = UnityWebRequest.Get(PrivateData.ec2 + url);

        bool connect = false;
        string response = null;

        while (tryCount < Constants.WebRequestTryCount)
        {
            tryCount++;
            loadingText.text = "연결중" + Environment.NewLine + tryCount + "/" + Constants.WebRequestTryCount;
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                connect = false;
                response = www.error;
                Debug.LogWarning(www.error);                
            }
            else if (www.responseCode != 200)
            {
                connect = false;
                response = www.responseCode.ToString();
                Debug.LogWarning("Response Code : " + www.responseCode);
            }
            else
            {
                connect = true;
                response = www.downloadHandler.text;
                Debug.Log("GET WebRequset : " + www.downloadHandler.text);
                break;
            }

            yield return waitSecs;
        }

        callback(connect, response);
        yield break;
    }
    IEnumerator API_POST(string url , string bodyJsonString , Action<bool,string> callback)
    {

        int tryCount = 0;
        var waitSecs = new WaitForSeconds(Constants.WebRequestDelayTime);

        var req = new UnityWebRequest(PrivateData.ec2 + url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(bodyJsonString);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        bool connect = false;
        string response = null;

        while (tryCount < Constants.WebRequestTryCount)
        {
            tryCount++;
            loadingText.text = "연결중" + Environment.NewLine + tryCount + "/" + Constants.WebRequestTryCount;
            yield return req.SendWebRequest();

            if (req.isHttpError || req.isNetworkError)
            {
                Debug.LogWarning(req.error);
                connect = false;
                response = req.error;
            }
            else if (req.responseCode != 200)
            {
                Debug.LogWarning("response code : " + req.responseCode);
                connect = false;
                response = req.responseCode.ToString();

            }
            else
            {
                Debug.Log("GET WebRequset : " + req.downloadHandler.text);
                connect = true;
                response = req.downloadHandler.text;
                break;
            }

            yield return waitSecs;
        }

        callback(connect, response);
        yield break;
    }


    //나의 모든 정보
    public IEnumerator GetAllUserData(string nickname, BooleanCallback booleanCallback)
    {
        CSVManager csvManager = CSVManager.instance;
        AWSManager awsManager = AWSManager.instance;
        bool isConnect = false;
        yield return StartCoroutine(API_GET("getall/get?nickname="+nickname, (connect, response) =>
        {
            isConnect = connect;
            if (connect)
            {
                
                List<string> fixDatas = JsonHelper.FixMultipleJson(response);


                string fixData = JsonHelper.fixJson(fixDatas[0]);
                UserInfo[] userInfos = JsonHelper.FromJson<UserInfo>(fixData);
                awsManager.userInfo = userInfos[0];

                if(awsManager.userInfo.boong >= 4000 && QuestManager.questDelegate != null)
                {
                    //QuestManager.questDelegate(4, QuestState.OnProgress);
                }

                fixData = JsonHelper.fixJson(fixDatas[1]);
                UserHistory[] userHistory = JsonHelper.FromJson<UserHistory>(fixData);
                awsManager.userHistory = userHistory[0];

                fixData = JsonHelper.fixJson(fixDatas[2]);
                UserStage[] userStages = JsonHelper.FromJson<UserStage>(fixData);

                awsManager.userStage.Clear();
                foreach (var clearStage in userStages)
                {
                    awsManager.userStage.Add(clearStage.stage_name, clearStage);
                    CustomMapItem item = AWSManager.instance.editorMap.Find(x => x.itemdata.map_id == clearStage.stage_name);
                    if (item != null)
                        item.SetMining(true);

                    using(var e = CSVManager.stageData.GetInfoEnumerator())
                    {
                        while(e.MoveNext())
                        {
                            var data = e.Current.Value;
                            if(data.GetStageName() == clearStage.stage_name)
                            {
                                data.ClearStage(clearStage.stage_star);
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

                  
                }
               
               

                
                
                fixData = JsonHelper.fixJson(fixDatas[3]);
                UserInventory[] userInventories = JsonHelper.FromJson<UserInventory>(fixData);
                for(int i = 0; i < userInventories.Length; i++)
                {
                    using(var e = CSVManager.skinData.GetInfoEnumerator())
                    {
                        while(e.MoveNext())
                        {
                            var data = e.Current.Value;
                            if(data.skinName == userInventories[i].item_name)
                            {
                                data.inPossession = true;
                                data.skin_get_time = DateTime.ParseExact(userInventories[i].time_get, "yyyy-MM-dd HH:mm:ss", null);
                            }
                        }
                    }
                    
                }
                awsManager.userInventory = userInventories.ToList();

                fixData = JsonHelper.fixJson(fixDatas[4]);
                UserFriend[] userFriends = JsonHelper.FromJson<UserFriend>(fixData);

                for(int i = 0; i < userFriends.Length; i++)
                {
                    DateTime request_time = DateTime.ParseExact(userFriends[i].time_request, "yyyy-MM-dd HH:mm:ss", null);

                    if(DateTime.Now.DayOfYear != request_time.DayOfYear)//하트 초기화
                    {
                        userFriends[i].send = false;
                    }
                    
                }

                awsManager.userFriend = userFriends.ToList();

                fixData = JsonHelper.fixJson(fixDatas[5]);
                UserReward[] userRewards = JsonHelper.FromJson<UserReward>(fixData);
                awsManager.userReward = userRewards.ToList();

                fixData = JsonHelper.fixJson(fixDatas[6]);
                Mailbox[] mailbox = JsonHelper.FromJson<Mailbox>(fixData);
                awsManager.mailbox = mailbox.ToList();

                fixData = JsonHelper.fixJson(fixDatas[7]);
                UserQuest[] userQuests = JsonHelper.FromJson<UserQuest>(fixData);
                foreach(var userQuest in userQuests)
                {
                    //QuestManager의 questDictionary 의 데이터를 유저의 퀘스트 데이터를 통해 업데이트
                    QuestData.Info questData = CSVManager.questData.GetInfo(userQuest.quest_number); 
                    questData.SetQuestState(Parser.ParseEnum<QuestState>(userQuest.quest_state.ToString()));
                }
                awsManager.userQuests = userQuests.ToList();

            }
            else
            {
                Debug.LogError(response);
                //재시도
            }
        }));
        booleanCallback(isConnect);
        webRequest = null;
        ProcessWebRequest();
        yield break;
    }
    //모든 유저의 정보 
    public IEnumerator GetAllUserInfo()
    {

        yield return StartCoroutine(API_GET("allUser/get", (connect, response) =>
        {
            if (connect)
            {
                string fixData = JsonHelper.fixJson(response);
                UserInfo[] userInfos = JsonHelper.FromJson<UserInfo>(fixData);
                AWSManager.instance.allUserInfo = userInfos.ToList();
            }
            else
            {
                Debug.LogError(response);
            }
        }));
        webRequest = null;
        ProcessWebRequest();
        yield break;

    }
    //모든 에디터 맵의 정보 //앱 시작 시 한번만 호출
    public IEnumerator GetAllEditorMap()
    {
        yield return StartCoroutine(API_GET("editorMap/get", (connect, response) =>
        {
            if (connect)
            {
                Transform mapList = AWSManager.instance.customMapList;

                string fixData = JsonHelper.fixJson(response);
                EditorStage[] editorStages = JsonHelper.FromJson<EditorStage>(fixData);
                for(int i = 0; i < editorStages.Length; i++)
                {
                    if(editorStages[i].maker != PlayerPrefs.GetString("nickname", "pingpengboong"))
                    {
                        CustomMapItem newItem = Instantiate(customMapPrefab);
                        newItem.Initialize(editorStages[i]);

                        AWSManager.instance.editorMap.Add(newItem);
                        newItem.transform.SetParent(mapList);
                    }
                    else
                    {
                        //My Map
                    }
                   
                }

            }
            else
            {
                Debug.LogError(response);
            }
        }));
        webRequest = null;
        ProcessWebRequest();
        yield break;
    }

    //데이터베이스 업데이트
    public IEnumerator POST_DATA(JsonData data, string url , BooleanCallback booleanCallback)
    {
        var json = JsonUtility.ToJson(data);
        bool isConnect = false;
        yield return StartCoroutine(API_POST(url, json, (connect, response) => {
            isConnect = connect;
            if (connect)
            {
                Debug.Log("success update data");
                data.CopyToLocal();
            }
            else
            {
                Debug.LogError(response);
            }
        }));

        booleanCallback(isConnect);
        webRequest = null;
        ProcessWebRequest();
        yield break;
    }

}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        Debug.Log(wrapper.Items.Length);

        for(int i = 0; i < wrapper.Items.Length; i++)
        {
            Debug.Log(wrapper.Items[i].ToString());
        }
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
    public static string fixJson(string value)//value : []
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static List<string> FixMultipleJson(string value)
    {
        value = value.Remove(0,1);
        value = value.Remove(value.Length-1,1);

        List<string> jsons = new List<string>();

        for(int i = 0; i < value.Length; i++)
        {
            string json = "";

            if(value[i] == '[')
            {
                while(true)
                {
                    json += value[i];
                    //Debug.Log(json);
                    if (value[i] == ']')
                    {
                        jsons.Add(json);
                        break;
                    }
                    i++;
                    
                    
                }
                
            }
            
        }

        Debug.Log(value);
        return jsons;

    }
   
    



}