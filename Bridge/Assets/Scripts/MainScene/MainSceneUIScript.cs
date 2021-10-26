using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Data;

public class MainSceneUIScript : UIScript
{
	
	

	public Button profileButton;
	public Button editorLobbyButton;
	public GameObject[] lowerButtons;
	
	SoundManager soundManager = SoundManager.instance;
	public TutorialManager tutorialManager;


	public Image fader;
	public float fadeOutTime;

	public GameObject comingsoonPanel;
	public GameObject snowEffect;

	public IslandRoadUIPrefab islandRoadUIPrefab;//스테이지 선택 화면의 유아이 프래팹
	public IslandUIPrefab islandUIPrefab;//메인화면의 섬 유아이 프리팹

	public Transform islandUIParent;
	public Transform islandRoadContent;

	public Button startButton;

	[Header("Island Text")]
	public Text boong_text;
	public Text heart_text;
	public Text heartTime_text;


	[Header("POPUP")]
	public GameObject settingPopup;
	public QuestPopup questPopup;
	
	public ProfilePopup profilePopup;
	public StagePopup stagePopup;
	public SkinInfoPopup infoPopup;

	

	[Header("Panel")]
	public Gatcha gatcha;
	public Store store;
	public MyIgloo myIgloo;
	public StarGuideBookPopup starGuideBookPopup;
	public EditorPanel editorPanel;
	public GameObject decorationPanel;

	List<GameObject> mainscenePanels = new List<GameObject>();


	private void Start()
	{
		
		int bannerID = Random.Range(0, Constants.BANNER_ANDROID.Length);
		
#if UNITY_ANDROID
		MoPub.RequestBanner(Constants.BANNER_ANDROID[bannerID], MoPub.AdPosition.TopCenter, MoPubBase.MaxAdSize.ScreenWidthHeight90);
		MoPub.ShowBanner(Constants.BANNER_ANDROID[bannerID], true);
#elif UNITY_EDITOR
#else
		MoPub.RequestBanner(Constants.BANNER_IOS[bannerID], MoPub.AdPosition.TopCenter, MoPubBase.MaxAdSize.ScreenWidthHeight90);
		MoPub.ShowBanner(Constants.BANNER_IOS[bannerID], true);
#endif


		soundManager = SoundManager.instance;

		//QuestManager.questDelegate(1, QuestState.OnProgress);
		QuestManager.questDelegate(6, QuestState.OnProgress);//첫 퀘스트 시작
		TutorialManager.instance.tutorialCanvas.gameObject.SetActive(false);
		/*
		if(CSVManager.questData.GetInfo(6).GetQuestState() == QuestState.OnProgress || CSVManager.questData.GetInfo(6).GetQuestState() == QuestState.Watched)
        {
			TutorialManager.instance.tutorialCanvas.gameObject.SetActive(true);
			TutorialManager.instance.helpText.text = "Press Start Button to play";
			TutorialManager.instance.helpText.rectTransform.anchoredPosition
				= startButton.GetComponent<RectTransform>().anchoredPosition + new Vector2(100,-100);

		}
		else if(CSVManager.questData.GetInfo(5).GetQuestState() == QuestState.OnProgress || CSVManager.questData.GetInfo(5).GetQuestState() == QuestState.Watched)
        {
			TutorialManager.instance.tutorialCanvas.gameObject.SetActive(true);
			TutorialManager.instance.helpText.text = "Press Start Button to play";
			TutorialManager.instance.helpText.rectTransform.anchoredPosition
				= startButton.GetComponent<RectTransform>().anchoredPosition + new Vector2(100, -100);
		}
		else
        {
			TutorialManager.instance.tutorialCanvas.gameObject.SetActive(false);
		}
		*/

		mainscenePanels.Add(gatcha.gameObject);
		mainscenePanels.Add(store.gameObject);
		mainscenePanels.Add(myIgloo.gameObject);
		mainscenePanels.Add(starGuideBookPopup.gameObject);
		mainscenePanels.Add(editorPanel.gameObject);
		mainscenePanels.Add(decorationPanel);
	}

	void InActivePanels()
    {
		foreach(var obj in mainscenePanels)
        {
			obj.SetActive(false);
        }
    }

	private void Update()
	{
		boong_text.text = xmlManager.database.userInfo.boong.ToString();
		/*
		boong_text.text = awsManager.userInfo.boong.ToString();
		if(awsManager.userInfo.heart > 5)
        {
			
			heart_text.text = "<color=#e34949>"+awsManager.userInfo.heart+ "</color>" + "/5";
		}
		else
        {
			heart_text.text = awsManager.userInfo.heart + "/5";
		}
		
		
		heartTime_text.text = IntToTimerString();
		*/
	}

	string IntToTimerString()
	{
		string time_string = "";
		int heart_time = awsManager.userInfo.heart_time;
		int min = 0;
		int sec = 0;
		while (heart_time != 0)
		{
			if (heart_time >= 60)
			{
				heart_time -= 60;
				min++;
			}
			else
			{
				sec = heart_time;
				heart_time = 0;
			}
		}
		time_string = min + ":" + sec;
		return time_string;
	}


/// <summary>
/// 패널 오픈
/// </summary>


	public void MyEglooButtonClicked()
	{
		InActivePanels();
		myIgloo.gameObject.SetActive(true);

	}
	public void StoreButtonClicked()
	{
		InActivePanels();
		store.gameObject.SetActive(true);
		
	}
	public void GatchaButtonClicked()
    {
		StartCoroutine(comingsoonPanelActivate());
		/*
		InActivePanels();
		gatcha.gameObject.SetActive(true);
		*/
	}
	/*
	public void PressIslandBtn(int islandNumber)
	{
		InActivePanels();
		stageSelectTab.ActivateStageSelectTab(islandNumber);
	}
	*/
	public void EditorButtonClicked()
    {
		InActivePanels();
		editorPanel.gameObject.SetActive(true);
    }

/// <summary>
/// 팝업 오픈 
/// </summary>
	public void SettingButtonClicked()
	{
		settingPopup.SetActive(true);
	}
	public void ProfileButtonClicked()
	{
		profilePopup.Activate();
	}
	public void ActiveStarGuideBookPopup(int islandNumber)
    {
		
		starGuideBookPopup.gameObject.SetActive(true);
		starGuideBookPopup.TagButtonClicked(islandNumber);

	}

	public void ActiveQuestPopup()
	{
		questPopup.ActivatePopup();
	}

	IEnumerator comingsoonPanelActivate()
	{
		comingsoonPanel.SetActive(true);
		float t = 0;
		while (t < 2)
		{
			t += Time.deltaTime;
			yield return null;
		}

		comingsoonPanel.SetActive(false);

		yield break;
	}
}
