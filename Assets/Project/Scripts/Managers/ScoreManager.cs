using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static float highScore = 0f;
    private float currentScore = 0;
    private bool isRunning = false;
    private bool isNewHighScore = false;
    private Vector2 lastPosition;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI highScoreText_Menu;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject newHighScoreText;

    void Start()
    {
        StartScore();
        highScore = PlayerPrefs.GetFloat("highScore", 0f);

        SetHighScoreText(highScoreText,highScore);
        SetHighScoreText(highScoreText_Menu,highScore);

        newHighScoreText.SetActive(false);
    }
    public void StartScore()
    {
        isRunning = true;
        currentScore = 0;
        lastPosition = player.transform.position;
    }
    public void CheckHighScore()
    {
        if(currentScore > PlayerPrefs.GetFloat("highScore",0)) 
        {
            highScore = currentScore;
            PlayerPrefs.SetFloat("highScore", highScore);

            SetHighScoreText(highScoreText,highScore);
            SetHighScoreText(highScoreText_Menu,highScore);

            isNewHighScore = true;
        }
    }
    public void SetHighScoreText(TextMeshProUGUI highScoreTextObject, float highScoreValue)
    {
        highScoreTextObject.text = $"Best: {Mathf.Round(highScoreValue)}m";
    }

     void Update()
    {
        if (isRunning)
        {
            float distance = player.transform.position.x - lastPosition.x;
            float roundedFloat = Mathf.Round(currentScore);
            if (distance > 0f) //Check if dis > 0 to avoid unecessary calculation while standing still
            {
                currentScore += distance;
                lastPosition = player.transform.position;
                scoreText.text = $"{roundedFloat}m";
            }
            CheckHighScore();
        }
        if (isNewHighScore)
        {
            newHighScoreText.SetActive(true);
        }
    }
}