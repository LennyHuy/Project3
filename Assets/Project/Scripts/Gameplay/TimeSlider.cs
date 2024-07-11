using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    [SerializeField] private Slider timeSlider;
    [SerializeField] private float timeValue;
    [SerializeField] private GameManager gameManager;
    private float remainingTime;

    private bool stopTimer;
    void Start()
    {
        remainingTime = timeValue;
        timeSlider.maxValue = timeValue;
        timeSlider.value = remainingTime;
    }

    void Update()
    {
        if(GameManager.playerState == PlayerState.Die && GameManager.gameState == GameState.Stop) StartCounting();
        
        if(GameManager.playerState == PlayerState.Idle) remainingTime = timeValue;
    }
    public void StartCounting()
    {
        remainingTime -= Time.deltaTime;
        timeSlider.value = remainingTime;

        if (timeSlider.value == 0.0f)
        {
            gameManager.GameOver();
            GameManager.playerState = PlayerState.Idle;
        }
    }
}
