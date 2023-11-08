using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : StaticInstance<LightManager>
{
    [SerializeField] private Color lightColor;
    [SerializeField] private Light2D light2D;

    public Light2D Light2D { get => light2D; set => light2D = value; }
    public Color LightColor { get => lightColor; set => lightColor = value; }

    // Start is called before the first frame update
    void Start()
    {
        Light2D.color = LightColor;
        TimerManager.ChangeLightColor += ChangeColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        TimerManager.ChangeLightColor -= ChangeColor;
    }

    public void ChangeColor(bool increase)
    {
        if (increase)
        {
            lightColor.r += 20f/255f;
            lightColor.g += 20f/255f;
            lightColor.b += 20f/255f;
        }
        else
        {
            lightColor.r -= 20f/255f;
            lightColor.g -= 20f/255f;
            lightColor.b -= 20f/255f;
        }
        light2D.color = LightColor;
    }
}
