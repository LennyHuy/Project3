using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public enum GameState{Play,Stop};
public enum PlayerState{Idle,Jumping,Die};
public enum AdState{Rewarded,NotRewarded};
public class GameManager : MonoBehaviour
{
    public static GameState gameState;
    public static PlayerState playerState;
    public static AdState adState;

    [Header("GAMEOBJECTS")]
    public LineRenderer lineRenderer;
    public GameObject FOV, player;
    public Player respawnPlayer;
    [SerializeField] private GameObject fadeSquare;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private AudioClip jumpAudio;

    [Header("PANELS")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject reviveOrNotPanel;
    [SerializeField] private GameObject gameOverPanel;

    [HideInInspector] private float fadeSpeedValue = 2f;
    void Awake()
    {
        Input.multiTouchEnabled = false;
        QualitySettings.vSyncCount = 0;
    }
    void Start()
    {
        CurrencyManager.Instance.SetCurrencyText(currencyText); //Update the currency value after reloading scene

        gameState = GameState.Stop;
        playerState = PlayerState.Idle;

        StartCoroutine(FadeBlackOutSquare(false));
    }
    private void Update()
    {
        if (player != null ) HandlePlayerInput();
    }

    // GameState : Stop
    public void EnterPlayMode()
    {
        StartCoroutine(WaitForFewSeconds());

        gameplayPanel.SetActive(true);
        menuPanel.SetActive(false);
    }
    IEnumerator WaitForFewSeconds()
    {
        yield return new WaitForSeconds(1f);
        gameState = GameState.Play;
    }

    // GameState : Play
    void HandlePlayerInput()
    {
    FOV.SetActive(false);
    lineRenderer.enabled = false;

        if (playerState == PlayerState.Idle && gameState == GameState.Play &&Input.GetMouseButton(0))
        {
            FOV.SetActive(true);
            lineRenderer.enabled = true;
            Player.StartArrow();
        }
        else if (playerState == PlayerState.Jumping && gameState == GameState.Play && Input.GetMouseButton(0)) Player.EndArrow(); //Prevent player from using the arrow while jumping
        
        else if (playerState == PlayerState.Idle && gameState == GameState.Play && Input.GetMouseButtonUp(0)) //Release the button to jump
        {
            Player.Jump();
            Player.EndArrow();
            SoundsManager.Instance.PlaySound(jumpAudio);
        }
    }
    
    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true)
    {
        Color objectColor = fadeSquare.GetComponent<Image>().color;
        float fadeAmount;
        float fadeSpeed = fadeSpeedValue;

        if(fadeToBlack)
        {
            while( fadeSquare.GetComponent<Image>().color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        } else 
        {
            while( fadeSquare.GetComponent<Image>().color.a >  0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                fadeSquare.GetComponent<Image>().color = objectColor;
                yield return null;
            }
        }
    }
    public IEnumerator WaitTillLoadScene()
    {
        StartCoroutine(FadeBlackOutSquare());
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Retry()
    {
        StartCoroutine(WaitTillLoadScene());
        Arrow.speedValue = 1f;
    }
    public void Revive()
    {
        respawnPlayer.Revive();
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        reviveOrNotPanel.SetActive(false);
    }
}
