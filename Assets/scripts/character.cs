using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    [SerializeField]
    public float
            forward_speed,
            player_speed,
            ejection_force,
            force_increment,
            min_velocity,
            start_weight,
            max_scale,
            max_scalable_weight;
    public GameObject left_border_object;
    public GameObject right_border_object;
    public DynamicJoystick dynamicJoystick;
    public GameObject waterUnit;
    public Text mainText;
    public GameObject SplashParticle;

    
    private bool level_started = false;
    [HideInInspector] public float weight;
    private float left_border;
    private float right_border;
    public float[] Levels = new float[5];
    private Rigidbody rb;
    private GameObject[] obstacles;
    private Renderer[] obstacles_renderer;
    private Obstacles[] obstacles_class;
    private Collider[] obstacles_collider;
    private Obstacles trigger;
    private GameObject SpawnerRef;
    // Start is called before the first frame update
    void Start()
    {
       
        //init character
        weight = start_weight;
        rb = this.GetComponent<Rigidbody>();

        //init borders
        left_border = left_border_object.transform.position.z;
        right_border = right_border_object.transform.position.z;

        //init obstacles
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        obstacles_class = new Obstacles[obstacles.Length];
        obstacles_renderer = new Renderer[obstacles.Length];
        obstacles_collider = new Collider[obstacles.Length];
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles_class[i] = obstacles[i].GetComponent<Obstacles>();
            obstacles_renderer[i] = obstacles[i].GetComponent<Renderer>();
            obstacles_collider[i] = obstacles[i].GetComponent<Collider>();
        }
        updateObstacles();
        SpawnerRef = GameObject.FindGameObjectsWithTag("Respawn")[0];

        //UnityEngine.Debug.Log(obstacles.Length);


    }

    // Update is called once per frame
    
    
    void Update()
    {
       
        
    }
    
    //Nobody knows when called FixedUpdate
    private void FixedUpdate()
    {

        if (level_started)
        {
            //move forward
            rb.velocity = new Vector3(forward_speed, rb.velocity.y, rb.velocity.z);//move forward

            //joystick
            Vector3 direction = Vector3.forward * dynamicJoystick.Horizontal;//read joystick
            rb.AddForce(direction * player_speed * Time.fixedDeltaTime, ForceMode.VelocityChange);//right-left acceleration

            //ejection
            if (this.transform.position.z < right_border)
            {
                rb.AddForce(0, 0, ejection_force + force_increment);
                force_increment += 0.03F;

            }
            else if (this.transform.position.z > left_border)
            {
                rb.AddForce(0, 0, -ejection_force + force_increment);
                force_increment -= 0.03F;
            }
            else force_increment = 0F;
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                level_started = true;
                mainText.text = "Weigth:" + weight.ToString();
            }
        }
       
      
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Instantiate<GameObject>(waterUnit, SpawnerRef.transform.position, SpawnerRef.transform.rotation);
            Instantiate<GameObject>(SplashParticle, transform.position, transform.rotation);
            trigger = other.GetComponent<Obstacles>();
            weight += Mathf.Lerp(trigger.max_given_weigth, trigger.min_given_weight,
             (weight - trigger.weight) / (trigger.max_character_weight - trigger.weight));

            mainText.text = "Weigth:" + weight.ToString();

            Destroy(other.gameObject);
            updateObstacles();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.AddForce(0, 200, 0);
        }
    }
    public void updateObstacles()
    {
    
        for (int i = 0; i < obstacles_class.Length; i++)
        {

            if (obstacles[i] != null)
            {
                if (obstacles_class[i].weight > this.weight)
                {
                    obstacles_renderer[i].material.EnableKeyword("_EMISSION");
                    obstacles_collider[i].isTrigger = false;

                }
                else
                {
                    obstacles_renderer[i].material.DisableKeyword("_EMISSION");
                    obstacles_collider[i].isTrigger = true;
                }
            }



        }
     

    }
}
