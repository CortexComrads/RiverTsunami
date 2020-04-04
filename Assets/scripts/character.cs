using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class character : MonoBehaviour
{

    
    
   

    



    [SerializeField]

    public float forward_speed;
    public float player_speed;
    public DynamicJoystick dynamicJoystick;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
        rb.velocity = new Vector3(forward_speed, rb.velocity.y, rb.velocity.z);//move forward

        Vector3 direction = Vector3.right * dynamicJoystick.Vertical + Vector3.forward * dynamicJoystick.Horizontal;//read joystick
        rb.AddForce(direction * player_speed * Time.fixedDeltaTime, ForceMode.VelocityChange);//right-left acceleration



    }
}
