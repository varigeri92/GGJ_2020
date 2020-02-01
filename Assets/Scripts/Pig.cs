using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("projectile"))
        {
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
