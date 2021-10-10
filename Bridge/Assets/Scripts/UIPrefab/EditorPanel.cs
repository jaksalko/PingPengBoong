using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class EditorPanel : UIPrefab
{

    bool playMode = false;

    public RectTransform editorLobbyPanel;
    public RectTransform editorPlayPanel;
    public GameObject[] MainScenelowerButtonPanel;

    Coroutine coroutine = null;
    public void BackToEditorLobbyButtonClicked()
    {
        if(coroutine == null)
            coroutine = StartCoroutine(SwitchEditorPanelMode());
    }

    public void EditorPlayButtonClicked()
    {
        if (coroutine == null)
            coroutine = StartCoroutine(SwitchEditorPanelMode());
    }

    public void EditorMakeButtonClicked()
    {
        SceneManager.LoadScene("MapEditor");
    }

    IEnumerator SwitchEditorPanelMode()
    {
        Vector2 distance = Vector2.zero;
        RectTransform rectTransform = GetComponent<RectTransform>();
        if(playMode)//panel move right
        {
            foreach(var obj in MainScenelowerButtonPanel)
            {
                obj.gameObject.SetActive(true);
            }
            distance = rectTransform.anchoredPosition + editorPlayPanel.anchoredPosition - editorLobbyPanel.anchoredPosition;
        }
        else
        {
            foreach (var obj in MainScenelowerButtonPanel)
            {
                obj.gameObject.SetActive(false);
            }
            distance = rectTransform.anchoredPosition + editorLobbyPanel.anchoredPosition - editorPlayPanel.anchoredPosition;
        }


        float t = 0;
        while(t <= 1)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, distance, t);
            t += Time.deltaTime;

            yield return null;
        }

        playMode = !playMode;
        coroutine = null;
        yield break;
    }
}
