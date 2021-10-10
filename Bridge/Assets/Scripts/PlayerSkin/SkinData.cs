using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SkinData : BaseData
{
    public class Info : BaseDataInfo
    {
        public DateTime skin_get_time;
        public string skinName;
        public string skinInfo;
        public string path;
        public char skinRank;

        public int boong_buy;
        public int powder_buy;
        public int powder_payback;

        public bool inPossession;

        public Info(int key, string _name, string _info, string _path, int boong, int powder, int payback) : base(key)
        {
            skinName = _name;
            skinInfo = _info;
            path = _path;
            skinRank = path[0];

            boong_buy = boong;
            powder_buy = powder;
            powder_payback = payback;
            inPossession = false;

            //skin_get_time = DateTime.ParseExact("0000-00-00 00:00:00", "yyyy-MM-dd HH:mm:ss", null);

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
                string type = csvDatas[idx]["type"];

                Debug.Log(type);
                if (type == "skin")
                {
                    string name_ = csvDatas[idx]["name"];
                    string skinInfo = csvDatas[idx]["info"];
                    int boong = ParseInt(csvDatas[idx]["boong"]);
                    int skin_powder = ParseInt(csvDatas[idx]["skin_powder"]);
                    string path = csvDatas[idx]["path"];

                    info = new Info(idx, name_, skinInfo, path, boong, skin_powder,0);
                    infos.Add(idx, info);
                }
                
            }
            else
            {
                Debug.LogErrorFormat("already exist data, key: {0}", idx);
            }
        }
    }
}
