using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamDestScr : MonoBehaviour
{
    public GameObject VFX;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(VFX, transform.position, transform.rotation).transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

    }

    // Update is called once per frame
    void Update()
    {

        this.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
        if (this.transform.localScale.z <= 0.05)
        {
            Destroy(this.gameObject);

        }
    }
}
