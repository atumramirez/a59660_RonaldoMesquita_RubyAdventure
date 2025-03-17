using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public Transform teleportLocation;

    private bool isPlayerInside = false;
    private Transform playerTransform;

    public OpacityController fadeController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            playerTransform = null;
        }
    }

    private void Update()
    {
        if (isPlayerInside && teleportLocation != null)
        {
            playerTransform.position = teleportLocation.position;
            fadeController.FadeInAndOut();
        }
    }
}