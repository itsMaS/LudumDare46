using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField]
    SeedPlanter seedPlanter;

    [SerializeField]
    float pickupRange = 0.1f;

    PointEffector2D pe;
    private void Awake()
    {
        pe = GetComponent<PointEffector2D>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("PICK");
        if(Vector3.Distance(other.transform.position, transform.position) < pickupRange)
        {
            if(!seedPlanter.Full)
            {
                Destroy(other.gameObject);
                seedPlanter.AddSeed();
                AudioManager.Play("pickup", 1f);
                GameManager.MakePopup("+1 Seed", transform.position);
            }
        }
    }
    private void Update()
    {
        if(seedPlanter.Full)
        {
            pe.enabled = false;
        }
        else
        {
            pe.enabled = true;
        }
    }
}
