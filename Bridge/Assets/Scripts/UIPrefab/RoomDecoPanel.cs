using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomDecoPanel : UIPrefab
{
    public DecorationPopup decorationPopup;
    public RoomDecoPrefab roomDeco;
    bool isLock = false;
    private void OnEnable()
    {
        //켜질 때 팝업 비활성화
        decorationPopup.gameObject.SetActive(false);
        if (!isLock)
            StartCoroutine(RoomDecoSlideUp());
    }

    private void Awake()
    {
        decorationPopup.Init();
        
        if (roomDeco.mode == Mode.Decoration)
        {
            for(int idx = 0; idx < roomDeco.roomResourceUIs.Length; idx++)
            {
                int key = idx;
                roomDeco.roomResourceUIs[key].RoomResourceEditButton.onClick.AddListener(() => ActiveDecorationPopup(key));
            }
        }
    }

    void ActiveDecorationPopup(int idx)
    {
        decorationPopup.ActiveSlot(idx);
    }

    public override void ExitButtonClicked()
    {
        if(!isLock)
            StartCoroutine(RoomDecoSlideDown());
    }

    IEnumerator RoomDecoSlideUp()
    {
        isLock = true;
        float t = 0;
        RectTransform rect = roomDeco.transform.GetComponent<RectTransform>();
        Vector2 targetPosition = rect.anchoredPosition + Vector2.up * 70;

        while(t <= 1)
        {
            Vector2 roomDecoSlideUpPosition = Vector3.Lerp(rect.anchoredPosition, targetPosition, t);
            rect.anchoredPosition = roomDecoSlideUpPosition;
            t += Time.deltaTime;
            yield return null;
        }
        isLock = false;
        yield break;
    }

    IEnumerator RoomDecoSlideDown()
    {
        isLock = true;
        decorationPopup.gameObject.SetActive(false);
        float t = 0;
        RectTransform rect = roomDeco.transform.GetComponent<RectTransform>();
        Vector2 targetPosition = rect.anchoredPosition - Vector2.up * 70;

        while (t <= 1)
        {
            Vector2 roomDecoSlideUpPosition = Vector3.Lerp(rect.anchoredPosition, targetPosition, t);
            rect.anchoredPosition = roomDecoSlideUpPosition;
            t += Time.deltaTime;
            yield return null;
        }
        isLock = false;

        MainSceneUIScript mainSceneUI = GameObject.Find("Main Canvas").GetComponent<MainSceneUIScript>();
        if (mainSceneUI != null)
        {
            foreach (var go in mainSceneUI.lowerButtons)
                go.SetActive(true);
        }

        gameObject.SetActive(false);

        yield break;
    }
}
