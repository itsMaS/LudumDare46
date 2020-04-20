using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialWindow : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI header;
    [SerializeField]
    TextMeshProUGUI footer;

    [SerializeField]
    CanvasGroup cg;

    [SerializeField]
    bool startEnabled;
    [SerializeField]
    string headerName;
    [SerializeField]
    [TextArea]
    string description;

    public enum TrackMode { mouse, stationary, startingPoint }
    [SerializeField]
    TrackMode selectedMode;

    Transform followPosition;
    Vector3 stationaryOffset;

    private void Start()
    {
        GameManager.EnableTutorial += EnableGlobal;
        cg.alpha = 0;

        if (selectedMode == TrackMode.startingPoint || selectedMode == TrackMode.mouse)
        {
            stationaryOffset = transform.localPosition;
            followPosition = transform.parent;
            transform.parent = null;
            transform.rotation = Quaternion.identity;
        }
        gameObject.name = headerName;
        local = startEnabled;
        UpdateWindow();
    }

    bool global;
    bool local = false;
    void EnableGlobal(bool global)
    {
        this.global = global;
        UpdateWindow();
    }
    public void EnableLocal(bool local)
    {
        this.local = local;
        UpdateWindow();
    }
    public void UpdateWindow()
    {
        if (global && local && cg)
        {
            cg.LeanAlpha(1,0.2f);
        }
        else
        {
            cg.LeanAlpha(0,0.2f);
        }
    }
    public void SetHeader(string text)
    {
        header.text = text;
    }
    public void SetFooter(string text)
    {
        footer.text = text;
    }

    private void OnValidate()
    {
        if(header)
        header.text = headerName;
        if(footer)
        footer.text = description;
    }

    private void Update()
    {
        if(selectedMode == TrackMode.startingPoint)
        transform.position = followPosition.position;
        else if(selectedMode == TrackMode.mouse)
        {
            transform.position = Vector3.Lerp(transform.position, PlayerController.mousePosition+stationaryOffset,0.2f);
        }
    }
}
