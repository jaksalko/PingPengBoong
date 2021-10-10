using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
public class Dialog : MonoBehaviour
{
    protected bool endDialog = false;
    protected bool endTexting = false;

    public GameObject pengDialog;
    public Text pengDialogText;

    public GameObject pingDialog;
    public Text pingDialogText;

    public IEnumerator DialogIEnumerator(List<DialogData> dialogDatas)
    {
        int dialogCount = 0;
        gameObject.SetActive(true);
        while (dialogCount != dialogDatas.Count)
        {
            yield return StartCoroutine(Texting(dialogDatas[dialogCount]));
            dialogCount++;
        }
        gameObject.SetActive(false);
        yield break;
    }

    public IEnumerator Texting(DialogData dialogData)
    {
        endTexting = false;
        Text dialogText;
        if(dialogData.speaker == "ping")
        {
            pengDialog.SetActive(false);
            pingDialog.SetActive(true);
            dialogText = pingDialogText;
        }
        else
        {
            pengDialog.SetActive(true);
            pingDialog.SetActive(false);
            dialogText = pengDialogText;
        }

        dialogText.text = "";
        int textCount = 0;
        while(textCount != dialogData.line.Length)
        {
            if(endTexting)
            {
                string line = dialogData.line;
                line = line.Replace("@", PlayerPrefs.GetString("nickname", "pingpengboong"));
                dialogText.text = line;

                break;
            }
            else
            {
                if (dialogData.line[textCount] == '@')
                    dialogText.text += PlayerPrefs.GetString("nickname", "pingpengboong");
                else
                    dialogText.text += dialogData.line[textCount];

                textCount++;
            }
            

            yield return new WaitForSeconds(0.1f);
        }
        endTexting = true;
        yield return new WaitUntil(() => !endTexting);

        pengDialog.SetActive(false);
        pingDialog.SetActive(false);

        yield break;
    }

    public void SkipTexting()
    {
        endTexting = true;
    }

    public void NextDialog()
    {
        endTexting = false;
    }

    public bool GetTexingIsEnd() { return endTexting; }
}
