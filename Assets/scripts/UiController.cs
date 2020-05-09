using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiController : MonoBehaviour
{
    public Slider slider;
    public GameObject playerRef;
    public GameObject damRef;
    public GameObject beginText;
    public GameObject Bar;
    public GameObject LoseScreen;
    public GameObject UpgradeMenu;
    public GameObject UpperUi;
    public TextMeshProUGUI RestartText;
    private float barT;
    private float appDelay = 0.2f;
    Sprite sprite;
    bool activation;
    private Vector3 finPos = new Vector3(0f, 990f, 0f);
    private Vector3 iniPos = new Vector3(0f, 1550f, 0f);
    private Vector3 downFinPos = new Vector3(0f, -1400f, 0f);
    int textTweenID;
    
    // Start is called before the first frame update
    void Start()
    {
        toggleIngameUi(false);
        textTweenID = LeanTween.alphaText(beginText.GetComponent<RectTransform>(), 1, 0.7f).setLoopPingPong().id;

    }

    // Update is called once per frame
    void Update()
    {
        if (activation == true) 
        {
            barT = playerRef.GetComponent<Character>().weight / damRef.GetComponent<DamScript>().damHp;
            Bar.gameObject.GetComponent<Slider>().value = barT;
        }
    }


    public void toggleIngameUi(bool state) 
    {
        if (state == true)
        {
            if (LeanTween.isTweening(textTweenID))
            {
                LeanTween.cancel(textTweenID);
            }
                activation = state;
            LeanTween.moveLocal(Bar, finPos, appDelay);
            LeanTween.alphaText(beginText.GetComponent<RectTransform>(), 0, appDelay);
            LeanTween.moveLocal(UpgradeMenu,downFinPos,appDelay);
            LeanTween.moveLocal(UpperUi, iniPos , appDelay);
            
        }
        else 
        {
            activation = state;
            LeanTween.moveLocal(Bar.gameObject, iniPos, 0.2f);
            
        }

    }
    public void ToggleFinishUi(bool level_complete) 
    {
        if(level_complete)RestartText.text = "Continue!";
        LeanTween.moveLocal(LoseScreen, new Vector3(0f, 0f, 0f) , 0.8f);
    }
}
