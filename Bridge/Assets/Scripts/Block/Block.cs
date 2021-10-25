using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Block : MonoBehaviour , IBlock
{
    public enum Type
    {
        Ground,
        Parfait,
        Cracker,
        Cloud,
        Gift,
        Other
    };

    public Type type;
    public int style; // 0 : 튜토리얼 1 : 아이스크림 2 : 파르페 3 : 크래커 4 : 솜사탕 5 : 에디터
    public GameObject[] object_styles;

    public int data;
    public bool isClear = true;

    Vector3[] directionVector = { Vector3.forward , Vector3.right , Vector3.back , Vector3.left};
    public virtual void Init(int block_num , int style)
    {
        data = block_num;
        this.style = style;
        isClear = true;

        if (data == BlockNumber.normal || data == BlockNumber.upperNormal)
        {
            type = Type.Ground;
        }
        else if ((data >= BlockNumber.cloudUp && data <= BlockNumber.cloudLeft) || (data >= BlockNumber.upperCloudUp && data <= BlockNumber.upperCloudLeft))
        {
            type = Type.Cloud;
        }
        else if ((data >= BlockNumber.cracker_0 && data <= BlockNumber.cracker_2) || (data >= BlockNumber.upperCracker_0 && data <= BlockNumber.upperCracker_2))
        {
            type = Type.Cracker;
        }
        else if ((data >= BlockNumber.parfaitA && data <= BlockNumber.parfaitD) || (data >= BlockNumber.upperParfaitA && data <= BlockNumber.upperParfaitD))
        {
            type = Type.Parfait;
        }
        else if(data == BlockNumber.gift || data == BlockNumber.upperGift)
        {
            type = Type.Gift;
        }
        else
        {
            type = Type.Other;
        }
    }

    public Block ShallowCopy()
    {
        return (Block)this.MemberwiseClone();
    }

    public virtual Vector3 GetCenterTargetPosition() { return transform.position + Vector3.up; }
    public virtual Vector3 GetDirectionTargetPosition(int direction ) { return transform.position + directionVector[direction]; }
    public virtual void RevertBlock(Block block)
    {
        data = block.data;
        isClear = block.isClear;
        type = block.type;
    }
}
