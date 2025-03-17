using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    public List<Transform> players; 
    public float smoothTime = 0.3f; 
    public float singlePlayerZoom = 7f; 
    public float minZoom = 5f; 
    public float maxZoom = 10f; 
    public float zoomLimiter = 5f; 

    private Vector3 velocity;
    private Camera cam;
    private Collider2D[] colliders;

    void Start()
    {
        cam = GetComponent<Camera>();
        colliders = FindObjectsOfType<Collider2D>(); 
    }

    void LateUpdate()
    {
        if (players.Count == 0)
            return;

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = new Vector3(centerPoint.x, centerPoint.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom;

        if (players.Count == 1)
        {
            newZoom = singlePlayerZoom;
        }
        else
        {
            float distance = GetGreatestDistance();
            newZoom = Mathf.Lerp(maxZoom, minZoom, Mathf.InverseLerp(0, zoomLimiter, distance));
        }

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
        {
            return players[0].position;
        }

        Bounds bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 1; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }
        return bounds.center;
    }

    float GetGreatestDistance()
    {
        if (players.Count == 1)
            return 0f;

        Bounds bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 1; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }
        return bounds.size.x;
    }
}
