using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FriendListItem : FriendItem
{
    public Button sendHeartButton;
    public Text favorabilityText;

   
    public override void Initialize(UserInfo friendinfo, UserFriend friend_, ProfilePopup profilePopup)
    {
        base.Initialize(friendinfo, friend_, profilePopup);
        if(friend_.send)
        {
            sendHeartButton.interactable = false;
        }
        else
        {
            sendHeartButton.interactable = true;
        }

        favorabilityText.text = friend.friendship.ToString();

        
    }

    public void Send_Heart()
    {
        Mailbox mailbox = new Mailbox(
            receiver_: friendInfo.nickname,
            sender_: AWSManager.instance.userInfo.nickname,
            item_: "heart",
            quantitiy_: 1);

        UserFriend userFriend = friend.DeepCopy();
        userFriend.friendship++;
        userFriend.send = true;
        userFriend.time_request = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        HeartRequest heartRequest = new HeartRequest(mailbox, userFriend);

        var request = jsonAdapter.POST_DATA(heartRequest, "sendmailbox/insert", (isConnect)=> {
            if(isConnect)
            {
                popup.Refresh();
            }
            else
            {

            }
        });

        jsonAdapter.ReadyRequest(request);
    }
    public void Delete_Friend()
    {
        UserFriend userFriend = new UserFriend(AWSManager.instance.userInfo.nickname, friendInfo.nickname, 0);
        UserFriend friend = new UserFriend(friendInfo.nickname, AWSManager.instance.userInfo.nickname, 0);
        FriendRequest deleteRequest = new FriendRequest(userFriend, friend);
        var request = jsonAdapter.POST_DATA(deleteRequest, "userFriend/delete", (isConnect) =>
        {
            if (isConnect)
            {
                popup.Refresh();
            }
            else
            {

            }
        });

        jsonAdapter.ReadyRequest(request);
    }


}
