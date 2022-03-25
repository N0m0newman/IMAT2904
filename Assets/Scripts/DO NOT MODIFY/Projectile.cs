using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public GameObject projectile;
    public ParticleSystem explosionParticle;
    public AudioSource explosionSound;
    float t = 0.02f;
    Rigidbody rb;
    bool destroy = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!rb.isKinematic)
        {
            t -= Time.deltaTime;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(t < 0 && destroy == false)
        {
            explosionParticle.Play();
            rb.isKinematic = true;
            projectile.SetActive(false);
            StartCoroutine(DestoryThis());
            destroy = true;
            explosionSound.Play();
        }

    }


    IEnumerator DestoryThis()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
