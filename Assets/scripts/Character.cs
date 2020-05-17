using UnityEngine;
using UnityEngine.UI;


using TMPro;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    [SerializeField]
    public float
            forward_speed,
            player_speed,
            ejection_force,
            force_increment,
            min_speed,
            start_weight,
            max_collision_radius,
            max_scalable_enemies;
    public Vector3 max_trigger_scale;
    public GameObject left_border_object,
                      right_border_object;
    public DynamicJoystick dynamicJoystick;
    public GameObject waterUnit;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI moneyGainText;
    public TextMeshProUGUI moneyGainedText;
    public GameObject SplashParticle;
    public GameObject VCam;
    public DamScript dam;
    public GameObject Destro;
    public GameObject DestroSmall;
    public GameObject lvlUpFx;
    

    [HideInInspector] public float weight;
    [HideInInspector] public int enemyCount = 1;

    private bool level_started = false;
    private bool game_over=false;
    private float last_instatiate_score;
    private float left_border;
    private float right_border;
    private Vector3 min_trigger_scale;
    private float min_collision_radius;
    private Rigidbody rb;
    private GameObject[] obstacles;
    private Renderer[] obstacles_renderer;
    private Obstacles[] obstacles_class;
    private Collider[] obstacles_collider;
    private Obstacles trigger;
    private GameObject SpawnerRef;
    private UiController UiController;
    private TextMeshProUGUI incrementText;
    private TextMeshProUGUI offlineText;
    private TextMeshProUGUI powerText;
    private int increment_lvl;
    private int power_lvl;
    private int offline_lvl;
    private bool level_complete;
    private int money, moneyGain=0;
    private float FT;



    // Start is called before the first frame update
    void Start()
    {
        //init character
        weight = start_weight;
        rb = this.GetComponent<Rigidbody>();
        UiController = GameObject.FindGameObjectsWithTag("Ui")[0].GetComponent<UiController>();

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
        SpawnerRef = GameObject.FindGameObjectWithTag("Respawn");

        //init colliders
        min_collision_radius = GetComponent<SphereCollider>().radius;
        min_trigger_scale = GetComponent<BoxCollider>().size;


        //setmoney
        money = PlayerPrefs.GetInt("money");
        moneyText.text = "$" + moneyConverter(money);
        moneyGainText.text = "$"+moneyConverter(moneyGain);

        //setmodificators
        incrementText = GameObject.FindGameObjectWithTag("increment").GetComponent<TextMeshProUGUI>();
        offlineText = GameObject.FindGameObjectWithTag("OflineEarnings").GetComponent<TextMeshProUGUI>();
        powerText = GameObject.FindGameObjectWithTag("power").GetComponent<TextMeshProUGUI>();
        increment_lvl = PlayerPrefs.GetInt("increment");
        power_lvl= PlayerPrefs.GetInt("power");
        offline_lvl = PlayerPrefs.GetInt("offline");
        if (increment_lvl == 0) increment_lvl = 1;
        if (power_lvl == 0) power_lvl = 1;
        if (offline_lvl == 0) offline_lvl = 1;
        incrementText.text = "lvl " + increment_lvl;
        powerText.text = "lvl " + power_lvl;
        offlineText.text = "lvl " + offline_lvl;
    }
    
    // Update is called once per frame
    void Update()
    {
            
    }
    
    //Nobody knows when called FixedUpdate
    private void FixedUpdate()
    {
        //print(GameObject.FindGameObjectsWithTag("Water").Length);

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
                FT = (float)enemyCount / 100;
                ejection_force = Mathf.Lerp(7f, 1f, FT);
                rb.AddForce(0, 0, ejection_force + force_increment);
                force_increment += 0.035f;   

            }
            else if (this.transform.position.z > left_border)
            {
                FT = (float)enemyCount / 100;
                ejection_force = Mathf.Lerp(7f, 1f, FT);
                rb.AddForce(0, 0, -ejection_force + force_increment);
                force_increment -= 0.035f;
            }
            
            else force_increment = 0F;


        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle")&&other.GetComponent<Obstacles>().weight<=weight)
        {
           
            Instantiate<GameObject>(SplashParticle, transform.position, transform.rotation);
            if (other.GetComponent<Obstacles>().money == 500)
            Instantiate<GameObject>(Destro, other.transform.position, other.transform.rotation);
            if (other.GetComponent<Obstacles>().money == 300)
                Instantiate<GameObject>(DestroSmall, other.transform.position, other.transform.rotation);
            if (Mathf.Round(last_instatiate_score+1f)<=weight)
            {
                Instantiate<GameObject>(waterUnit, SpawnerRef.transform.position, SpawnerRef.transform.rotation);
                enemyCount++;
                last_instatiate_score = weight;
                float alpha = enemyCount / max_scalable_enemies;
                GetComponent<SphereCollider>().radius = Mathf.Lerp(min_collision_radius,max_collision_radius,alpha);
                GetComponent<BoxCollider>().size = Vector3.Lerp(min_trigger_scale,max_trigger_scale,alpha);
                VCam.GetComponent<CamScript>().UpdateYpos(enemyCount);
                
            }
           
            trigger = other.GetComponent<Obstacles>();
            weight += Mathf.Lerp(trigger.max_given_weigth, trigger.min_given_weight,
             (weight - trigger.weight) / (trigger.max_character_weight - trigger.weight));
            moneyGain += trigger.money;
            moneyGainText.text = "$" + moneyConverter(moneyGain);
        
        //mainText.text = "Weigth:" + weight.ToString();

        Destroy(other.gameObject);
            updateObstacles();
            dam.damUpdate(weight);
        }
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.AddForce(0, 125, 0);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (rb.velocity.x < min_speed)
            {
                gameOver(false);
            }
        }
    }



    public void gameOver(bool isSucces)
    {
        GameObject[] waters = GameObject.FindGameObjectsWithTag("Water");
        for (int i = 0; i < waters.Length; i++)
        {
            waters[i].GetComponent<WaterScr>().enabled = false;
            waters[i].GetComponent<BoxCollider>().enabled = true;
            waters[i].GetComponent<Rigidbody>().useGravity = true;

        }
        game_over = true;
        level_started = false;
        moneyGainedText.text = moneyConverter(moneyGain);
        UiController.ToggleFinishUi(isSucces);
        UiController.toggleIngameUi(false);
        level_complete = isSucces;
        PlayerPrefs.SetInt("money",money+moneyGain);
        
    }




    public void updateObstacles()
    {
        for (int i = 0; i < obstacles_class.Length; i++)
        {
            if (obstacles[i] != null)
            {
                if (obstacles_class[i].weight > this.weight)
                {
                    obstacles_renderer[i].material.DisableKeyword("_EMISSION");
                    obstacles_collider[i].isTrigger = false;
                }
                else
                {
                    obstacles_renderer[i].material.EnableKeyword("_EMISSION");
                    obstacles_collider[i].isTrigger = true;
                }
            }
        }
    }


    public void restartPressed() 
    {
        int indx = SceneManager.GetActiveScene().buildIndex;
        if (level_complete) indx++;
        SceneManager.LoadScene(indx);
    }
    public void startPressed()
    {
        if (!level_started&&!game_over)
        {
            level_started = true;
            // mainText.text = "Weigth:" + weight.ToString();
            UiController.toggleIngameUi(true);
        }
        
    }
    public void incrementPressed()
    {
        money -= 250;
        moneyText.text = moneyConverter(money);
        PlayerPrefs.SetInt("money", money);
        increment_lvl++;
        PlayerPrefs.SetInt("increment", increment_lvl);
        incrementText.text = "lvl " + increment_lvl;
        Instantiate<GameObject>(lvlUpFx, this.gameObject.transform.position, this.gameObject.transform.rotation);
        
    }
    public void powerPressed()
    {
        money -= 250;
        moneyText.text = moneyConverter(money);
        PlayerPrefs.SetInt("money", money);
        power_lvl++;
        PlayerPrefs.SetInt("power", power_lvl);
        powerText.text = "lvl " + power_lvl;
        Instantiate<GameObject>(lvlUpFx, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
    public void offlinePressed()
    {
        money -= 250;
        moneyText.text = moneyConverter(money);
        PlayerPrefs.SetInt("money", money);
        offline_lvl++;
        PlayerPrefs.SetInt("offline", offline_lvl);
        offlineText.text = "lvl " + offline_lvl;
        Instantiate<GameObject>(lvlUpFx, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
    public string moneyConverter(int x)
    {
        return (x < 1000) ? x.ToString() : (x / 1000.0)+"k";
    }
    
}
