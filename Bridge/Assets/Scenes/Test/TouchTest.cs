using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class TouchTest : MonoBehaviour
{
    public float sensitivity;
    bool touchstream = false;
    int touchValue = 0;
    public Transform target;
    public Transform camera_transform;
    public Transform camera_temp_transform;
    Vector2 temp;

    float gap;
    // Start is called before the first frame update
    void Start()
    {
        Input.simulateMouseWithTouches = true;

        float distance_temp = Vector3.Distance(target.position, camera_temp_transform.position);
        float distance_camera = Vector3.Distance(target.position, camera_transform.position);
        gap = distance_camera / distance_temp;
        this.UpdateAsObservable()
            .Where(_ => !touchstream)
                
              .Where(_ => Input.touchCount >= 1)
              .Where(_ => Input.GetTouch(0).phase == TouchPhase.Began)
              .Select(count => Input.touchCount)
              .Subscribe(count => StartCoroutine(TouchStart()));

        this.UpdateAsObservable()
            .Where(_ => touchstream)

              .Where(_ => Input.touchCount >= 1)
              .Where(_ => Input.GetTouch(0).phase == TouchPhase.Ended)
              .Select(count => Input.touchCount)
              .Subscribe(count => TouchEnd());

        this.UpdateAsObservable()
            .Where(_ => touchstream)
            .Where(_ => touchValue == Input.touchCount)
              .Where(_ => Input.touchCount >= 1)
              .Where(_ => Input.GetTouch(0).phase == TouchPhase.Moved)
              .Select(count => Input.touchCount)
              .Subscribe(count => Moved(Input.GetTouch(0).position));

        /*
        this.UpdateAsObservable()
             
              .Where(_ => Input.touchCount >= 2)
              .Subscribe(_ => Debug.Log("two"));
        this.UpdateAsObservable()
             .Where(_ => Input.GetMouseButton(0))
              .Where(_ => Input.touchCount == 1)
              .Subscribe(_ => Debug.Log("one"));
        */
    }

    void Moved(Vector2 touchPosition)
    {
        switch(touchValue)
        {
            case 1:
                Debug.Log("make");
                break;
            case 2:
                CameraMove(touchPosition);
                break;
                
        }
    }

    void CameraMove(Vector2 touchPosition)
    {
        Vector2 gap = touchPosition - temp;
        Debug.Log(gap);
        float x = gap.x;
        float z = gap.y;
        /*
        camera_transform.position = new Vector3(camera_transform.position.x + gap.x, camera_transform.position.y, camera_transform.position.z + gap.y);
        temp = touchPosition;*/
        temp = touchPosition;

        
        
        
        camera_temp_transform.RotateAround(target.position,
                                    camera_transform.up,
                                    x* sensitivity);
        camera_temp_transform.RotateAround(target.position,
                                        camera_transform.right,
                                        -z* sensitivity);


        if(camera_temp_transform.position.y < 1 || camera_temp_transform.position.y > 14)
        {
            camera_temp_transform.position = camera_transform.position;
            camera_transform.rotation = camera_transform.rotation;
        }
        else
        {
            camera_transform.position = camera_temp_transform.position;
            camera_transform.rotation = camera_temp_transform.rotation;
        }
        
        camera_transform.LookAt(target);
    }

    IEnumerator TouchStart()//스타트는 한번 호출될수 있지1
    {
        yield return new WaitForSeconds(0.1f);
        int value = Input.touchCount;
        if(value > 0)
        {
            temp = Input.GetTouch(0).position;
            Debug.Log("began! touch count : " + Input.touchCount);
            touchValue = value;
            touchstream = true;
        }
        
    }

    void TouchEnd()//End는 손가락 갯수에 따라 여러번 실행될 수 있음을 유의
    {

        touchstream = false;
        Debug.Log("end! touch count : ");



    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp_pos = camera_temp_transform.position;

        camera_temp_transform.RotateAround(target.position,
                                    target.up,
                                    sensitivity);

        camera_temp_transform.RotateAround(target.position,
                                    target.right,
                                    sensitivity);


        temp_pos = temp_pos - camera_temp_transform.position;
        temp_pos = temp_pos * gap;
        camera_transform.position -= temp_pos;
        camera_transform.rotation = camera_temp_transform.rotation;
    }
}
