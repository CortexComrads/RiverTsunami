using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamScript : MonoBehaviour
{

    public GameObject playerRef;
    public float damHp;
    float T;
    float desiredSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        T = (playerRef.GetComponent<character>().weight / damHp);
       desiredSize = Mathf.Lerp(0f, 30f, T);
        transform.localScale = new Vector3(1,1,desiredSize);

       // print("desire size " + desiredSize + "the T " + T + "Player Weight " + playerRef.GetComponent<character>().weight + "Actual GO size " + transform.localScale);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Molodec... slomal dambu... teper mozhesh poiti pososat sebe hui)");
        }
    }
}
