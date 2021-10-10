using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SkinItem : MonoBehaviour
{
   
    public GameObject soldout;
    public Text skinText;
    public Image skinImage;
    public Image skinRank;
    public Text boongPrice;
    public Text powderPrice;

    public Button boongBuyButton;
    public Button powderBuyButton;

    //CSV SkinData data
    public SkinData.Info skin;

    public void Initialize(SkinData.Info data)
    {
        skin = data;
        skinText.text = data.skinName;
        
        skinImage.sprite = Resources.Load<Sprite>(Constants.SkinStorePath+data.path);
        skinRank.sprite = Resources.Load<Sprite>("store/rank/" + data.skinRank);
        boongPrice.text = skin.boong_buy.ToString();
        powderPrice.text = skin.powder_buy.ToString();

        var skinstream = this.ObserveEveryValueChanged(x => x.skin.inPossession)
                .Subscribe(x => UpdateItem(x));
    }
    // Start is called before the first frame update
    

    public void InfoButtonClicked()
    {

    }

    public void BuyBoongPrice()
    {
        //UserInfo deepCopyUserInfo = AWSManager.instance.userInfo.DeepCopy();
        //UserHistory deepCopyUserHistory = AWSManager.instance.userHistory.DeepCopy();

        UserInfo userInfo = XMLManager.ins.database.userInfo;
        UserHistory userHistory = XMLManager.ins.database.userHistory;


        if(userInfo.boong >= skin.boong_buy)
        {
           

            UserInventory newItem = new UserInventory(userInfo.nickname, skinText.text);
            XMLManager.ins.database.userInventory.Add(newItem);
            skin.inPossession = true;

            userInfo.boong -= skin.boong_buy;
            userHistory.boong_use += skin.boong_buy;

            QuestManager.questDelegate(5, Data.QuestState.OnProgress);//5 시작
            QuestManager.questDelegate(4, Data.QuestState.Clear);//4 완료
            /*
            BuyItem skinBuy = new BuyItem(deepCopyUserInfo, deepCopyUserHistory, newItem);

            var request = JsonAdapter.instance.POST_DATA(skinBuy, "buyItem/update",(isConnect)=> {
                if(isConnect)
                {
                    if (QuestManager.questDelegate != null)
                    {
                        QuestManager.questDelegate(5, 2);//5 시작
                        QuestManager.questDelegate(4, 4);//4 완료
                    }
                }
                else
                {

                }
            });

            JsonAdapter.instance.ReadyRequest(request);
            */
        }
        else
        {
            Debug.Log("cant buy");
        }
        
    }
    public void BuyPowderPrice()
    {
        //UserInfo userInfo = AWSManager.instance.userInfo.DeepCopy();
        //UserHistory userHistory = AWSManager.instance.userHistory.DeepCopy();

        UserInfo userInfo = XMLManager.ins.database.userInfo;
        UserHistory userHistory = XMLManager.ins.database.userHistory;


        if (userInfo.skin_powder >= skin.powder_buy)
        {
            UserInventory newItem = new UserInventory(userInfo.nickname, skinText.text);
            userInfo.skin_powder -= skin.powder_buy;
            BuyItem skinBuy = new BuyItem(userInfo, userHistory, newItem);

            var request = JsonAdapter.instance.POST_DATA(skinBuy, "buyItem/update", (isConnect) => {
                if (isConnect)
                {
                    if (QuestManager.questDelegate != null)
                    {
                        QuestManager.questDelegate(5, Data.QuestState.OnProgress);//5 시작
                        QuestManager.questDelegate(4, Data.QuestState.Clear);//4 완료
                    }
                }
                else
                {

                }
            });

            JsonAdapter.instance.ReadyRequest(request);
        }
        else
        {
            Debug.Log("cant buy");
        }

        
    }


    void UpdateItem(bool possession)
    {
        soldout.SetActive(possession);
        boongBuyButton.interactable = !possession;
        powderBuyButton.interactable = !possession;
        if (possession)
        {
            skinImage.color = Color.gray;
            skinRank.color = Color.gray;
           
            //skinBox.color = Color.white;
        }
        else
        {
            skinImage.color = Color.white;
            skinRank.color = Color.white;
           
            //skinBox.color = Color.gray;
        }
    }
}
