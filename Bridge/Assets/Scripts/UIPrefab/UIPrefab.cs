using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPrefab : MonoBehaviour
{
    protected BaseData.BaseDataInfo data;//C# Data Class

    protected GameManager gameManager;
    protected AWSManager awsManager;
    protected XMLManager xmlManager;
    protected JsonAdapter jsonAdapter;
    protected CSVManager csvManager;

    protected virtual void Start()
    {
        gameManager = GameManager.instance;
        awsManager = AWSManager.instance;
        xmlManager = XMLManager.ins;
        jsonAdapter = JsonAdapter.instance;
        csvManager = CSVManager.instance;

    }

    //자식을 파라미터로 받아 부모의 역할을 한다.
    public virtual void SetAsParentOfChild(Transform child)
    {
        child.SetParent(transform, false);
        transform.SetAsFirstSibling();
    }

    //부모를 파라미터로 받아 자식의 역할을 한다.
    public virtual void SetParentAsFirstSibling(Transform parent)
    {
        transform.SetParent(parent,false);
        transform.SetAsFirstSibling();
    }
    public virtual void SetParentAsLastSibling(Transform parent)
    {
        transform.SetParent(parent, false);
        transform.SetAsLastSibling();
    }
    public virtual void SetParentAsSiblingIndex(Transform parent,int index)
    {
        transform.SetParent(parent, false);
        transform.SetSiblingIndex(index);
    }

    public virtual void SetUIPrefab(BaseData.BaseDataInfo data)
    {
        this.data = data;
    }

    public void LoadSceneBySceneName(string sceneName, bool transition)
    {
        LoadingSceneManager.instance.LoadScene(sceneName,transition);
    }

    public virtual void ExitButtonClicked()
    {
        gameObject.SetActive(false);
    }

   

    
}
