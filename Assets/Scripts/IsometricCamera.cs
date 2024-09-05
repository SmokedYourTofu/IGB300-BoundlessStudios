using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    public GameObject player;      // Reference to the player GameObject
    public float height = 10.0f;   // Height of the camera from the player
    public float zDisp = 10.0f;    // Distance of the camera behind the player
    public float cameraSpeed = 1.0f; // Speed at which the camera follows the player
    public float cameraCutoff = -2f; // Determiens when the camera should stop moving at the bottom of the screen

    private Vector3 newCamPos;     // New camera position

    // Start is called before the first frame update
    void Start()
    {
        // Set initial camera position
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + height, player.transform.position.z - zDisp);
    }

    // Update is called once per frame
    void Update()
    {
        // If the player GameObject is assigned and active
        if (player)
        {
            CameraMovement();
        }
    }

    // Camera follows the player with smooth panning
    void CameraMovement()
    {

        // Set the new camera position based on the player's position
        if (player.transform.position.z > cameraCutoff)
        {
            newCamPos = new Vector3(player.transform.position.x, player.transform.position.y + height, player.transform.position.z - zDisp);
        }
        else
        {
            newCamPos = new Vector3(player.transform.position.x, player.transform.position.y + height, cameraCutoff - zDisp);
        }

        // Smoothly interpolate the camera's position towards the new position
        transform.position = Vector3.Lerp(transform.position, newCamPos, cameraSpeed * Time.deltaTime);
    }
}
