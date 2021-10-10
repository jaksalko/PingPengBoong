using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryViewUI : MonoBehaviour
{

    public Image summaryImage;
    bool endSummaryView = false;
    public IEnumerator SummaryViewIEnumerator(List<Sprite> sprites)
    {
        gameObject.SetActive(true);
        int summaryCount = 0;
        while(sprites.Count != summaryCount)
        {
            SetSummaryImage(sprites[summaryCount]);
            yield return new WaitUntil(() => endSummaryView);
            summaryCount++;
        }
        gameObject.SetActive(false);

        yield break;
    }

    void SetSummaryImage(Sprite sprite)
    {
        summaryImage.sprite = sprite;
        endSummaryView = false;
    }

    public void SetEndSummaryView(bool summaryView) { endSummaryView = summaryView; }
}
