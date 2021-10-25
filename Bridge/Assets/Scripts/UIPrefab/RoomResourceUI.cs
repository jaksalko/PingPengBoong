using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomResourceUI : MonoBehaviour
{
    public readonly static string RoomResourceDefaultPath = "Sprite/RoomResource/";

    
    public Button RoomResourceEditButton;
    public GameObject[] imageRoomResources;

    public void ButtonInteractable(bool isActive)
    {
        RoomResourceEditButton.interactable = isActive;
    }

    //idx == XML userRooms.slot_
    public void SetImageRoomResource(int idx)
    {
        foreach (var imageResource in imageRoomResources)
        {
            imageResource.SetActive(false);
        }

        if (idx == 0)
        {
            imageRoomResources[idx].SetActive(true);
            return;
        }
            

        int islandNumber = CSVManager.roomDecorationData.GetInfo(idx).GetIsland();
        /*
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
        */
        

        imageRoomResources[islandNumber-1].SetActive(true);
    }

    //Onclick Method ButtonRoomResourceEdit
    public void OnClick()
    {
        //Show Inventory and Shop Popup
    }
    
}
