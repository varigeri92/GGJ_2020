using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] WallProperties properties;
    Rigidbody rb;

    BoxCollider collider;

    public bool placed = false;

    [SerializeField] List<GameObject> destructionlevels = new List<GameObject>();
    int destuctionLevel = 0;

    int durability;
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        durability = properties.durability;
        foreach (GameObject go in destructionlevels)
        {
            go.SetActive(false);
        }
        destructionlevels[0].SetActive(true);
    }

    private void Update()
    {
        if (!placed)
        {
            transform.parent.position = GameSystemManager.Instance.worldMousePosition;

            if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                placed = true;
                GameSystemManager.Instance.AddWall(this);
                collider.enabled = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("projectile"))
        {
            float impactForce = collision.collider.GetComponent<Projectile>().GetImpactForce();
            if (impactForce > 1)
            {
            }
            Debug.Log(impactForce);
            durability -= 1;

            if (durability <= 0)
            {
                durability = 0;
                // gameObject.SetActive(false);
                collider.enabled = false;
                rb.isKinematic = true;
                foreach (GameObject go in destructionlevels)
                {
                    go.SetActive(false);
                }
                destructionlevels[2].SetActive(true);
                StartCoroutine(DestroyTimer());
            }else if (durability < 2)
            {
                foreach (GameObject go in destructionlevels)
                {
                    go.SetActive(false);
                }
                destructionlevels[1].SetActive(true);
            }
        }
    }

    public float GetImpactForce()
    {
        return rb.velocity.magnitude * rb.mass;
    }

    public float GetMagnitude()
    {
        return rb.velocity.magnitude;
    }

    public void TurnKinematic(bool turn)
    {
        rb.isKinematic = turn;
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }
}
