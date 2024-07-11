using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject objectToTeleport;
    [SerializeField] private GameObject player;
    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player)
        {
            isTeleporting = true;
        }
    }

    private void LateUpdate()
    {
        if (isTeleporting)
        {
            objectToTeleport.transform.position = player.transform.position;
            isTeleporting = false;
        }
    }
}
