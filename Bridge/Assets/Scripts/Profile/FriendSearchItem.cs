using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendSearchItem : FriendItem
{
    //친구 신청
    public void Send_Request()
    {
        UserFriend userFriend = new UserFriend(AWSManager.instance.userInfo.nickname,friendInfo.nickname,0);
        UserFriend friend = new UserFriend(friendInfo.nickname, AWSManager.instance.userInfo.nickname, 1);

        FriendRequest friendRequest = new FriendRequest(userFriend,friend);
        var request = jsonAdapter.POST_DATA(friendRequest, "userFriend/insert", (isConnect) => {
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
