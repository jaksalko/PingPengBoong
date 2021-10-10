using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationSlot : MonoBehaviour
{
    public Transform scrollViewContent;
    Dictionary<int,DecorationPopupItem> decorationPopupItems = new Dictionary<int,DecorationPopupItem>();

    public void AddSlotItem(int idx,DecorationPopupItem item)
    {
        if(!decorationPopupItems.ContainsKey(idx))
        {
            decorationPopupItems.Add(idx, item);
            item.transform.SetParent(scrollViewContent);
        }
        else
        {
            Debug.LogErrorFormat("already exist item {0}", idx);
        }
        
    }

    public void Init(int slot)
    {
        foreach(var info in CSVManager.roomDecorationData.GetInfos(slot))
        {
            DecorationPopupItem decorationPopupItem = Instantiate(Resources.Load<DecorationPopupItem>(DecorationPopup.DecorationPrefabPath + "DecorationPopupItem"));
            decorationPopupItem.Init(info.key);
            decorationPopupItem.transform.SetParent(scrollViewContent, false);
            decorationPopupItems.Add(info.key, decorationPopupItem);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
