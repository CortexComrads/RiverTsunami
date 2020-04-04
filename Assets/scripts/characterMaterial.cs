using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Vector2 material_speed;
    private Vector2 texture_offset;
    
    void Start()
    {
        texture_offset = this.GetComponent<Renderer>().material.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        texture_offset += material_speed;
        this.GetComponent<Renderer>().material.SetTextureOffset("_MainTex",texture_offset);
    }
}
