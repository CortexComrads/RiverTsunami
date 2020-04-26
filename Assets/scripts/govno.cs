using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class govno : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    
    public GameObject govneco;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = govneco.transform.position;  
    }
}
