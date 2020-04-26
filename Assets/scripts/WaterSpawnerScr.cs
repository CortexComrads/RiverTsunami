using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSpawnerScr : MonoBehaviour
{
    // Start is called before the first frame update
    public int[] Levels;
    public int[] Levels_min_с;
    public int[] Levels_max_c;
    public float[] T;
    [HideInInspector] public float[] cur_T;
    [HideInInspector] public int level_count=0;


    void Start()
    {
        cur_T = new float[T.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void newLevel()
    {
        level_count++;
        for (int i = 0, j = level_count - 1; i < level_count; i++, j--)
            cur_T[j] = T[i];
    }
}
