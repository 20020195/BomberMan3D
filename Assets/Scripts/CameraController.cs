using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController: MonoBehaviour
{
    public CinemachineBrain cinemachineBrain;
    public GameObject camWhenDead;
    public float sensitivity = 5f;
    public GameObject camFollowPlane;
    private float mouseX, mouseY;
    public GameObject player;

    private void Start()
    {
        cinemachineBrain = GetComponent<CinemachineBrain>();

        Application.targetFrameRate = 120;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (camFollowPlane.activeSelf)
            {
                camFollowPlane.SetActive(false);
                cinemachineBrain.enabled = false;

                transform.SetParent(player.transform);
                transform.localPosition = new Vector3(0, 1.15f, 0.5f);
                transform.rotation = Quaternion.Euler(0, 0, 0);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                camFollowPlane.SetActive(true);
                cinemachineBrain.enabled=true;
                transform.SetParent(null);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        // write script//

        if (!camFollowPlane.activeSelf && !camWhenDead.activeSelf)
        {
            mouseX += Input.GetAxis("Mouse X") * sensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
            mouseY = Mathf.Clamp(mouseY, -40f, 80f);
            player.transform.eulerAngles = new Vector3(0, mouseX, 0);
            transform.eulerAngles = new Vector3(mouseY, mouseX, 0f);
            
        }
            
    }

    public void SetCamWhenDie()
    {
        camFollowPlane.SetActive(false);

        cinemachineBrain.enabled = true;
        camWhenDead.SetActive(true);
    }

    public void SetCamWhenRevival()
    {
        camWhenDead.SetActive(false);
        camFollowPlane.SetActive(true);
    }
}
