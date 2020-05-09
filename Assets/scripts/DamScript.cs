using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamScript : MonoBehaviour
{

    
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


        

    }
    public void damUpdate(float player_weight)
    {
        T = player_weight / damHp;
        desiredSize = Mathf.Lerp(0f, 30f, T);
        transform.localScale = new Vector3(1, 1, desiredSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Molodec... slomal dambu... teper mozhesh poiti pososat sebe hui)");
        }
    }
}
