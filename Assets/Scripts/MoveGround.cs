using UnityEngine;
using System.Collections.Generic;

public class MoveGround : MonoBehaviour
{

    private const float GROUND_MOVE_SPEED=30f;
    private const float GROUND_DESTROY_POS=-200f;
    private const float GROUND_SIZE=200f;
    public float groundYPos;
    private List<Transform> groundList;
    private void Awake() {
        groundList=new List<Transform>();
        Transform ground1;
        ground1=Instantiate(GameAssets.GetInstance().pfGround,new Vector3(-30,groundYPos,0),Quaternion.identity);
        groundList.Add(ground1);
        ground1=Instantiate(GameAssets.GetInstance().pfGround,new Vector3(170,groundYPos,0),Quaternion.identity);
        groundList.Add(ground1);
        ground1=Instantiate(GameAssets.GetInstance().pfGround,new Vector3(370,groundYPos,0),Quaternion.identity);
        groundList.Add(ground1);
    }

    void Update()
    {   
        if(Bird.GetInstance().state==Bird.State.Playing)
        {
            HandleGroundMovement();
        }
    }
    private void HandleGroundMovement()
    {
        foreach(Transform groundTransform in groundList)
        {
            groundTransform.position += Vector3.left * GROUND_MOVE_SPEED * Time.deltaTime;
            if(groundTransform.position.x < GROUND_DESTROY_POS)
            {
                float rightPos = -100f;
                for(int i=0;i<groundList.Count;i++)
                {
                    if(groundList[i].position.x>rightPos)
                    {
                        rightPos=groundList[i].position.x;
                    }
                }
                groundTransform.position=new Vector3(rightPos+GROUND_SIZE,groundTransform.position.y,groundTransform.position.z);
            }
        }
    }
}
