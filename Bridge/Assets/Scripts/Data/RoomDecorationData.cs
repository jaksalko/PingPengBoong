using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{

    public class RoomDecorationData : BaseData
    {
        public class Info : BaseDataInfo
        {
            string name_key;
            public string imagePath;
            int price;
            int island;
            int level;
            int slot;

            public Info(int idx, string name_key, string imagePath, int price, int island, int level, int slot): base(idx)
            {
                this.name_key = name_key;
                this.imagePath = imagePath;
                this.price = price;
                this.island = island;
                this.level = level;
                this.slot = slot;
            }

            public int GetSlot() { return slot; }
            public int GetPrice() { return price; }
            public string GetName() { return name_key; }
        }

        Dictionary<int, Info> Infos = new Dictionary<int, Info>();
        public List<int> Slots = new List<int>();

        public Info GetInfo(int idx)
        {
            if (Infos.ContainsKey(idx))
            {
                return Infos[idx];
            }
            else
            {
                Debug.LogErrorFormat("room decoration info doesn't contain key {0}", idx);
                return null;
            }
        }

        public List<Info> GetInfos(int slot)
        {
            List<Info> infos = new List<Info>();

            foreach(var info in Infos.Values)
            {
                if(info.GetSlot() == slot)
                {
                    infos.Add(info);
                }
            }

            return infos;
        }

       

        public override void Parsing(List<Dictionary<string, string>> csvDictionary)
        {
            for (int i = 1; i < csvDictionary.Count; i++)
            {
                string type = csvDictionary[i]["type"];
                if(type == "room")
                {
                    string name_key = csvDictionary[i]["name"];
                    string imagePath = csvDictionary[i]["path"];
                    int price = ParseInt(csvDictionary[i]["boong"]);
                    int island = ParseInt(csvDictionary[i]["island"]);
                    int level = ParseInt(csvDictionary[i]["stage"]);
                    int slot = ParseInt(csvDictionary[i]["slot"]);
                    if (!Slots.Contains(slot))
                    {
                        Slots.Add(slot);
                    }
                    Info info = new Info(i, name_key, imagePath, price, island, level, slot);
                    Infos.Add(i, info);
                }
            }
        }

        public override void ClearData()
        {
            Infos.Clear();
        }
    }
}
