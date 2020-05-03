using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamScript : MonoBehaviour
{
    int maxPlayerWeight;
    int currentPlayerWeight;
    private GameObject WaterSpawnerRef;
    private WaterSpawnerScr WScriptRef;
    private float startPos = 1.4f;
    private float endPos = 2f;
    private float T;
    private float desiredPosY;
    private Vector3 desiredPos;
    // Start is called before the first frame update
    void Start()
    {
        WaterSpawnerRef = GameObject.FindGameObjectsWithTag("Respawn")[0];
        WScriptRef = WaterSpawnerRef.GetComponent<WaterSpawnerScr>();
        for (int i = 0; i < WScriptRef.Levels.Length; i++)
        {
            maxPlayerWeight += WScriptRef.Levels_min_с[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* if(WaterSpawnerRef !=  null)
         print(WaterSpawnerRef.GetComponent<WaterSpawnerScr>().Levels[0]);*/
    }

    public void UpdateYpos(int count) 
    {
        if (currentPlayerWeight < maxPlayerWeight) 
        {
            currentPlayerWeight = count;
            T =  currentPlayerWeight / (float)maxPlayerWeight;
            desiredPosY = Mathf.Lerp(startPos, endPos , T);
            desiredPos = new Vector3(-2, desiredPosY, 0);
            this.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = desiredPos;
            
        }
    } 
}
