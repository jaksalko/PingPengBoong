using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager_Level : UIScript
{
	public GameObject islandList;
	public GameObject islandListContents;
	public List<string> isLandNameList;
	public GameObject[] levelList;
	public Sprite clearBtn;
	public Sprite nonclearBtn;
	public Sprite clearSelect;
	public Sprite nonclearSelect;
	private int highLevel;
	public TutorialManager tutorialManager;

	public RectTransform content; //2160~-2160
	public Transform[] stageScrollView;
	
	public StagePopup stagePopup;
	// Start is called before the first frame update
	void Start()
    {

		

	}

	public void PressBackBtn()
	{
		SceneManager.LoadScene("MainScene");
	}

	


}
