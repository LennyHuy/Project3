using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public static Player Instance;
    public static Rigidbody2D rigidBody2D;
    static Arrow arrow;

    [Header("Values")]
    [SerializeField] private float force;
    [SerializeField] private int reviveValue;
    [SerializeField] private float maxArrowSpeed;
    [SerializeField] private float arrowSpeedAcceleration;

    [Header("Gameobjects")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI reviveCostText;
    [SerializeField] private AudioClip groundFallClip;
    

    private int reviveCount = 1;
    [HideInInspector] public int reviveCost;

    private bool isNotRewarded;
    
    void Awake()
    {
        Instance = this;
        rigidBody2D = this.GetComponent<Rigidbody2D>();
        arrow = this.GetComponentInChildren<Arrow>();
    }

    public static void StartArrow()
    {
        arrow.TurnOn();
    }
    public static void EndArrow()
    {
        arrow.TurnOff();
    }
    public static void Jump()
    {
        GameManager.playerState = PlayerState.Jumping;
        arrow.TurnOff();
        Vector3 jumpForce = arrow.direction*Instance.force;
        rigidBody2D.AddForce(jumpForce);
    }
    public void Die()
    {
        reviveCost = reviveValue * reviveCount; //Double the value for every revives (Power the value if needed : (int)Mathf.Pow(2, reviveCount))
        UpdateReviveCostText();

        gameObject.SetActive(false);
        deathScreen.SetActive(true);

        GameManager.playerState = PlayerState.Die;
        GameManager.gameState = GameState.Stop;
    }
    public void Revive()
    {
    if (CurrencyManager.Instance.CanSubtractCurrency(reviveCost) )
    {
        CurrencyManager.Instance.SubtractCurrency(reviveCost);

        // Spawn at the previous platform
        Vector3 revivePoint = transform.position;
        revivePoint.x = respawnPoint.transform.position.x;
        revivePoint.y = respawnPoint.transform.position.y + 20f; // Spawn from the ceiling

        transform.position = revivePoint;
        gameObject.SetActive(true);

        StartCoroutine(WaitForClosing());
        GameManager.gameState = GameState.Play;

        reviveCount++;

    } else { Debug.Log("Not enough currency to revive");}
    }
    IEnumerator WaitForClosing()
    {
        yield return new WaitForSeconds(0.5f);
        deathScreen.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.contacts[0].normal == Vector2.up) //Check if player reach the flat top of the platform (not in the side)
        {
            SoundsManager.Instance.PlaySound(groundFallClip);

            GameManager.playerState = PlayerState.Idle;
            rigidBody2D.velocity = Vector3.zero;
            StartArrow();

            //Update the current respawn position
            respawnPoint.transform.position = transform.position; //Set the current respawn postion 

            
            if (Arrow.speedValue <= maxArrowSpeed) Arrow.speedValue += arrowSpeedAcceleration;
        }
    }
    void UpdateReviveCostText()
    {
        if(reviveCostText != null) reviveCostText.text = reviveCost.ToString();
    }
}
