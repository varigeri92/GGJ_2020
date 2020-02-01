using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] WallProperties properties;
    Rigidbody rb;
    

    public bool placed = false;

    int durability;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        durability = properties.durability;
    }

    private void Update()
    {
        if (!placed)
        {
            transform.position = GameSystemManager.Instance.worldMousePosition;

            if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                placed = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("projectile"))
        {
            durability -= 1;
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
