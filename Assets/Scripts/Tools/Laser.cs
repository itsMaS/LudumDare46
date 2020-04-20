using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Tool
{
    [SerializeField]
    ContactFilter2D filter;
    [SerializeField]
    Collider2D col;
    [SerializeField]
    GameObject beam;
    [SerializeField]
    GameObject dischargeParticle;
    [SerializeField]
    float chargeTime = 5;

    public float cooldown;
    public float chargeSpeed;
    public float damage;
    public float explosionForce;
    public AnimationCurve damageFalloff;
    public AnimationCurve knockBackFalloff;

    Animator an;
    float charge = 0;

    bool cooled = true;
    bool charging = false;

    bool chargeStart;

    AudioSource chargeSound;
    private void Awake()
    {
        an = GetComponent<Animator>();
    }

    void Discharge()
    {
        AudioManager.Play("Robot/laser_shoot");
        CameraController.ShakeCamera(1.5f,0.5f);

        StartCoroutine(Cooldown());
        List<Collider2D> hits = new List<Collider2D>();
        col.OverlapCollider(filter, hits);
        foreach (Collider2D item in hits)
        {
            Damagable target = item.GetComponent<Damagable>();
            if(target != null && !item.CompareTag("Player"))
            {
                target.DealDamage(damage*damageFalloff.Evaluate(Vector3.Distance(transform.position,item.transform.position)), item.transform.position - transform.position);
                Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();
                if (rb)
                {
                    Debug.DrawLine(item.transform.position, transform.position, Color.red, 5);
                }
            }
        }
        if(dischargeParticle)
        {
            Instantiate(dischargeParticle,transform.position,Quaternion.identity);
        }

        hits = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, 10, filter, hits);
        foreach (Collider2D item in hits)
        {
            Rigidbody2D rb;
            if((rb = item.attachedRigidbody))
            {
                rb.AddForce(knockBackFalloff.Evaluate(Vector3.Distance(item.transform.position, transform.position)) 
                    * (item.transform.position - transform.position).normalized * explosionForce, ForceMode2D.Impulse);
                //rb.AddForceAtPosition((item.transform.position - transform.position).normalized * explosionForce, transform.position,ForceMode2D.Impulse);
                Debug.DrawLine(item.transform.position, transform.position,Color.red,5);
                Debug.DrawLine(item.transform.position, knockBackFalloff.Evaluate(Vector3.Distance(item.transform.position, transform.position))
                    * (item.transform.position - transform.position).normalized * explosionForce,Color.blue,5f);
            }
        }
        //StartCoroutine(StopTime());
    }

    IEnumerator StopTime()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 1;
    }

    IEnumerator Cooldown()
    {
        cooled = false;
        yield return new WaitForSeconds(cooldown);
        cooled = true;
    }

    public override void OnSelect()
    {
        base.OnSelect();
        beam.SetActive(true);
    }
    public override void OnDeselect()
    {
        base.OnDeselect();
        beam.SetActive(false);
    }
    public override void Using()
    {
        base.Using();
        if (leftClicked && cooled)
        {
            base.OnLeftClick();
            if(!chargeStart)
            {
                chargeStart = true;
                chargeSound = AudioManager.Play("Robot/laser_charge");
            }

            charge += chargeSpeed * Time.deltaTime;
            CameraController.ShakeCamera(0.3f * Mathf.InverseLerp(0, chargeTime, charge));
            if (charge >= chargeTime)
            {
                an.SetTrigger("Discharge");
                charge = 0;
                Discharge();
            }
        }
        else
        {
            chargeStart = false;
            charge = Mathf.Max(charge - Time.deltaTime * 20f, 0);
            if (chargeSound)
            {
                AudioManager.FadeOut(chargeSound, 0.1f);
            }
        }
        an.SetFloat("Progress", Mathf.InverseLerp(0, chargeTime, charge));
    }
}
