using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMover : MonoBehaviour
{
    public float CLOUD_MOVE_SPEED=10f;
    private const float CLOUD_SIZE=301f;
    public float CLOUD_DESTROY_POS=-310f;
    public float rightPos1=240f;
    private const float CLOUD_SPAWN_POS=280f;


    private List<Transform> cloudList;
    private void Awake() 
    {
        cloudList=new List<Transform>();
        Transform cloud;
        cloud=Instantiate(GameAssets.GetInstance().pfClouds,new Vector3(0,0,0),Quaternion.identity);
        cloudList.Add(cloud);
        cloud=Instantiate(GameAssets.GetInstance().pfClouds,new Vector3(CLOUD_SPAWN_POS,0,0),Quaternion.identity);
        cloudList.Add(cloud);
    }
    private void Update()
    {
        if(Bird.GetInstance().state==Bird.State.Playing) 
            HandleCloudMovement();
    }

    private void HandleCloudMovement()
    {
        foreach (Transform cloudTransformer in cloudList)
        {
            cloudTransformer.position += Vector3.left * CLOUD_MOVE_SPEED * Time.deltaTime;
            if (cloudTransformer.position.x < CLOUD_DESTROY_POS)
            {
                float rightPos = rightPos1;
                for (int i = 0; i < cloudList.Count; i++)
                {
                    if (cloudList[i].position.x > rightPos)
                    {
                        rightPos = cloudList[i].position.x;
                    }
                    cloudTransformer.position = new Vector3(rightPos, cloudTransformer.position.y, cloudTransformer.position.z);
                }
            }
        }
    }
}
