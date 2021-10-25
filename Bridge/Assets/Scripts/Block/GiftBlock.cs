using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBlock : Block
{

    public override void Init(int block_num, int style)
    {
        base.Init(block_num, style);
        isClear = false;
        object_styles[style].SetActive(true);
    }

    public override void RevertBlock(Block block)
    {
        base.RevertBlock(block);
        if(isClear)
        {
            object_styles[style].SetActive(false);
        }
        else
        {
            object_styles[style].SetActive(true);
        }
    }

    public override Vector3 GetCenterTargetPosition()
    {
        return transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        object_styles[style].SetActive(false);
    }
}
