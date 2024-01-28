using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{

    public int playTime = 1;


    public List<float> karmaScore = new(5);
    public List<int> collectCount = new(5);
    public List<int> giveCount = new(5);
    public List<int> throwCount = new(5);

    public bool isPaulsed = false;

    public bool isEnd = false;

    public bool isTutorial = true;
    public bool isTutorialPhase1 = true;
    public bool isTutorialPhase2 = false;
    public bool isTutorialPhase3 = false;
    public bool isTutorialPhase4 = false;
    public bool isTutorialPhase5 = false;
    public bool isTutorialPhase6 = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdatePlayData()
    {


    }

    public void RePlay()
    {
        SceneManager.LoadScene(0);
        UIManager.Instance.HealthSlider.value = 100;
        TimerManager.Instance.ResetTimer();
        isPaulsed = false;
        playTime++;
        if (playTime > 5)
        {
            playTime = 1;
        }
        isEnd = false;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        isPaulsed = true;
    }

    public void Resume()
    {
        isPaulsed = false;
    }



}
