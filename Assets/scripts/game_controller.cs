using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_controller : MonoBehaviour
{
    [SerializeField]
    public GameObject[] chunks;
    public GameObject object112;
    public Material material;
    Vector2 river_offset;
    // Start is called before the first frame update
    void Start()
    {
      
       
    }

    // Update is called once per frame
    void Update()
    {
        //object112.GetComponentInChildren<Renderer>().sharedMaterial.DisableKeyword("_EMISSION");
    }
}
