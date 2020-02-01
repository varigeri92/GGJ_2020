using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] WallProperties properties;
    Rigidbody rb;

    BoxCollider collider;

    public bool placed = false;

    int durability;
    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        durability = properties.durability;
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
            if (impactForce > 10 && impactForce < 20)
            {
                durability -= 1;
            }else if (impactForce >= 20)
            {
                durability -= 2;
            }

            if (durability <= 0)
            {
                durability = 0;
                gameObject.SetActive(false);
            }

            
        }
    }

    public void TurnKinematic(bool turn)
    {
        rb.isKinematic = turn;
    }
}
