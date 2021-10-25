using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

/*
 * 메인화면 / 데코레이션 패널에 있는 방 UI
 * 메인화면에서는 버튼의 기능과 비쥬얼 기능만 작용
 * 데코레이션에서는 슬롯을 변경할 수 있는 기능
 */
public enum Mode
{
    Lock,
    Decoration,
    Visualization
}

public class RoomDecoPrefab : MonoBehaviour
{
    public Mode mode;
    Button button = null;

    public Image dirtyImage;
    public Image roomImage;


    // 방에 달려있는 데코 슬롯들 5개
    public RoomResourceUI[] roomResourceUIs;

    private void Awake()
    {
        if (XMLManager.ins.database.userRooms[0].isDirty)
            mode = Mode.Lock;

        switch (mode)
        {
            case Mode.Lock:
                dirtyImage.gameObject.SetActive(true);
                foreach(RoomResourceUI resourceUI in roomResourceUIs)
                {
                    resourceUI.gameObject.SetActive(false);
                }
                break;
            case Mode.Visualization:
                UserRoom.changeSlotEvent += SetSlotImage;
                dirtyImage.gameObject.SetActive(false);
                button = transform.GetChild(0).GetComponent<Button>();
                button.onClick.AddListener(() => {
                    MainSceneUIScript mainSceneUI = GameObject.Find("Main Canvas").GetComponent<MainSceneUIScript>();
                    if(mainSceneUI != null)
                    {
                        foreach (var go in mainSceneUI.lowerButtons)
                            go.SetActive(false);
                        
                        mainSceneUI.decorationPanel.SetActive(true);
                    }
                });

                SetSlotImage();
                break;
            case Mode.Decoration:
                UserRoom.changeSlotEvent += SetSlotImage;
                dirtyImage.gameObject.SetActive(false);
                SetSlotImage();
                //Add Slot
                break;
        }
    }

    void SetSlotImage(int slotIdx, int value)
    {
        roomResourceUIs[slotIdx].SetImageRoomResource(value);
    }

    void SetSlotImage()
    {
        roomResourceUIs[0].SetImageRoomResource(XMLManager.ins.database.userRooms[0].slot_0);
        roomResourceUIs[1].SetImageRoomResource(XMLManager.ins.database.userRooms[0].slot_1);
        roomResourceUIs[2].SetImageRoomResource(XMLManager.ins.database.userRooms[0].slot_2);
        roomResourceUIs[3].SetImageRoomResource(XMLManager.ins.database.userRooms[0].slot_3);
        roomResourceUIs[4].SetImageRoomResource(XMLManager.ins.database.userRooms[0].slot_4);
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if(mode == Mode.Decoration || mode == Mode.Visualization)
            UserRoom.changeSlotEvent -= SetSlotImage;
    }
}
