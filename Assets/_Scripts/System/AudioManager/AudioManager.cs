using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : StaticInstance<LightManager>
{
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource soundEffectsAudioSource;

    [Header("Clips")]
    [SerializeField] private AudioClip eatSound;
    [SerializeField] private AudioClip storeSound;
    [SerializeField] private AudioClip giveFruitSound;
    [SerializeField] private AudioClip throwFruitHitSound;
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.StoreFruit += PlayStoreFruitSound;
        PlayerController.GiveFirstFruit += PlayGiveFruitSound;
        PlayerController.GiveSecondFruit += PlayGiveFruitSound;
        UIManager.EatFruit += PlayEatFruitSound;
        ThrowFruit.ThrowFruitHit += PlayHitSound;
    }

    private void OnDestroy()
    {
        PlayerController.StoreFruit -= PlayStoreFruitSound;
        PlayerController.GiveFirstFruit -= PlayGiveFruitSound;
        PlayerController.GiveSecondFruit -= PlayGiveFruitSound;
        UIManager.EatFruit -= PlayEatFruitSound;
        ThrowFruit.ThrowFruitHit -= PlayHitSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isPaulsed)
        {
            backgroundAudioSource.Stop();
        }
    }

    private void PlayStoreFruitSound(FruitPrefab fruit)
    {
        soundEffectsAudioSource.clip = storeSound;
        soundEffectsAudioSource.Play();
    }

    private void PlayGiveFruitSound(NPC npc)
    {
        soundEffectsAudioSource.clip = giveFruitSound;
        soundEffectsAudioSource.Play();
    }

    private void PlayEatFruitSound()
    {
        soundEffectsAudioSource.clip = eatSound;
        soundEffectsAudioSource.Play();
    }

    private void PlayHitSound()
    {
        soundEffectsAudioSource.clip = throwFruitHitSound;
        soundEffectsAudioSource.Play();
    }

}
