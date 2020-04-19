using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour, Waterable, Burnable, Damagable
{
    public static bool ShowWater = false;

    [SerializeField]
    float waterCapacity;
    [SerializeField]
    float dryingSpeed = 0.5f;
    [SerializeField]
    float water;
    [SerializeField]
    Image waterIndication;
    [SerializeField]
    Image witherIndication;
    [SerializeField]
    SpriteRenderer flower;
    [SerializeField]
    Color witheredColor;

    [SerializeField]
    Transform IK;

    Vector3 defaultPos;

    [SerializeField]
    float witherThreshold = 2;
    float witherProgress = 0;

    [SerializeField]
    Vector3 witherPos;
    Vector3 normalPos;

    private void Awake()
    {
        defaultPos = IK.transform.position;
        normalPos = defaultPos;
    }
    private void Start()
    {
        water = waterCapacity;
    }

    public void Burn(float amount)
    {
        Debug.Log("PLANT BURNED");
    }

    public void DealDamage(float amount, Vector3 direction)
    {
        Debug.Log("DAMAGE");
        IK.position += direction;
    }

    public void Water(float amount)
    {
        water = Mathf.Min(water + amount, waterCapacity);
    }

    private void Update()
    {
        if(ShowWater)
        {
            waterIndication.enabled = true;
        }
        else
        {
            waterIndication.enabled = false;
        }

        IK.position = Vector3.Lerp(IK.position,defaultPos,0.1f);
        water = Mathf.Max(water - Time.deltaTime*dryingSpeed, 0);

        float waterPercentage = Mathf.InverseLerp(0, waterCapacity, water);
        waterIndication.fillAmount = waterPercentage;
        defaultPos = Vector3.Lerp(transform.position + witherPos, normalPos, waterPercentage);

        if(water <= 0)
        {
            witherIndication.gameObject.SetActive(true);
            witherIndication.fillAmount = Mathf.InverseLerp(0,witherThreshold,witherProgress);
            witherProgress += Time.deltaTime;
            if(witherProgress >= witherThreshold)
            {
                Wither();
            }
        }
        else
        {
            witherIndication.gameObject.SetActive(false);
            witherProgress = Mathf.Max(witherProgress-Time.deltaTime*0.1f,0);
        }
    }

    void Wither()
    {
        Debug.Log("Plant died!");
        flower.color = witheredColor;
        witherIndication.gameObject.SetActive(false);
        Destroy(this);
    }
}
