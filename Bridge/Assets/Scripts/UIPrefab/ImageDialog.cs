using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;
public class ImageDialog : Dialog
{
    public Image backgroundCartoonImage;
    public Image pengImage;
    public Image pingImage;

    public IEnumerator ImageDialogIEnumerator(List<ImageDialogData.Info> questDialogs)
    {
        gameObject.SetActive(true);
        int dialogCount = 0;
        while (dialogCount != questDialogs.Count)
        {
            yield return StartCoroutine(Texting(questDialogs[dialogCount]));
            yield return new WaitUntil(() => !endTexting);
            dialogCount++;
        }
        gameObject.SetActive(false);
        yield break;
    }

    public void SetCartoonImage(Sprite sprite , bool value)
    {
        if(sprite !=null)
            backgroundCartoonImage.sprite = sprite;

        backgroundCartoonImage.gameObject.SetActive(value);
    }
    public void SetCartoonImage(bool value)
    {
        backgroundCartoonImage.gameObject.SetActive(value);
    }

    IEnumerator Texting(ImageDialogData.Info questDialog)
    {
        if (questDialog.speaker == "ping")
        {
            pingImage.sprite = Resources.Load<Sprite>("CharacterEmotion/"+questDialog.emotion);
        }
        else
        {
            pengImage.sprite = Resources.Load<Sprite>("CharacterEmotion/" + questDialog.emotion);
        }
        return Texting(new DialogData(questDialog.speaker,questDialog.line));
    }
}
