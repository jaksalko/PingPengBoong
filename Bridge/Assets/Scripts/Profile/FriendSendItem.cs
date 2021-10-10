using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendSendItem : FriendItem
{

    public void Cancel_Request()
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
