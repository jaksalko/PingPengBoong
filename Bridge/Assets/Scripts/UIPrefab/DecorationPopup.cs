using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 슬롯을 꾸밀 수 있는 데코레이션 팝업
 * DecorationSlot이 담긴다.
 * 미리보기 리셋 버튼
 * 전체 / 보유중
 */
public class DecorationPopup : MonoBehaviour
{
    public readonly static string DecorationPrefabPath = "Prefab/Decoration/";

    Dictionary<int,DecorationSlot> decorationSlots = new Dictionary<int, DecorationSlot>();

    public void Init()
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

    public void ActiveSlot(int idx)
    {
        
        foreach (var slot in decorationSlots)
        {
            Debug.LogFormat("key {0} slot {1}", idx, slot.Key);

            if (slot.Key == idx)
                slot.Value.gameObject.SetActive(true);
            else
                slot.Value.gameObject.SetActive(false);
        }

        gameObject.SetActive(true);

    }

}
