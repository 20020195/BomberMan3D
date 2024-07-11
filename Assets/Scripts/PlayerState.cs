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
            GetComponent<PlayerMove>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Camera.main.GetComponent<CameraController>().SetCamWhenDie();
            //Destroy(gameObject, 5f);
        }
    }
    public void GetDameExplode(int dame)
    {
        hp -= dame;

        if (hp <= 0)
        {
            animator.SetTrigger("dead");
            StartCoroutine(WaitForRevival());
        }
    }

    IEnumerator WaitForRevival()
    {
        Debug.Log(Time.time);
        yield return new WaitForSeconds(5f);
        
        animator.SetTrigger("normal");
        transform.position = new Vector3(0, transform.position.y, 0);
        hp = 100;

        GetComponent<PlayerMove>().enabled = true;
        GetComponent<PlayerController>().enabled = true;
        GetComponent<Collider>().enabled = true;
        Camera.main.GetComponent<CameraController>().SetCamWhenRevival();
        Debug.Log(Time.time);
    }
}
