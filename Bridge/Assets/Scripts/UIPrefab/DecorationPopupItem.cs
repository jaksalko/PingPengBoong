using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public enum ItemState
{
    Lock,
    Sale,
    Ready,
    Apply
}

/*
 * 데코레이션 슬롯에 적용할 수 있는 아이템
 * 버튼 기능(구매 또는 장착 또는 스테이지 선택팝업) 
 * 미리보기 기능
 */
public class DecorationPopupItem : MonoBehaviour
{
    public Button button;

    public GameObject[] stateButtonImages;
    public Image imageDecorationItem;
    public Text textDecorationItem;
    public Text textDecorationPrice;
    public Text textUnlockStage;

    public ItemState itemState;
    public int slot;
    public RoomDecorationData.Info refData = null;

    private void Awake()
    {
        button.onClick.AddListener(() => Onclick());
    }

    public void Init(int idx, int slot)//roomDecoKey, slot(0-4)
    {
        this.slot = slot;
        RoomDecorationData.Info roomDecorationData = CSVManager.roomDecorationData.GetInfo(idx);
        refData = roomDecorationData;
        imageDecorationItem.sprite = Resources.Load<Sprite>(Constants.RoomDecoPath+roomDecorationData.imagePath);
        textDecorationItem.text = roomDecorationData.GetName();
        textDecorationPrice.text = roomDecorationData.GetPrice().ToString();
        textUnlockStage.text = roomDecorationData.GetIsland() + "-" + roomDecorationData.GetLevel();

        int islandNumber = roomDecorationData.GetIsland();
        int stageNumber = roomDecorationData.GetLevel();

        using(var e = CSVManager.stageData.GetInfoEnumerator())
        {
            while(e.MoveNext())
            {
                var data = e.Current.Value;
                if(data.GetIslandNumber() == islandNumber && data.GetStageNumber() == stageNumber)
                {
                    if (data.GetIsClear())
                    {
                        if (XMLManager.ins.database.userInventory.Exists((x) => x.itemIdx == roomDecorationData.key))
                        {
                            if(XMLManager.ins.database.userRooms[0].GetSlotData(slot) == idx)
                            {
                                itemState = ItemState.Apply;
                            }
                            else
                            {
                                itemState = ItemState.Ready;
                            }
                        }
                        else
                        {

                            itemState = ItemState.Sale;
                        }
                    }
                    else
                        itemState = ItemState.Lock;
                }
            }
        }

        SetButtonImage();
        
    }

   

    public void Onclick()
    {
        switch (itemState)
        {
            case ItemState.Lock:
                GetStagePopup();
                break;
            case ItemState.Sale:
                BuyItem();
                break;
            case ItemState.Ready:
                ApplyItem();
                break;
            case ItemState.Apply:
                ReleaseItem();
                break;
        }
        XMLManager.ins.SaveItems();
    }

    void SetButtonImage()
    {
        foreach(var go in stateButtonImages)
        {
            go.SetActive(false);
        }

        switch (itemState)
        {
            case ItemState.Lock:
                stateButtonImages[0].SetActive(true);
                break;
            case ItemState.Sale:
                stateButtonImages[1].SetActive(true);
                break;
            case ItemState.Ready:
                stateButtonImages[2].SetActive(true);
                break;
            case ItemState.Apply:
                stateButtonImages[3].SetActive(true);
                break;
        }
    }

    void GetStagePopup()
    {

    }

    void BuyItem()
    {
        if(XMLManager.ins.database.userInfo.boong >= refData.GetPrice())
        {
            itemState = ItemState.Ready;
            UserInventory userInventory = new UserInventory(SystemInfo.deviceUniqueIdentifier, refData.key, refData.GetName());
            XMLManager.ins.database.userInventory.Add(userInventory);
            XMLManager.ins.database.userInfo.boong -= refData.GetPrice();
            XMLManager.ins.database.userHistory.boong_use += refData.GetPrice();
            
        }
        else
        {

        }
       
    }

    void ApplyItem()
    {
        itemState = ItemState.Apply;
        XMLManager.ins.database.userRooms[0].SetSlotData(slot, refData.key);
        QuestManager.questDelegate(5, QuestState.Clear);
        SetButtonImage();
    }

    void ReleaseItem()
    {
        itemState = ItemState.Ready;
        XMLManager.ins.database.userRooms[0].SetSlotData(slot, 0);
        SetButtonImage();
    }

    public void ChangeItemCallback()
    {
        itemState = ItemState.Ready;
        SetButtonImage();
    }

    public void SetSaleItem()
    {
        itemState = ItemState.Sale;
    }
}
