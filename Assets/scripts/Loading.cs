using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Load()
    {
        int cur_level = PlayerPrefs.GetInt("cur_level");
        string progress_string = PlayerPrefs.GetString("progress");
        string[] progress = progress_string.Split();
        int theme;
        print(progress_string);
        if (progress[0] == "")
        {
            PlayerPrefs.SetString("progress", "0");
            SceneManager.LoadScene(Character.levels[0, 0]);
            theme = 0;
        }
        else
        {
            
            if (cur_level > Character.levels.GetLength(1)-1)
            {
                cur_level = 0;
                PlayerPrefs.SetInt("cur_level", cur_level);
                
                if (progress.Length >= Character.levels.GetLength(0))
                {
                    theme = Random.Range(0, Character.levels.GetLength(0));
                    PlayerPrefs.SetString("progress", theme.ToString());
                }
                else
                {
                    do
                    {
                        theme = Random.Range(0, Character.levels.GetLength(0));
                    } while (progress_string.IndexOf(theme.ToString())>=0);
                    PlayerPrefs.SetString("progress",progress_string+" "+ theme.ToString());
                }


            }
            else
            {
                theme = int.Parse(progress[progress.Length - 1]);
            }
            
        }
        SceneManager.LoadScene(Character.levels[theme, cur_level]);
    }

    // Update is called once per frame
    int counter = 0;
    void Update()
    {
        this.transform.Rotate(1f, 1f, 1f);
        counter++;
        if (counter > 60) Load();
    }
}
