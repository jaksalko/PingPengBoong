using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class DecorationPopupItem : MonoBehaviour
{
    
    public Image imageDecorationItem;
    public Text textDecorationItem;
    public Text textDecorationPrice;

    public enum ItemState
    {
        Lock,
        Sale,
        Ready,
        Apply
    }

    public void Init(int idx)
    {
        RoomDecorationData.Info roomDecorationData = CSVManager.roomDecorationData.GetInfo(idx);
        imageDecorationItem.sprite = Resources.Load<Sprite>(Constants.RoomDecoPath+roomDecorationData.imagePath);
        textDecorationItem.text = roomDecorationData.GetName();
        textDecorationPrice.text = roomDecorationData.GetPrice().ToString();

        //유저 데이터 대입

    }
    public void Onclick()
    {

    }

    public void BuyItem()
    {

    }

    public void ApplyItem()
    {

    }
}
