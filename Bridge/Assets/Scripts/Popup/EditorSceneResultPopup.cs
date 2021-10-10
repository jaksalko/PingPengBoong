using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Text.RegularExpressions;
using Data;
public class EditorSceneResultPopup : MonoBehaviour
{
    public Text warningText;
    public Text moveCount;
    public Image levelImage;

    public InputField titleInputField;
   
    int move;
    int dif;


    public void ShowResultPopup(int count , int level)
    {
        move = count;
        dif = level;

        moveCount.text = "x" + count;
        levelImage.sprite = Resources.Load<Sprite>("Icon/Level/" + dif);

        
        gameObject.SetActive(true);
    }

    public void ModifyButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void MakeCustomStageClicked()
    {
       
        string title_regex = "^[a-zA-Z가-힣0-9]{1}[a-zA-Z가-힣0-9\\s]{0,6}[a-zA-Z가-힣0-9]{1}$";
        Regex regex = new Regex(title_regex);

        if(regex.IsMatch(titleInputField.text))
        {
            
            Map newMap = GameController.instance.GetMap();//GetStage
            StageData.Info stageData = new StageData.Info(-1,titleInputField.text.ToString(), 0, 0, false, 3, 0, newMap,null);
            

            EditorStage editorMap = new EditorStage(stageData : stageData, nick : AWSManager.instance.userInfo.nickname , moveCount : move );
            UserHistory deepCopyHistory = AWSManager.instance.userHistory.DeepCopy();
            deepCopyHistory.editor_make++;

            MakeEditorMapData makeEditorMapData = new MakeEditorMapData(deepCopyHistory, editorMap);

            var request = JsonAdapter.instance.POST_DATA(makeEditorMapData, "editorMap/insert", (isConnect) => {
                if(isConnect)
                {
                    SceneManager.LoadScene("MainScene");
                }
                else
                {

                }
            });

            JsonAdapter.instance.ReadyRequest(request);
           

        }
        else
        {
            Debug.Log("no match");
            titleInputField.text = "";
            
            StopAllCoroutines();
            StartCoroutine(WarningText());
            

        }

    }

    IEnumerator WarningText()
    {
        warningText.gameObject.SetActive(true);
        float time = 0;

        Color warningTextColor = warningText.color;

        while(time <= 3)
        {
            time += Time.deltaTime;
            warningTextColor.a = time * 2;
            warningText.color = warningTextColor;
            yield return null;
        }
        warningText.gameObject.SetActive(false);
        yield break;
  
    }
}
