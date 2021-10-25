using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockPiece : MonoBehaviour
{
    public string info;
    public string blockName;
    public int block_powder;

    public void Initialize(string name_ , string info_ , int block_powder_,string path_)
    {
        blockName = name_;
        info = info_;
        block_powder = block_powder_;
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(path_);
    }

    public void BuyBlockPowder()
    {
        if(AWSManager.instance.userInfo.block_powder >= block_powder)
        {
            UserInventory newItem = new UserInventory(AWSManager.instance.userInfo.nickname,-1, blockName);
            UserInfo deepcopyUserInfo = AWSManager.instance.userInfo.DeepCopy();
            UserHistory deepCopyUserHistory = AWSManager.instance.userHistory.DeepCopy();
            BuyItem buyBlockPiece = new BuyItem(deepcopyUserInfo, deepCopyUserHistory, newItem);

            deepcopyUserInfo.block_powder -= block_powder;

            var request = JsonAdapter.instance.POST_DATA(buyBlockPiece, "buyItem/update",(isConnect) => {
                if(isConnect)
                {
                    //연결 성공
                }
                else
                {
                    //연결 실패
                }

            });

            JsonAdapter.instance.ReadyRequest(request);
        }
        else
        {
            //돈이 부족합니다.
        }
         
    }


}
