using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    Animator rb;
    public float shakeMagnitude;
    public bool shake = false;

    AudioSource landStart;
    private void Awake()
    {
        rb = GetComponent<Animator>();
    }
    private void Start()
    {
        landStart = AudioManager.Play("Robot/landStart");
    }
    private void Update()
    {
        if(shake)
        {
            CameraController.ShakeCamera(shakeMagnitude);
        }
    }
    public void PlayLanding()
    {
        AudioManager.FadeOut(landStart,5f);
        AudioManager.Play("music",0.2f,true);
        //AudioManager.Play("Robot/land");
    }
}
