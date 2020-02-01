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
    }

    public float GetImpactForce()
    {
        return rb.mass * rb.velocity.magnitude;
    }
}
