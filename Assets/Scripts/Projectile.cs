using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        GameSystemManager.Instance.StartTimer();
        StartCoroutine(DestroyProjectile());
    }

    public float GetImpactForce()
    {
        return rb.mass * rb.velocity.magnitude;
    }

    IEnumerator DestroyProjectile()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
