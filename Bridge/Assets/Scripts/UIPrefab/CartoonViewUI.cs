using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartoonViewUI : MonoBehaviour
{
    public float faderSpeed;
    public Image summaryImage;
    bool endCartoonView = false;
    public IEnumerator CartoonViewIEnumerator(Sprite sprite)
    {
        gameObject.SetActive(true);
        endCartoonView = false;
        summaryImage.sprite = sprite;
        float t = 0;
        Color color = summaryImage.color;
        while (t < 1)//fade in
        {
            t += Time.deltaTime * faderSpeed;
            color.a = t;
            summaryImage.color = color;
            yield return null;
        }
        yield return new WaitUntil(() => endCartoonView);
        gameObject.SetActive(false);
        yield break;
    }

    

    public void SetEndCartoonView(bool cartoonView) { endCartoonView = cartoonView; }

   
}
