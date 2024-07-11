using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenesSelect : MonoBehaviour
{
    public GameObject[] skins;
    public int selectedScene;
    public Characters[] scenes;

    public Button unlockButton;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private AudioClip selectAudio;
    public void Awake()
    {
        selectedScene = PlayerPrefs.GetInt("SelectedScene",0);
        foreach (GameObject player in skins)
            player.SetActive(false);

        skins[selectedScene].SetActive(true);

        foreach(Characters c in scenes)
        {
            if(c.price == 0) c.isUnlocked = true;
            else 
            {
                c.isUnlocked = PlayerPrefs.GetInt(c.name,0) == 0 ? false : true;
            }
        }
    }

    public void ChangeNext()
    {
        skins[selectedScene].SetActive(false);
        selectedScene++;
        if (selectedScene == skins.Length) selectedScene = 0;

        skins[selectedScene].SetActive(true);

        if(scenes[selectedScene].isUnlocked)
            PlayerPrefs.SetInt("SelectedScene",selectedScene);

        SoundsManager.Instance.PlaySound(selectAudio);

        UpdateUI();
    }
    public void ChangePrevious()
    {
        skins[selectedScene].SetActive(false);
        selectedScene--;
        if (selectedScene == -1) selectedScene = skins.Length - 1;

        skins[selectedScene].SetActive(true);

        if(scenes[selectedScene].isUnlocked)
            PlayerPrefs.SetInt("SelectedScene",selectedScene);

        SoundsManager.Instance.PlaySound(selectAudio);

        UpdateUI();
    }
    public void EnterShopMode()
    {
        skins[0].SetActive(false); //Set character to the current locked character (if it is)
        skins[selectedScene].SetActive(true);

        menuPanel.SetActive(false);
        shopPanel.SetActive(true);
    }
    public void ExitShopMode()
    {
        if(!scenes[selectedScene].isUnlocked)
        {
            skins[0].SetActive(true); //Set character to default if the current is not unlocked
            skins[selectedScene].SetActive(false);
        }

        menuPanel.SetActive(true);
        shopPanel.SetActive(false);
    }
    public void UpdateUI()
    {
        if (scenes[selectedScene].isUnlocked == true)
        {
            unlockButton.gameObject.SetActive(false);
            unlockButton.interactable = false;
        }
            
        else
        {
            unlockButton.GetComponentInChildren<TextMeshProUGUI>().text = "" + scenes[selectedScene].price;
            if (!CurrencyManager.Instance.CanSubtractCurrency(scenes[selectedScene].price))
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
        int price = scenes[selectedScene].price;
        CurrencyManager.Instance.SubtractCurrency(price);

        PlayerPrefs.SetInt(scenes[selectedScene].name, 1);
        PlayerPrefs.SetInt("SelectedScene", selectedScene);

        scenes[selectedScene].isUnlocked = true;
        UpdateUI();
    }
}
