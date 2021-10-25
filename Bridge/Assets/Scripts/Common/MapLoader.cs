using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;




public class MapLoader : MonoBehaviour
{


    //public int[,] map; // sampleMap에 있음
    //public bool[,] check; // 생성하며 구현
    public BlockFactory blockFactory;
    int style;

    [Header("Block Type")]
    public GroundBlock groundBlock;
  
    public GroundBlock groundBlock_second;
    public ObstacleBlock obstacleBlock;
    public ParfaitBlock[] parfaitBlock;
    public SlopeBlock slopeBlock;
    public CloudBlock cloudBlock;
    public CrackedBlock crackedBlock;
   

    [Header("Block Storage")]
    public Transform groundParent;
    public Transform obstacleParent;

    public Map editorMap;//made by editor scene

    public List<Map> sample;//island map datas

    //public Vector3 parfaitEndPoint;
    public Vector3 centerOfMap;
    public Transform minimapTarget;

    [Header("DESIGN OBJECT")]
    public Transform[] design;
    public Transform waterQuad;


    public Map liveMap;//on stage...

    
    CSVManager csvManager = CSVManager.instance;
    public CSVManager CSVManager_
    {
        get
        {
            if (csvManager == null)
            {
                csvManager = (CSVManager)FindObjectOfType(typeof(CSVManager));
            }

            return csvManager;
        }
        
    }
    void Start()
    {
        csvManager = CSVManager.instance;
        

    }
   
    void MakeMap(int mapsizeH , int mapsizeW , bool parfait)
    {
       
 
        

        centerOfMap = new Vector3((float)(mapsizeW - 1) / 2, -0.5f, (float)(mapsizeH - 1) / 2);
        minimapTarget.position = centerOfMap;

        waterQuad.position = centerOfMap;
        waterQuad.localScale = new Vector3(mapsizeW-2, mapsizeH-2, 1);

        //design[0].position = centerOfMap + new Vector3(0, -0.5f, 0);
        //design[1].localPosition = design[1].localPosition + new Vector3(0, 0, centerOfMap.z + 8);
        //design[2].localPosition = design[2].localPosition + new Vector3(0, 0, -(centerOfMap.z + 8));


       

        MakeGround(mapsizeH,mapsizeW);//block into map object block list

        /*
        if (parfait)
            MakeParfait(mapsizeH, mapsizeW);
        */
        
       
        
    }

   
    
    void MakeGround(int mapsizeH, int mapsizeW)
    {
        


        for (int i = 0; i < mapsizeH; i++)
        {
            for (int j = 0; j < mapsizeW; j++)
            {
                

                int data = liveMap.GetBlockDataByIndex(i,j);
                List<int> style = liveMap.GetBlockSyleByIndex(i * mapsizeW + j);

                if (i ==0 || i == mapsizeH - 1)
                {
                    data = BlockNumber.broken;
                }
                else if(j == 0 || j == mapsizeW-1)
                {
                    data = BlockNumber.broken;
                }

                Block newBlock = blockFactory.CreateBlock(data, style, new Vector2(j, i));
                liveMap.SetBlocks(j, i, newBlock);

                if (newBlock.data == BlockNumber.normal || newBlock.data == BlockNumber.upperNormal)
                {
                    liveMap.UpdateCheckArray(j, i, false);
                }
                else if(newBlock.data >= BlockNumber.parfaitA && newBlock.data <= BlockNumber.parfaitD)
                {
                    liveMap.UpdateCheckArray(j, i, false);
                }
                else if (newBlock.data >= BlockNumber.upperParfaitA && newBlock.data <= BlockNumber.upperParfaitD)
                {
                    liveMap.UpdateCheckArray(j, i, false);
                }
                else if(newBlock.data == BlockNumber.character || newBlock.data == BlockNumber.upperCharacter)
                {
                    liveMap.UpdateCheckArray(j, i, false);
                }
                else if(newBlock.data == BlockNumber.gift || newBlock.data == BlockNumber.upperGift)
                {
                    liveMap.UpdateCheckArray(j, i, false);
                }
                else
                {
                    liveMap.UpdateCheckArray(j, i, true);
                }
            }
        }
    }
       

    
//스테이지 모드
    public Map GenerateMap(int index)//index == gameManager.nowLevel
    {

        liveMap = GameManager.instance.stageDataOnPlay.GetMap();       
        MakeMap(liveMap.mapSizeHeight, liveMap.mapSizeWidth, liveMap.parfait);

        return liveMap;

        //return MakeMap(liveMap.mapsizeH, liveMap.mapsizeW, liveMap.parfait);
    }
    //에디터 모드
    public Map EditorStage()//내가 먼든 맵
    {
        liveMap = editorMap;
        MakeMap(liveMap.mapSizeHeight, liveMap.mapSizeWidth, liveMap.parfait);
        return liveMap;
    }
    //에디터 프레이모드
    public Map CustomPlayMap()//다른 유저가 만든 
    {
        //editorMap.Initialize(GameManager.instance.playEditorStage.itemdata);
        liveMap = GameManager.instance.playEditorMap;
        MakeMap(liveMap.mapSizeHeight, liveMap.mapSizeWidth, liveMap.parfait);
        return liveMap;
    }
    public IEnumerator InfiniteMAP(int level , System.Action<Map> callback)
    {
        UnityWebRequest www = UnityWebRequest.Get(PrivateData.ec2 + "map/difficulty?difficulty=" +level+"&nickname="+GameManager.instance.id);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            callback(null);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;

            //Get data and convert to samplemap list..

            
            string fixdata = JsonHelper.fixJson(www.downloadHandler.text);
            EditorStage[] datas = JsonHelper.FromJson<EditorStage>(fixdata);

            EditorStage selectedData = datas[Random.Range(0, datas.Length)];

            liveMap = editorMap;
            callback(liveMap);
            //selectedData.DataToString();//Debug.Log
            MakeMap(liveMap.mapSizeHeight, liveMap.mapSizeWidth, liveMap.parfait);
           
        }

        yield break;
    }



   

}




