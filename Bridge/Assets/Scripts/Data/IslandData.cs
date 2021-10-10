using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Data
{
    public class IslandRoadUIStagePosition
    {
        public static Dictionary<int, List<Vector3>> IslandRoadUIPrefabStagePosition
            = new Dictionary<int, List<Vector3>>()
            {
                {1,new List<Vector3>(){ new Vector3(-120,-390,0), new Vector3(224, -169, 0), new Vector3(-200, 152, 0), new Vector3(113, 337, 0), new Vector3(399, 558, 0), new Vector3(-8, 844, 0) } },
                {2,new List<Vector3>(){ new Vector3(-77,-1330,0), new Vector3(-339, -544, 0), new Vector3(261, 10, 0), new Vector3(10, 714, 0) } },
                {3,new List<Vector3>(){ new Vector3(139,-935,0), new Vector3(-139, -180, 0), new Vector3(231, 678, 0), new Vector3(-288, 1171, 0)} },
                {4,new List<Vector3>(){ new Vector3(113,-996,0), new Vector3(-144, -216, 0), new Vector3(123, 524, 0), new Vector3(-344, 1130, 0) } },
                {5,new List<Vector3>(){ new Vector3(241,-1104,0), new Vector3(-329, -339, 0), new Vector3(-98, 416, 0), new Vector3(98, 1012, 0) } }
            };

    }
  


    [System.Serializable]
    public class IslandData : BaseData
    {
        public class Info : BaseDataInfo
        {
            private readonly string islandName;//리소스 호출 및 신 호출에 사용
            private readonly int maxStar;
            private int myStar;
            private readonly string islandInfo;

            public Info(int key, string islandName, string islandInfo, int maxStar) : base(key)
            {
                this.islandName = islandName;
                this.maxStar = maxStar;
                myStar = 0;

                this.islandInfo = islandInfo;
            }

            public void SetMyStar()
            {
                using(var e = CSVManager.stageData.GetInfoEnumerator())
                {
                    myStar = 0;
                    while(e.MoveNext())
                    {
                        var data = e.Current.Value;
                        if(data.GetIslandNumber() == key)
                        {
                            myStar += data.GetStarCount();
                        }
                    }
                }
            }

           
            public int GetMaxStar()
            {
                return maxStar;
            }
            public int GetMyStar()
            {
                return myStar;
            }
            public int GetIslandNumber()
            {
                return key;
            }
            public string GetIslandName()
            {
                return islandName;
            }
            public bool IsIslandClear()
            {
                bool isClear = true;

                using (var e = CSVManager.stageData.GetInfoEnumerator())
                {
                    while(e.MoveNext())
                    {
                        var data = e.Current.Value;
                        if(data.GetIslandNumber() == key)
                        {
                            if(!data.GetIsClear())
                            {
                                isClear = false;
                                break;
                            }
                        }
                    }
                }

                return isClear;
            }

            public StageData.Info NextStage(int stageNumber)
            {
                //다음 스테이지 번호
                stageNumber += 1;
                return CSVManager.stageData.GetInfo(key, stageNumber);
            }

            public StageData.Info BeforeStage(int stageNumber)
            {
                //다음 스테이지 번호
                stageNumber -= 1;
                return CSVManager.stageData.GetInfo(key, stageNumber);
            }


        }

        readonly private Dictionary<int, Info> infos = new Dictionary<int, Info>();

        public Info GetInfo(int key)
        {
            return infos[key];
        }

        

        public Dictionary<int, Info>.Enumerator GetInfoEnumerator()
        {
            return infos.GetEnumerator();
        }

        public override void Parsing(List<Dictionary<string, string>> csvDatas)
        {
            for (int idx = 1; idx < csvDatas.Count; idx++)
            {
                Info info = null;
                if (!infos.TryGetValue(idx, out info))
                {
                    string islandName = csvDatas[idx]["islandName"];
                    string islandInfo = csvDatas[idx]["islandInfo"];
                    int maxStar = ParseInt(csvDatas[idx]["maxStar"]);
                    info = new Info(idx, islandName, islandInfo, maxStar);
                    infos.Add(idx, info);
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

        //ChangeMyStarEvent

        //StageClearEvent 

        //SetIslandDataEvent 섬데이터가 생성이 되면 IslandUIPrefab을 생성한다.

    }

}
