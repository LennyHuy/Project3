using UnityEngine;

public class Coins : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Add coin value to currency manager
            CurrencyManager.Instance.AddCurrency(coinValue);

            // Play sound 
            SoundsManager.Instance.PlaySound(clip);

            // Destroy the coin gameobject
            Destroy(gameObject);
        }
    }
}
