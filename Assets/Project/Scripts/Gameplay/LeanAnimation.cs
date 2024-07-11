using UnityEngine;
using System.Collections;

public class LeanAnimation : MonoBehaviour
{
    public Transform box;
    [SerializeField] private float speed;
    [SerializeField] private float delayValue;
    
    [Range(-1.0f, 1.0f)]
    [SerializeField] private float screenNormalize;
    private void OnEnable()
    {
        box.localPosition = new Vector2(0, Screen.height * screenNormalize);
        box.LeanMoveLocalY(0,speed).setEaseOutExpo().delay = delayValue;
    }
    public void ClosePanel()
    {
        box.LeanMoveLocalY(Screen.height * screenNormalize,speed).setEaseInExpo();
    }
    // IEnumerator WaitForClose()
    // {
    //     yield return new WaitForSeconds(0.5f);
        
    // }
}
