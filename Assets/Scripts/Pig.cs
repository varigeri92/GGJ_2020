using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : MonoBehaviour
{
    [SerializeField] ParticleSystem pS;
    [SerializeField] AudioSource aS;

    [SerializeField] CapsuleCollider collider;
    [SerializeField]  Rigidbody rb;

    bool placed = false;
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
                pS.transform.parent = null;
                aS.transform.parent = null;
                aS.Play();
                pS.Play();

                gameObject.SetActive(false);
            }
            
        }
    }

    private void Update()
    {
        if (!placed)
        {
            transform.position = GameSystemManager.Instance.worldMousePosition;

            if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                placed = true;
                collider.enabled = true;
                GameSystemManager.Instance.pigs.Add(this);
            }
            if (Input.GetMouseButtonDown(1) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetKinematic(bool val)
    {
        rb.isKinematic = val;
    }

}
