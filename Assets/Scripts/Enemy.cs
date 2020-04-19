using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, Damagable
{
    public float maxHealth;
    float health;

    Rigidbody2D rb;

    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void DealDamage(float amount, Vector3 direction)
    {
        rb.AddForce(direction.normalized*amount,ForceMode2D.Impulse);
        GameManager.MakePopup(string.Format($"{amount:0.00}"),transform.position);
    }

    public virtual void Start()
    {
        health = maxHealth;
    }
}
