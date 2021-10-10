using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using System;

using Amazon;
using Amazon.CognitoIdentity;

using System.Text;
using System.Security.Cryptography;



public class LoadingScene : MonoBehaviour
{
    bool isAuth;
    string facebookUserId;
    public Text id;
    public Image fade;
    public Image title;
    public float minSize;
    public float maxSize;

    public InputField nickname;
    public Text addAccountText;
    public GameObject accountPanel;

    public Button make_account_button;
    public Button play_button;

    public Button facebook_login_button;
    public Button guest_login_button;

    AWSManager awsManager = AWSManager.instance;
    XMLManager xmlManager = XMLManager.ins;
    JsonAdapter jsonAdapter = JsonAdapter.instance;

    public Button pressAnyButton;

    private void Awake()
    {
        
        StartCoroutine(Interpolation());//Animation Effect
        /*
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });


        */
    }
   
    void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("tutorial", 0));
        PlayerPrefs.SetInt("tutorial", 99);

        awsManager = AWSManager.instance;
        xmlManager = XMLManager.ins;
        jsonAdapter = JsonAdapter.instance;

        //서버 로그인 
        //LoginWithUserState();//call back GetWebRequestCallback
        /*
        var allUserRequest = jsonAdapter.GetAllUserInfo();
        var allEditorMapRequest = jsonAdapter.GetAllEditorMap();

        jsonAdapter.ReadyRequest(allUserRequest);
        jsonAdapter.ReadyRequest(allEditorMapRequest);
        */

        LoginWithLocalData();


    }

    //로컬 로그인
    void LoginWithLocalData()
    {
        xmlManager.LoadItems();//if xml file not exist create it.
        xmlManager.database.SyncWithCSV();
    }

    
    /*
    void FacebookNotLoggedInCallback(ILoginResult result)//처음 앱을 사용할 경우 또는 다시 앱을 설치했을 경우
    {
        if (FB.IsLoggedIn)//success get token
        {

            Debug.Log("You get Access Token : " + AccessToken.CurrentAccessToken.UserId);
            awsManager.AddLogin_To_Credentials(AccessToken.CurrentAccessToken.TokenString);
            jsonAdapter.GetUserInfo(xmlManager.database.userInfo.nickname);
        }
        else//error...
        {
            Debug.Log("FB Login error");
        }
    }
    void Callback_Load_UserInfo(bool isExist)
    {
        //xml은 언제나 최신 업데이트라는 가정.
        //xml -> dynamodb

    }
    */

    #region 로그인 버튼
    public void SignUpGuest()
    {
        isAuth = false;
        accountPanel.SetActive(true);
        
    }
    
    #endregion

    public void AddAccount()//Account Panel
    {

        string nickname_regex = "^[a-zA-Z가-힣0-9]{1}[a-zA-Z가-힣0-9\\s]{0,6}[a-zA-Z가-힣0-9]{1}$";
        Regex regex = new Regex(nickname_regex);

        for (int i = 0; i < awsManager.allUserInfo.Count; i++)
        {
            if (nickname.text == awsManager.allUserInfo[i].nickname)
            {
                addAccountText.text = "이미 존재하는 닉네임입니다";
                return;
            }
        }

        if (regex.IsMatch(nickname.text))
        {
            UserInfo userInfo = new UserInfo(nickname.text, facebookUserId);
            UserHistory userHistory = new UserHistory(nickname.text);
            UserInventory skin0 = new UserInventory(nickname.text, CSVManager.skinData.GetInfo(1).skinName);
            UserInventory skin1 = new UserInventory(nickname.text, CSVManager.skinData.GetInfo(2).skinName);

            UserAccountCreate userAccount = new UserAccountCreate(userInfo, userHistory, skin0, skin1);
            var request = jsonAdapter.POST_DATA(userAccount, "newUser/create", (isConnect) => {
                if(isConnect)
                {
                    PlayerPrefs.SetString("nickname", nickname.text);

                    if (facebookUserId != null)
                        PlayerPrefs.SetString("Login", "facebook");
                    else
                        PlayerPrefs.SetString("Login", "guest");

                    Login();
                }
                else
                {

                }


            });

            jsonAdapter.ReadyRequest(request);
        }
        else
        {
            addAccountText.text = "한글,영어,숫자 포함 최소 2자, 최대 8자입니다";
        }
    }

   
    public void PressAnyButtonClicked()
    {
        GameStart();
        /*
        if(PlayerPrefs.GetString("Login", "none") == "none")
        {
            SignUpGuest();
        }
        else
        {
            GameStart();
        }
        */
    }
    

    public void GameStart()
    {
        if(PlayerPrefs.GetInt("intro",0) == 0)
        {
            LoadingSceneManager.instance.LoadScene("IntroScene", true);
        }
        else
        {
            LoadingSceneManager.instance.LoadScene("MainScene", true);
        }
        
    }
    
    IEnumerator Interpolation()
    {
        float t = 0;
        while (true)
        {
            t += Time.deltaTime *1.2f;
            float interpolation = Mathf.Abs(Mathf.Sin(t));

            float size = Mathf.Lerp(minSize, maxSize, interpolation);
            title.transform.localScale = new Vector3(size, size, 1);
            yield return null;
        }

    }


   

    void Login()
    {
        Debug.Log("Login");
        accountPanel.SetActive(false);
        //facebook_login_button.gameObject.SetActive(false);
        //guest_login_button.gameObject.SetActive(false);

        awsManager.Count_LogOut_Time();
        awsManager.StartCoroutine(awsManager.StartTimer());

        pressAnyButton.interactable = true;

        id.text = "UID : " + awsManager.userInfo.nickname;

    }
}
