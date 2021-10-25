using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class RewardData : BaseData
{
    public class Info : BaseDataInfo
    {
        public int island;
        public int star_index;

        public int boong;
        public int heart;
        public int skin_powder;
        public int block_powder;

        public List<UserInventory> userInventory;

        public Info(int key,int island_num, int level, int boong, int heart, int block_powder, int skin_powder, int[] itemIdx) : base(key)
        {
            island = island_num;
            star_index = level;
            this.boong = boong;
            this.heart = heart;
            this.block_powder = block_powder;
            this.skin_powder = skin_powder;

            if (itemIdx != null)
            {
                Debug.Log("item");
                List<UserInventory> userInventory = new List<UserInventory>();
                foreach (var item in itemIdx)
                {
                    userInventory.Add(new UserInventory(SystemInfo.deviceUniqueIdentifier, item, CSVManager.skinData.GetInfo(item).skinName));
                }
                this.userInventory = userInventory;

            }
            else
            {
                this.userInventory = null;
            }
        }
    }

    readonly private Dictionary<int, Info> infos = new Dictionary<int, Info>();

    public Info GetInfo(int key)
    {
        return infos[key];
    }

    public int GetInfoIdx(int island, int level)
    {
        foreach(var info in infos)
        {
            if(info.Value.island == island && info.Value.star_index == level)
            {
                return info.Key;
            }
        }

        return -1;
    }

    public override void ClearData()
    {
        infos.Clear();
    }

    public override void Parsing(List<Dictionary<string, string>> csvDatas)
    {
        for (int idx = 1; idx < csvDatas.Count; idx++)
        {
            Info info = null;
            if (!infos.TryGetValue(idx, out info))
            {
                int island = ParseInt(csvDatas[idx]["island"]);
                int level = ParseInt(csvDatas[idx]["level"]);

                int boong = ParseInt(csvDatas[idx]["boong"]);
                int heart = ParseInt(csvDatas[idx]["heart"]);
                int block_powder = ParseInt(csvDatas[idx]["block_powder"]);
                int skin_powder = ParseInt(csvDatas[idx]["skin_powder"]);
                int[] item = ParseIntArr(csvDatas[idx]["itemIdx"]);
                info = new Info(idx, island, level, boong, heart, block_powder, skin_powder, item);

                infos.Add(idx, info);
            }
            else
            {
                Debug.LogErrorFormat("already exist data, key: {0}", idx);
            }
        }
    }


    //something items...
}

