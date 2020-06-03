using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
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
    public int increment_start;
    public int power_start;
    public int offline_start;
    public float increment_price_multiplier;
    public float power_price_multiplier;
    public float offline_price_multiplier;
    public float power_coefficient;
    public int offline_coefficient;
    
    public Vector3 max_trigger_scale;
    public GameObject left_border_object,
                      right_border_object;
    public DynamicJoystick dynamicJoystick;
    public GameObject waterUnit;
    public GameObject SplashParticle;
    public GameObject VCam;
    public GameObject Destro;
    public GameObject DestroSmall;
    public GameObject lvlUpFx;
    

    [HideInInspector] public float weight;
    [HideInInspector] public int enemyCount;
    
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
    private TextMeshProUGUI incrementPriceText;
    private TextMeshProUGUI offlinePriceText;
    private TextMeshProUGUI powerPriceText;
    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI moneyGainText;
    private TextMeshProUGUI moneyGainedText;
    private GameObject[] damPieces;
    private int damI;

    private bool level_complete;
    private int money, moneyGain = 0;
    private int increment_lvl;
    private int power_lvl;
    private int offline_lvl;
    private int increment_price;
    private int power_price;
    private int offline_price;
    private float FT;
    public readonly static int[,] levels = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } };
    private int cur_level;



    // Start is called before the first frame update
    void Start()
    {
       
        //init character
        weight = start_weight;
        rb = this.GetComponent<Rigidbody>();
        UiController = GameObject.FindGameObjectsWithTag("Ui")[0].GetComponent<UiController>();

        //init Dam
        damPieces = GameObject.FindGameObjectsWithTag("DamPieces");
        damI = damPieces.Length;

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
        LoadSaves();

    }
    void LoadSaves()
    {
        //setmoney
        moneyText = GameObject.FindGameObjectWithTag("Money").GetComponent<TextMeshProUGUI>();
        moneyGainText = GameObject.FindGameObjectWithTag("moneyGain").GetComponent<TextMeshProUGUI>();
        moneyGainedText = GameObject.FindGameObjectWithTag("moneyGained").GetComponent<TextMeshProUGUI>();
        money = PlayerPrefs.GetInt("money");
        moneyText.text = "$" + moneyConverter(money);
        moneyGainText.text = "$" + moneyConverter(moneyGain);

        //setmodificators
        incrementText = GameObject.FindGameObjectWithTag("increment").GetComponent<TextMeshProUGUI>();
        offlineText = GameObject.FindGameObjectWithTag("OflineEarnings").GetComponent<TextMeshProUGUI>();
        powerText = GameObject.FindGameObjectWithTag("power").GetComponent<TextMeshProUGUI>();
        incrementPriceText = GameObject.FindGameObjectWithTag("iPrice").GetComponent<TextMeshProUGUI>();
        offlinePriceText = GameObject.FindGameObjectWithTag("oPrice").GetComponent<TextMeshProUGUI>();
        powerPriceText = GameObject.FindGameObjectWithTag("pPrice").GetComponent<TextMeshProUGUI>();
        increment_lvl = PlayerPrefs.GetInt("increment");
        power_lvl = PlayerPrefs.GetInt("power");
        offline_lvl = PlayerPrefs.GetInt("offline");
        if (increment_lvl == 0) increment_lvl = 1;
        if (power_lvl == 0) power_lvl = 1;
        if (offline_lvl == 0) offline_lvl = 1;
        refreshPrice();
        updateEnemyCount();

        //offline earnings
        string last_time_str = PlayerPrefs.GetString("time");
        if (last_time_str != "")
        {
            DateTime last_time = DateTime.Parse(last_time_str);
            int offlineHours = (int)((DateTime.Now - last_time).TotalHours);
            offlineEarner(offlineHours);
        }
        PlayerPrefs.SetString("time",DateTime.Now.ToString());


        //setlevels
        cur_level = PlayerPrefs.GetInt("cur_level");

    

       
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
            for (int ii = 0; ii  < damI; ii ++)
            {
                if (damPieces[ii]!=null) 
                {
                    damPieces[ii].GetComponent<DamNew>().updateWeight(weight);
                }
            }
           
            trigger = other.GetComponent<Obstacles>();
            float weight_increment = Mathf.Lerp(trigger.max_given_weigth, 
                trigger.min_given_weight,
                (weight - trigger.weight) / (trigger.max_character_weight - trigger.weight));
            weight_increment += weight_increment * ((float)power_lvl * power_coefficient);
            weight += weight_increment;
            moneyGain += trigger.money;
            moneyGainText.text = "$" + moneyConverter(moneyGain);
        
        //mainText.text = "Weigth:" + weight.ToString();

        Destroy(other.gameObject);
            updateObstacles();

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
    public void updateEnemyCount()
    {
        int i0 = enemyCount;
        enemyCount = increment_lvl;
        for (int i = i0; i < increment_lvl - 1; i++)
            Instantiate<GameObject>(waterUnit,
                SpawnerRef.transform.position, SpawnerRef.transform.rotation);
        float alpha = enemyCount / max_scalable_enemies;
        GetComponent<SphereCollider>().radius = Mathf.Lerp(min_collision_radius, max_collision_radius, alpha);
        GetComponent<BoxCollider>().size = Vector3.Lerp(min_trigger_scale, max_trigger_scale, alpha);
        VCam.GetComponent<CamScript>().UpdateYpos(enemyCount);
    }


    public void restartPressed() 
    {
        if (level_complete)
        {
            PlayerPrefs.SetInt("cur_level", cur_level + 1);
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
        if (money >= increment_price)
        {
            money -= increment_price;
            PlayerPrefs.SetInt("money", money);
            increment_lvl++;
            PlayerPrefs.SetInt("increment", increment_lvl);
            refreshPrice();
            updateEnemyCount();
        }


    }
    public void powerPressed()
    {
        if (money >= power_price)
        {
            money -= power_price;
            PlayerPrefs.SetInt("money", money);
            power_lvl++;
            PlayerPrefs.SetInt("power", power_lvl);
            refreshPrice();
        }

    }
    public void offlinePressed()
    {
        if (money >= offline_price)
        {
            money -= offline_price;
            PlayerPrefs.SetInt("money", money);
            offline_lvl++;
            PlayerPrefs.SetInt("offline", offline_lvl);
            refreshPrice();
        }
    }
    public void refreshPrice()
    {
        moneyText.text = moneyConverter(money);
        incrementText.text = "lvl " + increment_lvl;
        powerText.text = "lvl " + power_lvl;
        offlineText.text = "lvl " + offline_lvl;
        increment_price = (int)(increment_start * Mathf.Pow((float)increment_price_multiplier, increment_lvl - 1));
        power_price = (int)(power_start * Mathf.Pow((float)power_price_multiplier, power_lvl - 1));
        offline_price = (int)(offline_start * Mathf.Pow((float)offline_price_multiplier, offline_lvl - 1));
        incrementPriceText.text = moneyConverter(increment_price);
        offlinePriceText.text = moneyConverter(offline_price);
        powerPriceText.text = moneyConverter(power_price);
        Instantiate<GameObject>(lvlUpFx, this.gameObject.transform.position, this.gameObject.transform.rotation);
    }
    public string moneyConverter(int money)
    {
        string k = "";
        float x = money;
        while (x >= 1000f)
        {
            x /= 1000f;
            k += "k";
        }
        return Mathf.Round(x * 100f) / 100f + k;
    }
    public void offlineEarner(int hours)
    {
        if (hours >=4)
        {
            int offline_money = offline_lvl * offline_coefficient;
            if (hours < 12) offline_money /= 2;
            if (hours < 6) offline_money /= 2;
            moneyGainedText.text = "$" + moneyConverter(offline_money);
            PlayerPrefs.SetInt("money", money+offline_money);
            
            UiController.ToggleFinishUi(true);
            UiController.toggleIngameUi(false);
        }
        
    }

}
