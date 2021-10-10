using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParfaitBlock : Block
{
    public Animator iceBox;
    public ParticleSystem[] activeParticle;//parfait light

    public enum State
    {
        inactive,//비활성화 상태
        active,//활성화
        clear//이미 먹음
        
    }
    public int sequence;
    public State state;
    
	public AudioClip meltSound;
    public override void Init(int block_num,int style)
    {
        isClear = false;
        base.Init(block_num,style);

        sequence = block_num % 10 - 1;
        if(GameController.instance != null)
            GameController.instance.mapLoader.parfaitBlock[sequence] = this;
        object_styles[sequence].SetActive(true);

        state = State.inactive;

        if (sequence == 0)
            state = State.active;
        else
            state = State.inactive;

        SetParfait();

    }

    public override void RevertBlock(Block block)
    {
        ParfaitBlock parfaitBlock = (ParfaitBlock)block;
        base.RevertBlock(block);

        state = parfaitBlock.state;
        SetParfait();

    }

    public void SetParfait()
    {
        switch(state)
        {
            case State.inactive:
                DeActivate();
                break;
            case State.active:
                Activate();
                break;
            case State.clear:
                ClearParfait();
                break;
        }
    }


    public void ActiveNextParfait()
    {
        state = State.clear;
        SetParfait();

        if(sequence < 3)
        {
            ParfaitBlock nextParfaitBlock = GameController.instance.mapLoader.parfaitBlock[sequence + 1];
            nextParfaitBlock.state = State.active;
            nextParfaitBlock.SetParfait();           
        }

        
        // Destroy(gameObject);
    }

    

	public void ClearParfait()
    {
        object_styles[sequence].SetActive(false);
        iceBox.gameObject.SetActive(false);
        for (int i = 0; i < activeParticle.Length; i++)
        {
            activeParticle[i].Stop();
        }

        GetComponent<BoxCollider>().enabled = false;
    }

    public void DeActivate()//얼려있는 상태로 돌아가기
    {
        object_styles[sequence].SetActive(true);//오브젝트가 보여야 하므로 true
        iceBox.gameObject.SetActive(true);
        iceBox.SetBool("melt", false);


        for (int i = 0; i < activeParticle.Length; i++)
        {
            activeParticle[i].Stop();
        }

        GetComponent<BoxCollider>().enabled = true;
    }

    public void Activate()//얼음이 녹은 상태로
    {
        object_styles[sequence].SetActive(true);//오브젝트가 보여야 하므로 true

        Debug.Log("activate");
        iceBox.gameObject.SetActive(true);
        iceBox.SetBool("melt", true);
        //GetComponent<BoxCollider>().enabled = true;
        if (!GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = meltSound;
            GetComponent<AudioSource>().Play();
        }

        for (int i = 0; i < activeParticle.Length; i++)
        {
            activeParticle[i].Play();
        }

        GetComponent<BoxCollider>().enabled = true;

    }

    public override Vector3 GetCenterTargetPosition()
    {
        return transform.position;
    }
}
