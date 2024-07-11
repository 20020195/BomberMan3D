using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerState : NetworkBehaviour
{
    public int hp = 100;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hp <= 0 && isLocalPlayer)
        {
#if UNITY_ANDROID
            GetComponent<AndroidPlayerController>().enabled = false;
#else
            GetComponent<PlayerMove>().enabled = false;
#endif
            GetComponent<PlayerController>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Camera.main.GetComponent<CameraController>().SetCamWhenDie();
        }
    }
    public void GetDameExplode(int dame)
    {
        hp -= dame;

        if (hp <= 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            animator.SetTrigger("dead");
            StartCoroutine(WaitForRevival());
        }
    }

    IEnumerator WaitForRevival()
    {
        Debug.Log(Time.time);
        yield return new WaitForSeconds(10f);
        
        animator.SetTrigger("normal");
        transform.position = new Vector3(0, transform.position.y, 0);
        hp = 100;

#if UNITY_ANDROID
        GetComponent<AndroidPlayerController>().enabled = true;
#else
            GetComponent<PlayerMove>().enabled = true;
#endif
        GetComponent<PlayerController>().enabled = true;
        GetComponent<Collider>().enabled = true;
        Camera.main.GetComponent<CameraController>().SetCamWhenRevival();
        Debug.Log(Time.time);
    }
}
