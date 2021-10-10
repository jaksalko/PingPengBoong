using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationPopup : MonoBehaviour
{
    public readonly static string DecorationPrefabPath = "Prefab/Decoration/";

    Dictionary<int,DecorationSlot> decorationSlots = new Dictionary<int, DecorationSlot>();

    private void Awake()
    {
        Debug.Log(CSVManager.roomDecorationData.Slots.Count);
        foreach(int slot in CSVManager.roomDecorationData.Slots)
        {
            DecorationSlot decorationSlot = Instantiate(Resources.Load<DecorationSlot>(DecorationPrefabPath + "DecorationSlot"));
            decorationSlot.Init(slot);
            decorationSlot.transform.SetParent(transform,false);
            decorationSlots.Add(slot, decorationSlot);
        }

    }

}
