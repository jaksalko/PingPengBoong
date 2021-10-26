using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedBlock : Block
{
	public int count;

    public Material transparentMaterial;
	public Material crackerMaterial;


	public MeshRenderer[] crackerRenderer;
    public MeshFilter[] crackerMesh;
	public Mesh cracker1;
    public Mesh cracker2;
    public Mesh cracker3;

    public ParticleSystem cracker_particle;

    public AudioClip[] crackerSound;
    public AudioSource audioSource;
    public Transform crackerDebris;

    public override void Init(int block_num,int style)
    {
        base.Init(block_num,style);
        object_styles[style].SetActive(true);

        count = 0;
        if(block_num < BlockNumber.upperNormal)
        {
            count = block_num - BlockNumber.cracker_0;
        }
        else
        {
            count = block_num - BlockNumber.upperCracker_0;
        }

        if (count == 1)//1
        {
            for (int i = 0; i < crackerMesh.Length; i++)
            {
                crackerMesh[i].mesh = cracker2;
            }
        }
        else if (count == 2)//2
        {

            crackerMesh[5].mesh = cracker3;
        }
        else if(count == 3)//broken
        {
           
            for (int i = 0; i < crackerRenderer.Length; i++)
            {
                crackerRenderer[i].material = transparentMaterial;
                
            }
        }
        

    }

    

    private void OnTriggerEnter(Collider other)//들어오고
    {
        cracker_particle.Play();
    }
    
    private void OnTriggerExit(Collider other)//나갈 때 깨짐
    {
        /*
        if (other.gameObject.CompareTag("Leg"))
        {
            audioSource.clip = crackerSound[count];
            audioSource.Play();
            count++;//material setting
            data++;//block data setting


            Debug.Log("through the cracked block :" + count);
            SetMaterial();
        }
        */
    }

    public void BreakCrackerBlock()
    {
        audioSource.clip = crackerSound[count];
        audioSource.Play();
        count++;//material setting
        data++;//block data setting


        Debug.Log("through the cracked block :" + count);
        SetMaterial();
    }
    

	public void SetMaterial()
	{
        for (int i = 0; i < crackerRenderer.Length; i++)
        {
            crackerRenderer[i].material = crackerMaterial;
        }


        if (count == 0)
		{
			for (int i = 0; i < crackerMesh.Length; i++)
			{

				crackerMesh[i].mesh = cracker1;
			}
		}
		else if (count == 1)
		{
			for (int i = 0; i < crackerMesh.Length; i++)
			{

				crackerMesh[i].mesh = cracker2;
			}
		}
		else if (count == 2)
		{

            for (int i = 0; i < crackerMesh.Length; i++)
            {

                crackerMesh[i].mesh = cracker2;
            }

            crackerMesh[5].mesh = cracker3;
		}
        else if (count == 3)
        {
            for (int i = 0; i < crackerRenderer.Length; i++)
            {
                crackerRenderer[i].material = transparentMaterial;
            }
        }

        if (count == 3)
        {
            crackerDebris.gameObject.SetActive(true);
        }
        else
            crackerDebris.gameObject.SetActive(false);

    }

    public override void RevertBlock(Block block)
    {
        base.RevertBlock(block);
        CrackedBlock crackedBlock = (CrackedBlock)block;
        Debug.Log(transform.position + " is " + crackedBlock.count);
        if(crackedBlock.count != count)
        {
            count = crackedBlock.count;
            SetMaterial();
        }
        

    }
}
