using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant : MonoBehaviour, Waterable, Burnable, Damagable, Swarmable
{
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

    [SerializeField]
    Color SeedGrowthColor;
    [SerializeField]
    Color WaterColor;

    private void Awake()
    {
        defaultPos = IK.transform.position;
        normalPos = defaultPos;
    }

    public void Burn(float amount)
    {
    }

    public void DealDamage(float amount, Vector3 direction)
    {
        IK.position += direction;
        AudioManager.PlayWithPitchDeviation("swipe", 1f, false, 0.3f);
    }

    public void Water(float amount)
    {
        water = Mathf.Min(water + amount, waterCapacity);
    }

    bool danger = false;
    private void Update()
    {
        IK.position = Vector3.Lerp(IK.position,defaultPos,0.1f);
        water = Mathf.Max(water - Time.deltaTime*dryingSpeed, 0);

        float waterPercentage = Mathf.InverseLerp(0, waterCapacity, water);
        waterIndication.fillAmount = waterPercentage;
        defaultPos = Vector3.Lerp(transform.position + witherPos, normalPos, waterPercentage);

        if(water <= 0)
        {
            if(!danger)
            {
                danger = true;
                AudioManager.Play("danger", 0.2f);
            }
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
            danger = false;
            witherIndication.gameObject.SetActive(false);
            witherProgress = Mathf.Max(witherProgress-Time.deltaTime*0.1f,0);
            SeedGrowth();
        }

        if (PlayerController.current == PlayerController.Tools.SeedPlanter || PlayerController.current == PlayerController.Tools.WateringCan)
        {
            waterIndication.enabled = true;
            switch (PlayerController.current)
            {
                case PlayerController.Tools.WateringCan:
                    waterIndication.color = WaterColor;
                    waterIndication.fillAmount = waterPercentage;
                    break;
                case PlayerController.Tools.SeedPlanter:
                    waterIndication.color = SeedGrowthColor;
                    waterIndication.fillAmount = Mathf.InverseLerp(0, growthThreshold, growth);
                    break;
            }
        }
        else
        {
            waterIndication.enabled = false;
        }
    }

    void Wither()
    {
        GameManager.Wither();
        flower.color = witheredColor;
        witherIndication.gameObject.SetActive(false);
        Destroy(this);
    }

    public void Swarm(Transform swarmer, float strength)
    {
        water = Mathf.Max(water - Time.deltaTime*strength, 0);
    }

    GameObject lastSeed;
    [SerializeField]
    GameObject SeedPrefab;
    [SerializeField]
    float growthThreshold = 10;
    float growth = 0;
    void SeedGrowth()
    {
        growth += Time.deltaTime;
        if(growth >= growthThreshold && !lastSeed)
        {
            growth = 0;
            lastSeed = Instantiate(SeedPrefab,transform.position,Quaternion.identity);
        }
    }
}
