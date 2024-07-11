using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombControlelr : MonoBehaviour
{
    private Collider cl;
    public ParticleSystem explosion;
    public int dame = 100;
    public float waitTime;
    public float radius;
    public LayerMask playerLayer;
    public Image imageCircle;


    private void Start()
    {
        cl = GetComponent<Collider>();

        StartCoroutine(WaitForExplode());
        StartCoroutine(FadeImage());

        Destroy(transform.parent.gameObject, 5f);
    }

    IEnumerator WaitForExplode()
    {
        yield return new WaitForSeconds(waitTime);

        HandleExplode();
    }
    public void HandleExplode()
    {
        explosion.Play();
        cl.enabled = false;

        if (DetectPlayer(out List<GameObject> objectList) != null)
        {
            imageCircle.gameObject.SetActive(false);

            foreach (GameObject go in objectList)
            {
                if (go.CompareTag("Player"))
                {
                    go.GetComponent<PlayerState>().GetDameExplode(dame);
                } 
                else if (go.CompareTag("Obstacte"))
                {
                    go.AddComponent<Rigidbody>();
                    go.GetComponent<Rigidbody>().AddExplosionForce(500f, transform.position,radius, 1f) ;
                    Destroy(go, 1f);
                }
                else if (go.CompareTag("Bomb"))
                {
                    go.GetComponent<BombControlelr>().HandleExplode();
                }
            }

            Destroy(gameObject);
        }
    }

    private List<GameObject> DetectPlayer(out List<GameObject> objectList)
    {
        objectList = new List<GameObject>();
        float angleStep = 360f / 360f;

        for (int i = 0; i < 360; i++)
        {
            float angle = i * angleStep;

            Vector3 direction = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, radius, playerLayer))
            {
                if ((hit.collider.CompareTag("Player") || hit.collider.CompareTag("Obstacte") || hit.collider.CompareTag("Bomb")) && !objectList.Contains(hit.collider.gameObject))
                {
                    objectList.Add(hit.collider.gameObject);
                }
            }
        }

        return objectList;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cl.isTrigger = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator FadeImage()
    {
        while (true)
        {
            yield return Fade(0, 1, 0.5f); 
            yield return Fade(1, 0, 0.5f); 
        }
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            SetImageAlpha(alpha);
            yield return null;
        }
        SetImageAlpha(endAlpha);
    }

    private void SetImageAlpha(float alpha)
    {
        if (imageCircle != null)
        {
            Color color = imageCircle.color;
            color.a = alpha;
            imageCircle.color = color;
        }
    }
}
