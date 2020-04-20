using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bees : MonoBehaviour, Damagable
{
    [SerializeField]
    ContactFilter2D filter;
    [SerializeField]
    Collider2D col;


    [SerializeField]
    float speed;

    Animator an;
    Rigidbody2D rb;
    Transform target;

    IEnumerator SearchForPlants()
    {
        while(true)
        {
            float minDistance = float.MaxValue;
            int count = 0;
            foreach (Plant item in FindObjectsOfType<Plant>())
            {
                count++;
                float distance = Vector3.Distance(item.transform.position,transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    target = item.transform;
                }
            }
            if(count == 0)
            {
                target = transform;
            }
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(SearchForPlants());
    }

    private void Update()
    {
        List<Collider2D> hits = new List<Collider2D>();
        col.OverlapCollider(filter, hits);
        foreach (Collider2D item in hits)
        {
            Swarmable target = item.GetComponent<Swarmable>();
            if (target != null)
            {
                target.Swarm(transform);
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = (target.position - transform.position);
        if(moveDirection.magnitude < 0.1f)
        {
            moveDirection = new Vector3();
        }
        else
        {
            moveDirection.Normalize();
        }
        rb.AddForce(moveDirection*speed);
    }

    public void DealDamage(float amount, Vector3 direction)
    {
        an.SetTrigger("Die");
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
