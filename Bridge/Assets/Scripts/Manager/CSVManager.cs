using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Data;
using System.Linq;

public class CSVManager : MonoBehaviour
{
    public static CSVManager instance = null;

    public static RewardData rewardData = new RewardData();
    public static RoomDecorationData roomDecorationData = new RoomDecorationData();
    public static StageData stageData = new StageData();
    public static QuestData questData = new QuestData();
    public static ImageDialogData imageDialogData = new ImageDialogData();
    public static IslandData islandData = new IslandData();
    public static SkinData skinData = new SkinData();
    void Awake()
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

        //GetStyleList();
        //GetBlockItemData();
        stageData.Parsing(CSVReader.Read("stage_soft"));
        roomDecorationData.Parsing(CSVReader.Read("item"));
        questData.Parsing(CSVReader.Read("Quest"));
        islandData.Parsing(CSVReader.Read("IslandData"));
        skinData.Parsing(CSVReader.Read("item"));
        imageDialogData.Parsing(CSVReader.Read("QuestDialog"));
        rewardData.Parsing(CSVReader.Read("starReward"));
    }

   

   

    /*
    public void GetStyleList()//칭호 리스트
    {
        List<Dictionary<string, object>> item = CSVReader.Read("style");

        for (int i = 0; i < item.Count; i++)
        {
            string txt = item[i]["style_text"].ToString();
            string info = item[i]["style_info"].ToString();
            string type = item[i]["type"].ToString();
            string condition = item[i]["condition"].ToString();
            int standard = int.Parse(item[i]["standard"].ToString());
            int reward_boong = int.Parse(item[i]["reward_boong"].ToString());
            int reward_heart = int.Parse(item[i]["reward_heart"].ToString());
            string reward_item = item[i]["reward_item"].ToString();

            Style style = new Style(txt, info, type, condition, standard, reward_boong, reward_heart, reward_item);
            styleList.Add(style);
            //StartCoroutine(style.CheckStyle());
        }



    }
    */
    

    /*
    public void GetBlockItemData()
    {
        List<Dictionary<string, object>> item = CSVReader.Read("item");

        for (int i = 0; i < item.Count; i++)
        {
            string type = item[i]["type"].ToString();
            if (type == "blockPiece")
            {
                string name = item[i]["name"].ToString();
                string info = item[i]["info"].ToString();
                int block_powder = int.Parse(item[i]["block_powder"].ToString());
                
                string path = item[i]["path"].ToString();

                BlockPiece blockPiece = Instantiate(blockPiecePrefab);
                blockPiece.transform.SetParent(transform);
                blockPiece.Initialize(name, info, block_powder, path);
                blockPieces.Add(blockPiece);
            }
        }
    }
    */

   

    
}
