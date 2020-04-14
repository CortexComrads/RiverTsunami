using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScr : MonoBehaviour
{
    public float T;
    public float speed;
    public float Distance;
    public float level;
    float angle = 360;
    float posX, posZ, posY;
    float sameEnemy;
    int ID;
    int localID;
    int enemyCount;
    bool appeared=false;
    public GameObject WaterFx;
    GameObject playerRef;
    public Vector3 desiredPosition;


    int y;
    // Start is called before the first frame update
    void Start()
    {
        
        playerRef = GameObject.FindGameObjectsWithTag("Player")[0];
        ID = GameObject.FindGameObjectsWithTag("Water").Length;

        if (ID < 15)
        {
            level = 0;
           //T = Random.Range(0.14f, 0.14f);
            T = 0.13f;
            playerRef.GetComponent<character>().Levels[(int)level] ++;
            localID = ID - 0;
        }
        if (ID >= 15 && ID <= 25) 
        {
            level = 1;
            //T = Random.Range(0.1f, 0.12f);
            T = 0.175f;
            playerRef.GetComponent<character>().Levels[(int)level]++;
            localID = ID - 20;
        }
        if (ID > 25 && ID <= 33) 
        {
            level = 2;
            //T = Random.Range(0.13f, 0.15f); 
            T = 0.2f;
            playerRef.GetComponent<character>().Levels[(int)level]++;
            localID = ID - 35;
        }
        if (ID > 33 && ID < 40)
        {
            level = 3;
            //T = Random.Range(0.17f, 0.2f);
            T = 0.25f;
            playerRef.GetComponent<character>().Levels[(int)level]++;
            localID = ID - 45;
        }

        


        angle = angle * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void Update()
    {
        if (appeared == false)
        {
            if (Vector3.Distance(transform.position, playerRef.transform.position) < 0.8f)
            {
                Instantiate(WaterFx, transform);
                appeared = true;
            }
        }
        enemyCount = GameObject.FindGameObjectsWithTag("Water").Length;
   
        sameEnemy = playerRef.GetComponent<character>().Levels[(int)level];
              
            posX = playerRef.transform.position.x + Mathf.Sin(angle / sameEnemy * localID) * ((Distance + (float)enemyCount * 0.017f) - (level/ 10f));
            posZ = playerRef.transform.position.z + Mathf.Cos(angle / sameEnemy * localID) * ((Distance + (float)enemyCount * 0.017f) - (level/ 10f));

        posY = playerRef.transform.position.y + (float)level/ 10f;


        desiredPosition = new Vector3(posX , posY, posZ);
        
        transform.position = Vector3.Slerp(transform.position, desiredPosition, T);

        /*if (ID == 1)
            print(playerRef.GetComponent<Rigidbody>().velocity);*/
    }
}
