using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScript : MonoBehaviour
{
    bool baseHit = false;
    private ParticleSystem destroyPart;
    private GameObject modelGO;
    private Rigidbody rBody;
    private BoxCollider bCollider;
    private void Start()
    {
        destroyPart = transform.Find("DestroyParticle").GetComponent<ParticleSystem>();
        modelGO = transform.Find("Model").gameObject;
        rBody = GetComponent<Rigidbody>();
        bCollider = GetComponent<BoxCollider>();
        rBody.isKinematic = true;
        bCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Projectile" && baseHit == false)
        {
            bCollider.isTrigger = false;
            print(this.transform.parent.gameObject.name + " base has been hit!");
            baseHit = true;
            StartCoroutine(BaseDestroyed());
            modelGO.SetActive(false);
        }
    }

    IEnumerator BaseDestroyed()
    {
        destroyPart.Play();
        yield return new WaitForSeconds(0.75f);
        Destroy(this.gameObject);
    }
}
