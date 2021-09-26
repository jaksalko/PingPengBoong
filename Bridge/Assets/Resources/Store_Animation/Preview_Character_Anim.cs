using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview_Character_Anim : MonoBehaviour
{

	public Animator previewCharAnim;

	private int randomNum = 0;
	private float horizontalSpeed = 2.0F;
	private bool isClicked = false;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(isClicked)
		{
			float h = horizontalSpeed * Input.GetAxis("Mouse X");
			transform.Rotate(0, -h, 0);
		}

	}


	void OnMouseDown()
	{
		// isClicked = true;
	}

	void OnMouseDrag()
	{
		isClicked = true;
	}

	void OnMouseUp()
	{
		isClicked = false;
	}

	void OnMouseUpAsButton()
	{
		isClicked = false;

		if(randomNum == 0)
		{
			StartCoroutine("RandomAnim");
		}

	}

	IEnumerator RandomAnim()
	{
		randomNum = Random.Range(1, 7);
		previewCharAnim.SetInteger("RandomNum", randomNum);

		yield return new WaitForSecondsRealtime(3.0f);

		randomNum = 0;
		previewCharAnim.SetInteger("RandomNum", 0);
	}
}
