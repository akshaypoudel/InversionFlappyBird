using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCameraController : MonoBehaviour
{
    public float speed;
    void Update()
    {
        if(Bird.GetInstance().state==Bird.State.Playing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z=-10f;
            mousePosition.x=0f;

            float clampedY = Mathf.Clamp(mousePosition.y,-50,50);
            Vector3 movePos=new Vector3(0,clampedY,-10);
            transform.position = Vector3.Lerp(transform.position,movePos,speed*Time.deltaTime);
        }
    }
}
