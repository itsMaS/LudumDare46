using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    static Tool selected;
    [SerializeField]
    bool updateWhenDeselected = false;

    protected bool leftClicked;
    protected bool rightClicked;
    protected Vector3 mousePos;
    public void SelectTool()
    {
        if(this != selected)
        {
            if(selected)
            {
                selected.OnDeselect();
            }
            selected = this;
            OnSelect();
        }
    }
    public virtual void OnSelect()
    {

    }
    public virtual void OnDeselect()
    {

    }
    public virtual void OnLeftClick()
    {

    }
    public virtual void OnRightClick()
    {

    }
    public virtual void OnLeftClickBegin()
    {

    }
    public virtual void OnRightClickBegin()
    {

    }
    public virtual void OnLeftClickEnd()
    {

    }
    public virtual void OnRightClickEnd()
    {

    }
    public virtual void Using()
    {

    }
    public virtual void Update()
    {
        mousePos = PlayerController.mousePosition;
        if(selected == this)
        {
            Using();
        }
        if(selected == this || updateWhenDeselected)
        {
            if (leftClicked = Input.GetMouseButton(0))
            {
                OnLeftClick();
            }
            if (rightClicked = Input.GetMouseButton(1))
            {
                OnRightClick();
            }
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClickBegin();
            }
            if (Input.GetMouseButtonDown(1))
            {
                OnRightClickBegin();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnLeftClickEnd();
            }
            if (Input.GetMouseButtonUp(1))
            {
                OnRightClickEnd();
            }
        }
    }
}
