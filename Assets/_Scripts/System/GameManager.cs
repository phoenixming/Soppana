using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : StaticInstance<GameManager>
{
    private float karmaScore = 0;

    public float KarmaScore { get => karmaScore; set => karmaScore = value; }

    public bool isPaulsed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RePlay()
    {
        SceneManager.LoadScene(0);
    }
}
