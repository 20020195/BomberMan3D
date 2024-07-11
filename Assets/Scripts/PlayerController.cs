using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public GameObject bombPrefab;

    private bool canPlaceBomb;
    [SerializeField]
    private float timeDelay;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canPlaceBomb == false)
        {
            CmdBomb();
        }
    }

    [Command]
    public void CmdBomb()
    {
        SpawnBomb();
    }

    [ClientRpc]
    public void SpawnBomb()
    {
        canPlaceBomb = true;

        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        bomb.transform.position = new Vector3(bomb.transform.position.x, 0.5f, bomb.transform.position.z);
        StartCoroutine(PlaceBombCoroutine());
    }

    IEnumerator PlaceBombCoroutine()
    {
        yield return new WaitForSeconds(timeDelay);
        canPlaceBomb = false;
    }
}
