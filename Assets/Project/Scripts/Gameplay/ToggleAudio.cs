using UnityEngine;
using UnityEngine.UI;
public class ToggleAudio : MonoBehaviour
{
    [SerializeField] private bool toggleEffects;
    [SerializeField] private Image toggleOn;
    [SerializeField] private AudioClip clip;
    public void Start()
    {
        if(SoundsManager.toggleValue == -1) toggleOn.enabled = false; //Adjust the icon after each scene reload
    }
    public void ToggleEffect()
    {
        if(toggleEffects)
        {
            toggleOn.enabled = !toggleOn.enabled;
            SoundsManager.Instance.ToggleEffects();
        }
        if(toggleEffects == true) SoundsManager.Instance.PlaySound(clip);

    }
}
