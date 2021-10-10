using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class CustomsSceneResultPopup : UIScript
{
    public TextMeshProUGUI moveCount;
    public TextMeshProUGUI snowCount;
    public Text stageText;

    public Image[] starImage;

    public GameObject successPopup;
    public GameObject failPopup;
    public GameObject successEffect;

    public Button likeButton;

    public void ShowResultPopup(bool isSuccess, int remain_snow, int move_count, int star_count, bool retry)
    {
        if (retry)
            likeButton.gameObject.SetActive(false);
        else
            likeButton.gameObject.SetActive(true);

        stageText.text = gameManager.playEditorStage.title;// gameManager.playEditorMap.map_title;

        if (isSuccess)
        {
            
            successEffect.SetActive(true);
            successPopup.SetActive(true);
            
        }
        else
        {
            failPopup.SetActive(true);
            
        }

        moveCount.text = move_count.ToString();
        snowCount.text = remain_snow.ToString();
        starImage[star_count].gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void GoLobbyButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RetryButtonClicked()
    {
        SceneManager.LoadScene("CustomMapPlayScene");//customMode Scene
    }



    public void LikeButtonClicked()
    {
        
        var request = jsonAdapter.POST_DATA(gameManager.playEditorStage, "editorMap/like/update", (isConnect)=> {
            if(isConnect)
            {
                likeButton.interactable = false;
                gameManager.playEditorStage.likes++;

                CustomMapItem map = awsManager.editorMap.Find(x => x.itemdata.map_id == GameManager.instance.playEditorStage.map_id);
                map.itemdata = GameManager.instance.playEditorStage;
                map.likes.text = map.itemdata.likes.ToString();
            }
            else
            {

            }
        });

        jsonAdapter.ReadyRequest(request);
    }

   
}
