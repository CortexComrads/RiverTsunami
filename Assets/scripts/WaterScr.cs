using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScr : MonoBehaviour
{
    public float T;
    public float Distance;
    public int level;
    public GameObject WaterFx;
    private float angle = 360;
    private float posX, posZ, posY;
    private float sameEnemy;
    private int ID;
    private int localID;
    private bool appeared = false;
    private Vector3 desiredPosition;
    private WaterSpawnerScr waterSpawner;
    private GameObject playerRef;
    private Character playerScript;
    


    int time;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerScript = playerRef.GetComponent<Character>();
        waterSpawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<WaterSpawnerScr>();
        //ID = GameObject.FindGameObjectsWithTag("Water").Length;

        for(int i = 0; i < waterSpawner.Levels.Length; i++)
        {
            if (waterSpawner.Levels[i] < waterSpawner.Levels_min_с[i])
            {
                localID = ++waterSpawner.Levels[i];
                level = i;
                if (level + 1 > waterSpawner.level_count) waterSpawner.newLevel();
                break;
            }
        }
        angle = angle * Mathf.Deg2Rad;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (appeared == false)
        {
            if (Vector3.Distance(transform.position, playerRef.transform.position) < 0.8f)
            {
                Instantiate(WaterFx, transform);
                appeared = true;
            }
        }
        

        sameEnemy = waterSpawner.Levels[level];

        posX = playerRef.transform.position.x + Mathf.Sin(angle / sameEnemy * localID) * ((Distance + (float)playerScript.enemyCount * 0.01f) - (level / 10f));
        posZ = playerRef.transform.position.z + Mathf.Cos(angle / sameEnemy * localID) * ((Distance + (float)playerScript.enemyCount * 0.01f) - (level / 10f));
        posY = playerRef.transform.position.y + level / 10f;
        posY += (Mathf.Sin(Time.time * 8 + localID * (level * 10 + 1))) / 20;


        desiredPosition = new Vector3(posX, posY, posZ);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, waterSpawner.cur_T[level]);

        
    }
}
