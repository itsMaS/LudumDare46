using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : Tool
{
    [SerializeField]
    AnimationCurve strengthProgression;

    [SerializeField]
    ContactFilter2D filter;
    [SerializeField]
    ParticleSystem ps;
    public float wateringStrength = 1;

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
            if (waterAmount > 0 || true)
            {
                waterAmount -= Time.deltaTime;
                List<Collider2D> hits = new List<Collider2D>();
                areaOfEffect.OverlapCollider(filter, hits);
                foreach (Collider2D item in hits)
                {
                    Waterable target = item.GetComponent<Waterable>();
                    if (target != null)
                    {
                        target.Water(Time.deltaTime*wateringStrength);
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

    public override void OnNextLevel(int level)
    {
        base.OnNextLevel(level);
        wateringStrength = strengthProgression.Evaluate(level);
    }

    public override void OnDeselect()
    {
        base.OnDeselect();
        ps.enableEmission = false;
        if (watering) AudioManager.FadeOut(watering, 1);
    }
    public override void OnSelect()
    {
        base.OnSelect();
    }

    AudioSource watering;
    public override void OnLeftClickBegin()
    {
        base.OnLeftClickBegin();
        watering = AudioManager.Play("Robot/watering",0.5f,true);
        
    }
    public override void OnLeftClickEnd()
    {
        if (watering) AudioManager.FadeOut(watering, 0.3f);
        base.OnLeftClickEnd();
    }
}
