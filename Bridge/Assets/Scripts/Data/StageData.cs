using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Data
{
    [System.Serializable]
    public class StageData : BaseData
    {
        

        public class Info : BaseDataInfo
        {
            private readonly string stageName;
            private readonly int numberOfSnow;

            private readonly int stageNumber;
            private readonly int islandNumber;
            private bool isClear;
            private bool isOpen;
            private int starCount;
            private int[] unLock;
            private Map stageMap;

            public Info(int key, string stageName, int stageNumber, int islandNumber, bool isClear, int starCount, int numberOfSnow, Map map, int[] unLock) : base(key)
            {
                this.stageName = stageName;
                this.numberOfSnow = numberOfSnow;

                this.stageNumber = stageNumber;
                this.islandNumber = islandNumber;
                this.isClear = isClear;
                if(key == 1)
                {
                    isOpen = true;
                }
                else
                {
                    isOpen = isClear;
                }
               
                this.starCount = starCount;
                stageMap = map;
                this.unLock = unLock;
            }

            public string GetStageText() { return islandNumber + "-" + stageNumber; }
            public string GetStageName() { return stageName; }
            public int GetNumberOfSnow() { return numberOfSnow; }
            public bool GetIsClear() { return isClear; }
            public bool GetIsOpen() { return isOpen; }
            public int GetStageNumber() { return stageNumber; }
            public int GetIslandNumber() { return islandNumber; }
            public int GetStarCount() { return starCount; }
            public Map GetMap() { Debug.Log("LoadMap In Island : " + islandNumber); return stageMap; }
            public void SetOpen(bool isOpen) { this.isOpen = isOpen; }
            public void ClearStage(int starCount)//서버에 제대로 전송되면 업데이트 됨.
            {
                isClear = true;
                isOpen = true;
                this.starCount = starCount;
                if(unLock != null)
                {
                    foreach (int idx in unLock)
                    {
                        CSVManager.stageData.GetInfo(idx).SetOpen(true);
                    }

                }

            }

        }

        readonly private Dictionary<int, Info> infos = new Dictionary<int, Info>();

        public Info GetInfo(int key)
        {
            return infos[key];
        }

        public Info GetInfo(int island, int stageNumber)
        {
            foreach(var info in infos.Values)
            {
                if(info.GetIslandNumber() == island && info.GetStageNumber() == stageNumber)
                {
                    return info;
                }
            }

            return null;
        }

        public Dictionary<int, Info>.Enumerator GetInfoEnumerator()
        {
            return infos.GetEnumerator();
        }

        

        public override void Parsing(List<Dictionary<string, string>> csvDatas)
        {
            
            for(int idx = 1; idx < csvDatas.Count; idx++)
            {
                Info info = null;
                if (!infos.TryGetValue(idx,out info))
                {
                    int islandNumber = ParseInt(csvDatas[idx]["islandNumber"]);
                    int stageNumber = ParseInt(csvDatas[idx]["stageNumber"]);
                    int numberOfSnow = ParseInt(csvDatas[idx]["numberOfSnow"]);
                    string stageName = csvDatas[idx]["stageName"];
                    int[] stepLimits = ParseIntArr(csvDatas[idx]["stepLimits"]);

                    int width = ParseInt(csvDatas[idx]["width"]);
                    int height = ParseInt(csvDatas[idx]["height"]);
                    List<List<int>> mapBlockDataList = MapString(csvDatas[idx]["mapBlockDataList"].Split(';'), height, width);
                    List<List<int>> mapBlockStyleList = new List<List<int>>();
                    for (int i = 0; i < width * height; i++)
                    {
                        mapBlockStyleList.Add(new List<int>() {islandNumber-1, islandNumber - 1, islandNumber - 1 });
                    }

                    Vector3 characterAPosition = ParseVector3(csvDatas[idx]["characterAPosition"]);
                    Vector3 characterBPosition = ParseVector3(csvDatas[idx]["characterBPosition"]);
                    bool isParfait = ParseInt(csvDatas[idx]["parfait"]) == 0 ? false : true;

                    Map newMap = new Map(mapBlockDataList, mapBlockStyleList, stepLimits, characterAPosition, characterBPosition, width, height, isParfait);

                    int[] unLock = ParseIntArr(csvDatas[idx]["unLock"]);
                    info = new Info(idx,stageName, stageNumber, islandNumber, false, 0, numberOfSnow, newMap, unLock);

                    infos.Add(idx,info);
                }
                else
                {
                    Debug.LogErrorFormat("already exist data, key: {0}", idx);
                }
            }
        }

        public override void ClearData()
        {
            infos.Clear();
        }

        List<List<int>> MapString(string[] data_string, int height, int width)
        {

            List<List<int>> map_datas = new List<List<int>>();


            int count = 0;

            for (int i = 0; i < height; i++)
            {
                List<int> line = new List<int>();
                for (int j = 0; j < width; j++)
                {
                    int data = int.Parse(data_string[count]);
                    line.Add(data);

                    count++;
                }
                map_datas.Add(line);
            }

            return map_datas;
        }

    }
}

