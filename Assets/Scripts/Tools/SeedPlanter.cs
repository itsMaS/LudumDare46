using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedPlanter : Tool
{
    [SerializeField]
    GameObject plantPrefab;

    [SerializeField]
    GameObject indicator;
    [SerializeField]
    ContactFilter2D filter;
    Collider2D col;
    SpriteRenderer sr;

    [SerializeField]
    Color availableColor;
    [SerializeField]
    Color unavailableColor;
    [SerializeField]
    Color clickedColor;
    [SerializeField]
    TextMeshProUGUI seedInfo;

    [SerializeField]
    int seedCount = 1;
    public int maxSeeds = 5;
    bool placable = false;

    public bool Full { get { return seedCount >= maxSeeds; } }

    public void AddSeed()
    {
        if (!Full)
        {
            seedCount++;
        }
        
    }

    private void Awake()
    {
        indicator.transform.parent = null;
        indicator.transform.rotation = Quaternion.identity;
        col = indicator.GetComponent<Collider2D>();
        sr = indicator.GetComponent<SpriteRenderer>();
    }
    public override void Using()
    {
        base.Using();
        indicator.transform.position = Vector3.Lerp(indicator.transform.position,mousePos,0.2f);
        List<Collider2D> hits = new List<Collider2D>();
        col.OverlapCollider(filter, hits);
        if(hits.Count > 0)
        {
            sr.color = unavailableColor;
            placable = false;
        }
        else
        {
            sr.color = availableColor;
            placable = true;
        }

        if(!placable)
        {
            seedInfo.text = "Space is ocupied!";
            sr.color = unavailableColor;
        }
        else
        {
            if(seedCount > 0)
            {
                seedInfo.text = string.Format($"Seeds : [{seedCount}/{maxSeeds}]");
                sr.color = availableColor;
            }
            else
            {
                seedInfo.text = string.Format($"No seeds! : [{seedCount}/{maxSeeds}]");
                sr.color = unavailableColor;
            }
        }

    }
    public override void OnSelect()
    {
        base.OnSelect();
        indicator.SetActive(true);
        seedInfo.enabled = true;
    }
    public override void OnDeselect()
    {
        base.OnDeselect();
        indicator.SetActive(false);
        seedInfo.enabled = false;
    }
    public override void OnLeftClickBegin()
    {
        base.OnLeftClickBegin();
        Debug.Log("CLICK");
        if(placable && seedCount > 0)
        {
            Instantiate(plantPrefab, indicator.transform.position, Quaternion.identity);
            AudioManager.PlayWithPitchDeviation("plant",1f,false,0.3f);
            placable = false;
            GameManager.Plant();
            seedCount--;
        }
        else
        {
            AudioManager.Play("deny", 1f);
        }
    }
}
