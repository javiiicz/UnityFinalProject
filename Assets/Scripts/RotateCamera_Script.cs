using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script rotates the camera left and right when the holds the left mouse button and drags 
public class RotateCamera_Script : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float mouseSensitivity = 10f;

    private Vector3 difference;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        difference = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + difference;

        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity;

        transform.RotateAround(player.transform.position, Vector3.up, moveX);

        difference = transform.position - player.transform.position;

    }
}
