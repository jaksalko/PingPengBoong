using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{

    public static LoadingSceneManager instance = null;
    public static string nextScene;
    public Animator transitionAnimator;
    public float customTimerMin;
    bool loading = false;
    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("Single instance is null");
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Single instance is not Single.. Destroy gameobject!");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);//Dont destroy this singleton gameobject :(

    }

    

    public void LoadScene(string sceneName , bool useTransition)
    {
        if(!loading)
        {
            loading = true;
            nextScene = sceneName;

            if (useTransition)
            {
                StartCoroutine(LoadScene());
            }
            else
            {
                loading = false;
                SceneManager.LoadScene(nextScene);
            }
        }
        
        
        
    }

    IEnumerator LoadScene()
    {
        
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        transitionAnimator.SetInteger("Load",1);
        op.allowSceneActivation = false;
        float customTimer = 0f;
        while (!op.isDone)
        {
            customTimer += Time.deltaTime;
            //Debug.Log(op.progress + ", " + customTimer);
            if (op.progress >= 0.9f && customTimer >= customTimerMin)
            {
                loading = false;
                op.allowSceneActivation = true;
                transitionAnimator.SetInteger("Load",2);
            }
           
            yield return null;
        }
        
    }

}
