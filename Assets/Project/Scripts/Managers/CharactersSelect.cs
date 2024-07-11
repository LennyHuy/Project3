using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharactersSelect : MonoBehaviour
{
    public GameObject[] skins;
    public int selectedCharacter;
    public Characters[] characters;

    public Button unlockButton;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private AudioClip selectAudio;
    public void Awake()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter",0);
        foreach (GameObject player in skins)
            player.SetActive(false);

        skins[selectedCharacter].SetActive(true);

        foreach(Characters c in characters)
        {
            if(c.price == 0) c.isUnlocked = true;
            else 
            {
                c.isUnlocked = PlayerPrefs.GetInt(c.name,0) == 0 ? false : true;
            }
        }
    }
    // void Update ()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         // Reset your data here
    //         PlayerPrefs.DeleteAll();
    //         Debug.Log("Data has been reset.");
    //     }
    // }
    
    public void ChangeNext()
    {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if (selectedCharacter == skins.Length) selectedCharacter = 0;

        skins[selectedCharacter].SetActive(true);

        if(characters[selectedCharacter].isUnlocked)
            PlayerPrefs.SetInt("SelectedCharacter",selectedCharacter);

        SoundsManager.Instance.PlaySound(selectAudio);

        UpdateUI();
    }
    public void ChangePrevious()
    {
        skins[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter == -1) selectedCharacter = skins.Length - 1;

        skins[selectedCharacter].SetActive(true);

        if(characters[selectedCharacter].isUnlocked)
            PlayerPrefs.SetInt("SelectedCharacter",selectedCharacter);

        SoundsManager.Instance.PlaySound(selectAudio);

        UpdateUI();
    }
    public void EnterShopMode()
    {
        skins[0].SetActive(false); //Set character to the current locked character (if it is)
        skins[selectedCharacter].SetActive(true);

        menuPanel.SetActive(false);
        shopPanel.SetActive(true);
    }
    public void ExitShopMode()
    {
        if(!characters[selectedCharacter].isUnlocked)
        {
            skins[0].SetActive(true); //Set character to default if the current is not unlocked
            skins[selectedCharacter].SetActive(false);
        }

        menuPanel.SetActive(true);
        shopPanel.SetActive(false);
    }
    public void UpdateUI()
    {
        if (characters[selectedCharacter].isUnlocked == true)
        {
            unlockButton.gameObject.SetActive(false);
            unlockButton.interactable = false;
        }
            
        else
        {
            unlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "" + characters[selectedCharacter].price;
            if (!CurrencyManager.Instance.CanSubtractCurrency(characters[selectedCharacter].price))
            {
                unlockButton.gameObject.SetActive(true);
                unlockButton.interactable = false;
            }
            else
            {
                unlockButton.gameObject.SetActive(true);
                unlockButton.interactable = true;
            }
        }
    }
    public void Unlock()
    {
        int price = characters[selectedCharacter].price;
        CurrencyManager.Instance.SubtractCurrency(price);

        PlayerPrefs.SetInt(characters[selectedCharacter].name, 1);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);

        characters[selectedCharacter].isUnlocked = true;
        UpdateUI();
    }
}
