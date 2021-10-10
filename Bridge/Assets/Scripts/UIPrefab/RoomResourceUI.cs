using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomResourceUI : MonoBehaviour
{
    public readonly static string RoomResourceDefaultPath = "Sprite/RoomResource/";

    public Image imageRoomResource;
    public Button buttonRoomResourceEdit;

    
    public void ButtonInteractable(bool isActive)
    {
        buttonRoomResourceEdit.interactable = isActive;
    }

    public void SetImageRoomResource(string imagePath)
    {
        Sprite sprite = null;
        sprite = Resources.Load<Sprite>(RoomResourceDefaultPath + imagePath);
        if(sprite == null)
        {
            Debug.LogFormat("Image path is wrong or not exist {0}", name);
        }
        else
        {
            imageRoomResource.sprite = sprite;
        }
    }

    //Onclick Method ButtonRoomResourceEdit
    public void OnClick()
    {
        //Show Inventory and Shop Popup
    }
    
}
