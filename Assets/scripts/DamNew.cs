using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamNew : MonoBehaviour
{

    public float reqWeight;
    public GameObject destruction;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateWeight(float plWeight) 
    {
        if(plWeight > reqWeight) 
        {
            GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
            GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player")) 
        {
        Instantiate<GameObject>(destruction, transform.position,transform.rotation);
        Destroy(this.gameObject);
        other.gameObject.GetComponent<Character>().gameOver(true);
        }
    }
}
