using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


   

public class UIScript : MonoBehaviour
{
    protected GameManager gameManager = GameManager.instance;
    protected AWSManager awsManager = AWSManager.instance;
    protected XMLManager xmlManager = XMLManager.ins;
    protected JsonAdapter jsonAdapter = JsonAdapter.instance;
    protected CSVManager csvManager = CSVManager.instance;
    
    void Start()
    {
        gameManager = GameManager.instance;
        awsManager = AWSManager.instance;
        xmlManager = XMLManager.ins;
        jsonAdapter = JsonAdapter.instance;
        csvManager = CSVManager.instance;

    }
   
    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
