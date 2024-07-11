using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private AudioClip deathAudio;
    public float offsetY = 1f;
    private void LateUpdate()
    {
        Vector3 newPos = transform.position;
        newPos.x = cameraTransform.position.x;
        newPos.y = cameraTransform.position.y + offsetY;
        transform.position = newPos;
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Player player = hitInfo.GetComponent<Player>();
        if(player != null)
        {
            SoundsManager.Instance.PlaySound(deathAudio);
            player.Die();//Set non-active, not destroyed
        } 
    }
}
