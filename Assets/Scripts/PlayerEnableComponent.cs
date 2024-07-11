using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;
using Cinemachine;

public class PlayerEnableComponent : NetworkBehaviour
{
    public Transform target;

    private void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerState>().enabled = true;
#if UNITY_ANDROID
            GetComponent<AndroidPlayerController>().enabled = true;
#else
            GetComponent<PlayerMove>().enabled = true;
#endif


            GameObject cfp = Camera.main.GetComponent<CameraController>().camWhenDead;
            CinemachineVirtualCamera cvc = cfp.GetComponent<CinemachineVirtualCamera>();
            cvc.Follow = gameObject.transform;
            cvc.LookAt = gameObject.transform;
            cfp.SetActive(false);
            Camera.main.GetComponent<CameraController>().camFollowPlane.SetActive(true);
            Camera.main.GetComponent<CameraController>().player = gameObject;
        }
    }
}
