using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class SkinStore : MonoBehaviour
{
    public Dropdown sortDropDown;
    public Text skin_powder;
    public Transform content;
    public SkinItem skinItemPrefab;
    public List<SkinItem> skinItems;

    CSVManager csvManager = CSVManager.instance;

    // Start is called before the first frame update
    void Start()
    {
        csvManager = CSVManager.instance;
        GetItemList();
        Sorting();
    }

    void GetItemList()
    {
        using (var e = CSVManager.skinData.GetInfoEnumerator())
        {
            while (e.MoveNext())
            {
                var data = e.Current.Value;
                if(data.skinRank != 's')
                {
                    SkinItem item = Instantiate(skinItemPrefab);
                    item.Initialize(data);
                    skinItems.Add(item);
                    item.transform.SetParent(content, false);
                }
                
            }

        }

    }

    private void Update()
    {
        //skin_powder.text = AWSManager.instance.userInfo.skin_powder.ToString();
    }

    public void Sorting()
    {
        int dropDown = sortDropDown.value;
        if (dropDown == 0)//이름 SkinItem
        {
            skinItems = skinItems.OrderByDescending(x => x.skin.skinName).ToList();
        }
        else if (dropDown == 1)//등급 순
        {
            skinItems = skinItems.OrderBy(x => x.skin.key).ToList();
        }

        foreach (SkinItem item in skinItems)
        {
            item.transform.SetAsFirstSibling();
        }
    }
}