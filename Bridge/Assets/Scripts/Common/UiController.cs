﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Triggers;



public class UiController : UIScript
{
    public GameObject inGame;

    public GameObject pausePopup;
    public GameObject settingPopup;
    public Text pauseStageText;
    public Button pausePopup_retryButton;

    int order = 0;
    public GameObject[] parfaitOrder;
    public GameObject[] parfaitOrder_done;

    
    public GameObject mission_parfait;

    public StageSceneResultPopup stageSceneResultPopup;
    public CustomsSceneResultPopup customSceneResultPopup;
    public EditorSceneResultPopup editorSceneResultPopup;

	public Button nextLevelBtn;
    
    public Text devtext;
    public Text remainText;
    public Text moveText;

    bool mini = false;

    public Button player1;
    public Button player2;


    public Button revertButton;


    public StarSlider starSlider;
    List<int> star_limit = new List<int>();

    private bool warning = false;

    private void Start()
    {
        devtext.text = "platform : " + Application.platform + "\n" + "level : " + PlayerPrefs.GetInt("level", 0);

        //player1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/SkinData/" + awsManager.userInfo.skin_a);
        //player2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/SkinData/" + awsManager.userInfo.skin_b);
        player1.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/Skin/" + xmlManager.database.userInfo.skin_a);
        player2.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icon/Skin/" + xmlManager.database.userInfo.skin_b);
        SpriteState spriteState = new SpriteState();

        spriteState = player1.spriteState;
        spriteState.disabledSprite = Resources.Load<Sprite>("Icon/Skin/" + xmlManager.database.userInfo.skin_a + "_on");
        player1.spriteState = spriteState;

        spriteState = player2.spriteState;
        spriteState.disabledSprite = Resources.Load<Sprite>("Icon/Skin/" + xmlManager.database.userInfo.skin_b + "_on");
        player2.spriteState = spriteState;

        player1.interactable = false;
        player2.interactable = true;

        Color c = player2.GetComponent<Image>().color;
        c.a = 0.4f;
        player2.GetComponent<Image>().color = c;

        c = player1.GetComponent<Image>().color;
        c.a = 1f;
        player1.GetComponent<Image>().color = c;

        var spaceStream = this.UpdateAsObservable()
           .Where(_ => Input.GetKeyDown(KeyCode.Space))
           .Where(_ => GameController.Playing)
           .Select(playerNumber => GameController.instance.player1.isActive ? 2 : 1)
           .Where(playerNumber => (playerNumber == 1 && player1.interactable) || (playerNumber == 2 && player2.interactable))
           .Subscribe(playerNumber => {ChangeCharacter(playerNumber);});

        var tutorialSpaceStream = this.UpdateAsObservable()
          .Where(_ => Input.GetKeyDown(KeyCode.Space))
          .Where(_ => !GameController.Playing)
          .Where(_ => !TutorialManager.instance.inScript)
          .Select(playerNumber => GameController.instance.player1.isActive ? 2 : 1)
          .Where(playerNumber => (playerNumber == 1 && GameController.instance.ui.player1.interactable) || (playerNumber == 2 && GameController.instance.ui.player2.interactable))
          .Subscribe(playerNumber => { GameController.instance.ui.ChangeCharacter(playerNumber); });

    }

    public void GameEnd(bool isSuccess, int star,int remain_snow, int moveCount, bool custom , bool editor)
    {
        //inGame.SetActive(false);
        //SetMoveCountText(moveCount);

        //infinite --> 종료 팝업 선택 버튼 : 다음 맵 / 로비로?
        //editor --> 종료 팝업 선택 버튼 : 생성할지 말지
        //Default --> 종료 팝업 선택 버튼 : 다음 스테이지 / 로비로
        if (custom)
        {
            customSceneResultPopup.ShowResultPopup(isSuccess, remain_snow, moveCount, star_count: star, gameManager.retry);
        }
        else if(editor)
        {
            int level = (moveCount / 5) + 1;
            if (level > 5) level = 5;

            editorSceneResultPopup.ShowResultPopup(moveCount, level);
        }
        else
        {
            stageSceneResultPopup.ShowResultPopup(isSuccess,remain_snow, moveCount , star_count : star);
        }
       
    }

    #region 인게임 UI
    public void SetRemainText(int remain, int total)//inGame UI
    {
        remainText.text = remain + "/" + total;
    }
    public void SetMoveCountText(int count , int max)//Result UI
    {
        Debug.Log(count +"," + max);
        Debug.Log(starSlider);
        starSlider.SetSliderValue(count);
        int remain_move = max - count;
        moveText.text = remain_move + "/" + max;
    }
    public void SetSlider(int[] star_list , int maxValue)
    {
        starSlider.SetSlider(star_list, maxValue);
        
    }
    public void ParfaitDone(int order)
    {
        Debug.Log("done");
        for(int i = 0; i < parfaitOrder.Length; i++)
        {
            if(i < order)
            {
                parfaitOrder[i].SetActive(false);
                parfaitOrder_done[i].SetActive(true);
            }
            else
            {
                parfaitOrder[i].SetActive(true);
                parfaitOrder_done[i].SetActive(false);
            }
        }
        
        
    }
    
    #endregion

    #region 결과 창 UI
    
    public void Pause()
    {
        if(GameController.Playing)
        {
            GameController.instance.SetPlaying(false);
            pauseStageText.text = "STAGE " + gameManager.stageDataOnPlay.GetStageText();

            /*
            if (AWSManager.instance.userInfo.heart == 0)
            {
                pausePopup_retryButton.interactable = false;
            }
            */
            pausePopup.SetActive(true);
        }
        
    }

    public void SettingButtonClicked()
    {
        if (GameController.Playing)
        {
            GameController.instance.SetPlaying(false);
            settingPopup.SetActive(true);
        }
            
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        /*
        UserInfo copyInfo = awsManager.userInfo.DeepCopy();
        UserHistory copyHistory = awsManager.userHistory.DeepCopy();

        copyInfo.heart--;
        copyHistory.heart_use++;
        InfoHistory startStage = new InfoHistory(copyInfo, copyHistory);

        var request = jsonAdapter.POST_DATA(startStage, "infoHistory/update", (isConnect) => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

        jsonAdapter.ReadyRequest(request);
        */
    }
  
    public void GoLobby()
    {
        SceneManager.LoadScene("MainScene");
        
    }

    public void Resume()
    {
        GameController.instance.SetPlaying(true);
        pausePopup.SetActive(false);
    }
    #endregion


    public void MiniMapButton()
    {
        Player now = GameController.instance.nowPlayer;

        if (!now.Moving() && !now.other.Moving())
        {
            mini = GameController.instance.cameraController.MiniMapView(mini);
            GameController.instance.SetPlaying(!mini);
        }
            
    }

    public void ReturnButton()
    {
        Player now = GameController.instance.nowPlayer;

        if (!now.Moving() && !now.other.Moving())
        {
            GameController.instance.moveCommand.Undo();
        }
    }


    public void ChangeCharacter(int player_num)
    {
        GameController instance = GameController.instance;

        Player now = instance.nowPlayer;

        Player player;
        if(player_num == 1)
        {
            player = instance.player1;
        }
        else
        {
            player = instance.player2;
        }

        if(!now.Moving() && !now.other.Moving())
        {
            now.isActive = false;
            instance.nowPlayer = player;
            player.isActive = true;

            if(player == GameController.instance.player1)
            {
                player1.interactable = false;//1이 활성화된 캐릭터
                player2.interactable = true;

                Color c = player2.GetComponent<Image>().color;
                c.a = 0.4f;
                player2.GetComponent<Image>().color = c;

                c = player1.GetComponent<Image>().color;
                c.a = 1f;
                player1.GetComponent<Image>().color = c;
            }
            else
            {
                player1.interactable = true;
                player2.interactable = false;

                Color c = player1.GetComponent<Image>().color;
                c.a = 0.4f;
                player1.GetComponent<Image>().color = c;

                c = player2.GetComponent<Image>().color;
                c.a = 1f;
                player2.GetComponent<Image>().color = c;
            }
            
        }

		
        
    }
    public void MasterFocus(Player master)
    {
        GameController.instance.nowPlayer = master;
        GameController.instance.nowPlayer.isActive = true;
        Debug.Log("master : " + master.name);Debug.Log("master : " + master.name);
    }
    public void NextLevel()
    {
        int islandNumber = gameManager.stageDataOnPlay.GetIslandNumber();
        string islandName = CSVManager.islandData.GetInfo(islandNumber).GetIslandName();
        //GameController googleinstance level++....
        if (GameController.instance.customMode)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        else
            LoadingSceneManager.instance.LoadScene(islandName, true);
        
    }

    public void ActivateWarningUI()
    {
        if (!warning)
        {
            warning = true;
            StartCoroutine(CoroutineWarningUI());
        }
            
    }
    IEnumerator CoroutineWarningUI()
    {
        Debug.LogWarning("Warning");
        yield break;
    }

   


    
}
