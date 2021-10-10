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

	[Header("Island Text")]
	public Text boong_text;
	public Text heart_text;
	public Text heartTime_text;


	[Header("POPUP")]
	public GameObject settingPopup;
	public QuestPopup questPopup;
	public StarGuideBookPopup starGuideBookPopup;
	public ProfilePopup profilePopup;
	public StagePopup stagePopup;


	

	[Header("Panel")]
	public Gatcha gatcha;
	public Store store;
	public MyIgloo myIgloo;
	public StageSelectTab stageSelectTab;
	public EditorPanel editorPanel;

	List<GameObject> mainscenePanels = new List<GameObject>();

	private void Start()
	{
		
		soundManager = SoundManager.instance;

		QuestManager.questDelegate(1, QuestState.OnProgress);

		using (var e = CSVManager.islandData.GetInfoEnumerator())
		{
			while (e.MoveNext())
			{
				var data = e.Current.Value;
				IslandUIPrefab islandUI = Instantiate(islandUIPrefab);//메인화면의 섬 유아이
				IslandRoadUIPrefab islandRoadUI = Instantiate(islandRoadUIPrefab);//레벨 패널의 섬 길 유아이

				islandUI.SetUIPrefab(data);
				islandUI.SetParentAsLastSibling(islandUIParent);

				islandRoadUI.SetUIPrefab(data);
				islandRoadUI.SetParentAsFirstSibling(islandRoadContent);
				stageSelectTab.SetHeightDictionary(data.GetIslandNumber(), islandRoadUI.GetIslandBackgroundSize());

			}

		}


		

		mainscenePanels.Add(gatcha.gameObject);
		mainscenePanels.Add(store.gameObject);
		mainscenePanels.Add(myIgloo.gameObject);
		mainscenePanels.Add(stageSelectTab.gameObject);
		mainscenePanels.Add(editorPanel.gameObject);
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
	public void PressIslandBtn(int islandNumber)
	{
		InActivePanels();
		stageSelectTab.ActivateStageSelectTab(islandNumber);
	}
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
