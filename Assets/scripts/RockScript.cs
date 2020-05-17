using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockScript : MonoBehaviour
{
    private GameObject controllerRef; 
    // Start is called before the first frame update
    void Start()
    {
        controllerRef = GameObject.FindGameObjectWithTag("GameController");
        gameObject.GetComponent<Renderer>().material = controllerRef.GetComponent<game_controller>().rocksMat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
