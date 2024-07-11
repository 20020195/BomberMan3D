using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidPlayerController : NetworkBehaviour
{
    public GameObject canvasCtrl;
    public VariableJoystick joystick;
    public Button placeBombBtn;
    public GameObject bombPrefab;

    private Vector3 moveInput;
    private Rigidbody rb;

    public float moveSpeed = 8f;
    public float turnSpeed = 180f;
    public Animator anim;
    public float Speed;
    [Range(0, 1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;

    private bool canPlaceBomb = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        canvasCtrl = GameObject.Find("AndroidCanvas");
        foreach (Transform child in canvasCtrl.transform)
        {
            child.gameObject.SetActive(true);
            if (child.GetComponent<VariableJoystick>()  != null)
            {
                joystick = child.GetComponent<VariableJoystick>();
            } else if (child.GetComponent<Button>() != null)
            {
                placeBombBtn = child.GetComponent<Button>();
            }
        }

        placeBombBtn.onClick.AddListener(PlaceBomb);
    }

    private void Update()
    {
        moveInput = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        Speed = moveInput.magnitude;

        if (Speed > 0.1f)
        {
            anim.SetFloat("Blend", Speed, StartAnimTime, Time.deltaTime);
        }
        else if (Speed < 0.1f)
        {
            anim.SetFloat("Blend", Speed, StopAnimTime, Time.deltaTime);
        }

        if (moveInput != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 180f);
        }

        rb.velocity = moveInput * moveSpeed;
    }

    private void PlaceBomb()
    {
        if (canPlaceBomb)
        CmdBomb();
    }

    [Command]
    public void CmdBomb()
    {
        SpawnBomb();
    }

    [ClientRpc]
    public void SpawnBomb()
    {
        canPlaceBomb = false;

        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bomb.transform.position = new Vector3(bomb.transform.position.x, 0.5f, bomb.transform.position.z);
        StartCoroutine(PlaceBombCoroutine());
    }

    IEnumerator PlaceBombCoroutine()
    {
        yield return new WaitForSeconds(1);
        canPlaceBomb = true;
    }
}
