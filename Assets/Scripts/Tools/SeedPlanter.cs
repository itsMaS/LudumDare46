using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPlanter : Tool
{
    [SerializeField]
    GameObject indicator;

    private void Awake()
    {
        indicator.transform.parent = null;
        indicator.transform.rotation = Quaternion.identity;
    }
    public override void Using()
    {
        base.Using();
        indicator.transform.position = mousePos;
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
}
