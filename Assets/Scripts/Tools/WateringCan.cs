using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField]
    ContactFilter2D filter;
    [SerializeField]
    ParticleSystem ps;

    public float waterAmount;
    public float maxWater;

    Collider2D areaOfEffect;
    bool spraying = false;
    private void Awake()
    {
        areaOfEffect = GetComponent<Collider2D>();
        ps.enableEmission = false;
    }

    public override void Using()
    {
        base.Using();
        if(leftClicked)
        {
            if (waterAmount > 0)
            {
                waterAmount -= Time.deltaTime;
                List<Collider2D> hits = new List<Collider2D>();
                areaOfEffect.OverlapCollider(filter, hits);
                foreach (Collider2D item in hits)
                {
                    Waterable target = item.GetComponent<Waterable>();
                    if (target != null)
                    {
                        target.Water(Time.deltaTime);
                    }
                }
                ps.enableEmission = true;
            }
            else
            {
                waterAmount = 0;
                ps.enableEmission = false;
            }
        }
        else
        {
            ps.enableEmission = false;
        }
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        ps.enableEmission = false;
        Plant.ShowWater = false;
    }
    public override void OnSelect()
    {
        base.OnSelect();
        Plant.ShowWater = true;
    }
}
