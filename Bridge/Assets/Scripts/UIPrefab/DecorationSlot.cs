using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 데코레이션 팝업 창의 슬롯 
 * 슬롯마다 DecorationPopupItem을 가지고 있음
 * 
 */
public class DecorationSlot : MonoBehaviour
{
    public Transform scrollViewContent;
    Dictionary<int, DecorationPopupItem> decorationPopupItems = new Dictionary<int, DecorationPopupItem>();

    int slotIdx;
    public void AddSlotItem(int idx, DecorationPopupItem item)
    {
        if (!decorationPopupItems.ContainsKey(idx))
        {
            decorationPopupItems.Add(idx, item);
            item.transform.SetParent(scrollViewContent);
        }
        else
        {
            Debug.LogErrorFormat("already exist item {0}", idx);
        }

    }

    public void UpdateSlotItems(int slot, int key)
    {
        if (slot == slotIdx)
        {
            foreach (var decorationItem in decorationPopupItems.Values)
            {
                if (decorationItem.refData.key != key && decorationItem.itemState == ItemState.Apply)
                {
                    decorationItem.ChangeItemCallback();
                }

            }
        }
    }

    public void Init(int slot)//slot 번호 0-4
    {
        slotIdx = slot;
        foreach (var info in CSVManager.roomDecorationData.GetInfos(slot))
        {
            DecorationPopupItem decorationPopupItem = Instantiate(Resources.Load<DecorationPopupItem>(DecorationPopup.DecorationPrefabPath + "DecorationPopupItem"));
            decorationPopupItem.Init(info.key, slot);
            decorationPopupItem.transform.SetParent(scrollViewContent, false);
            decorationPopupItems.Add(info.key, decorationPopupItem);
        }

        UserRoom.changeSlotEvent += UpdateSlotItems;
    }

    public void AllButtonClicked()
    {
        foreach (var item in decorationPopupItems)
        {
            item.Value.gameObject.SetActive(true);
        }
    }

    public void OwnedButtonClicked()
    {
        foreach (var item in decorationPopupItems)
        {
            if (item.Value.itemState == ItemState.Lock || item.Value.itemState == ItemState.Sale)
            {
                item.Value.gameObject.SetActive(false);
            }
            else
            {
                item.Value.gameObject.SetActive(true);
            }
        }
    }

    public void ResetButtonClicked()
    {

    }
    private void OnDestroy()
    {
        UserRoom.changeSlotEvent -= UpdateSlotItems;
    }
}
