using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager Instance;
    [SerializeField] private AudioSource effectSource;
    public static int toggleValue = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }
    
    public void ToggleEffects()
    {
        effectSource.mute = !effectSource.mute;
        toggleValue *= -1;
    }

}
