using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NVIDIA.Flex;

public class character : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Rigidbody>().AddForce(new Vector3(10, 0, 0));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
