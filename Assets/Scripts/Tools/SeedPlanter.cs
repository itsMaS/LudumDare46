using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    int seedCount = 1;
    bool placable = false;

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
        indicator.transform.position = mousePos;
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
    }
    public override void OnSelect()
    {
        base.OnSelect();
        indicator.SetActive(true);
    }
    public override void OnDeselect()
    {
        base.OnDeselect();
        indicator.SetActive(false);
    }
    public override void OnLeftClickBegin()
    {
        base.OnLeftClickBegin();
        Debug.Log("CLICK");
        if(placable && seedCount > 0)
        {
            Instantiate(plantPrefab, indicator.transform.position, Quaternion.identity);
            placable = false;
            seedCount--;
        }
    }
}
