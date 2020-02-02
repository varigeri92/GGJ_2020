using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField]
    ParticleSystem pS;
    [SerializeField]
    AudioSource aS;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("projectile"))
        {
            pS.transform.parent = null;
            aS.transform.parent = null;
            aS.Play();
            pS.Play();

            gameObject.SetActive(false);

        }
        if (collision.collider.gameObject.CompareTag("Wall"))
        {
            if (collision.collider.GetComponent<Wall>().GetImpactForce() > 5f)
            {
                gameObject.SetActive(false);
            }
            
        }
    }
}
