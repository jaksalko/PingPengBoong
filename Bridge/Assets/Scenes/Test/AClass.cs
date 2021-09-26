using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AClass : MonoBehaviour
{

    public Transform idle1;
    public Vector3 start;
    public Vector3 end;
    public float tweenTime;
    private void Start()
    {
        StartCoroutine(MoveTracker());
    }

    IEnumerator MoveTracker()
    {
        float t = 0;

        Vector3 pos = start;

        idle1.SetParent(this.transform);
        //float lean = (from.position.x - to.position.x) / (from.position.y - to.position.y);
        //lean = -1/lean;
        Vector3 lean = (end - start).normalized;
        Quaternion qRotate = Quaternion.AngleAxis(90.0f, Vector3.left); //


        Vector3 direction = (end - start).normalized;
        // 정면을 기준으로 한다면 transform.forward; 를 입렵하면 된다.

        var quaternion = Quaternion.Euler(0, 0, 90);
        Vector3 newDirection = quaternion * direction;


        while (t <= tweenTime)
        {
            t += Time.deltaTime;

            pos = Vector3.Lerp(start, end, t / tweenTime);
            this.transform.position = pos;

            idle1.localPosition = newDirection;
            

            yield return null;
        }
    }

}
