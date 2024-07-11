using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] Player player;
    [SerializeField] private float highestX ;
    [SerializeField] private float lowestX;

    [Header("Zoom")]
    [SerializeField] private Camera myCamera;
    [SerializeField] private float zoomMagnitude = 2.0f;
    [SerializeField] private float zoomTime = 1.0f;
    [SerializeField] private Vector3 zoomPoint;
    private Vector3 original;
    private float originalSize;

    public void Start()
    {
        original = myCamera.transform.position;
        originalSize = myCamera.orthographicSize;
    }

    public void FixedUpdate()
    {
        float xDif = 0;

        if(player.transform.position.x > highestX)
        {
            xDif = player.transform.position.x - highestX;
            highestX = player.transform.position.x;
            lowestX+=xDif;
        }
        if(player.transform.position.x < lowestX)
        {
            xDif = player.transform.position.x - lowestX;
            lowestX = player.transform.position.x;
            highestX+=xDif;
        }

        Vector3 tempPosition = transform.position;
        tempPosition.x +=xDif;
        transform.position = tempPosition;
    }

    public void ZoomInto()
    {
        StartCoroutine(ZoomIn());
    }
    public void ZoomOutto()
    {
        StartCoroutine(ZoomOut());
    }
    IEnumerator ZoomIn()
    {
        // Use LeanTween to smoothly zoom the camera towards the target point
        LeanTween.move(myCamera.gameObject, zoomPoint, zoomTime).setEaseInOutQuad();
        LeanTween.value(myCamera.orthographicSize, originalSize / zoomMagnitude, zoomTime)
            .setOnUpdate((float value) => { myCamera.orthographicSize = value; })
            .setEaseInOutQuad();

        // Wait for the zoom to complete
        yield return new WaitForSeconds(zoomTime);
    }
    IEnumerator ZoomOut()
    {
        // Use LeanTween to smoothly zoom the camera back to the original position
        LeanTween.move(myCamera.gameObject, original, zoomTime).setEaseInOutQuad();
        LeanTween.value(myCamera.orthographicSize, originalSize, zoomTime)
            .setOnUpdate((float value) => { myCamera.orthographicSize = value; })
            .setEaseInOutQuad();

        // Wait for the zoom to complete
        yield return new WaitForSeconds(zoomTime);
    }
}
