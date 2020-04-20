using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject popup;
    [SerializeField]
    bool debugMode = false;

    public static GameManager instance;

    private void Awake()
    {
        ResetStaticVariables();
        GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
        GraphicsSettings.transparencySortAxis = new Vector3(0.0f, 1.0f, 0.0f);
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if (debugMode)
        {
            foreach (Animator item in FindObjectsOfType<Animator>())
            {
                item.speed = 20;
            }
            StartCoroutine(BackToNormal());
        }
        else
        {
            Time.timeScale = 1;
        }

        plants = 0;
        level = 0;
    }
    public static void MakePopup(string message, Vector3 position)
    {
        Instantiate(GameManager.instance.popup, position, Quaternion.identity).GetComponent<Popup>().Initialize(message);
    }

    public static Action<bool> EnableTutorial;
    bool tutorialEnabled = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) || Input.GetKeyDown(KeyCode.F))
        {
            if(tutorialEnabled)
            {
                EnableTutorial(false);
                tutorialEnabled = false;
            }
            else
            {
                EnableTutorial(true);
                tutorialEnabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.K) && debugMode)
        {
            foreach (Animator item in FindObjectsOfType<Animator>())
            {
                item.speed = 1;
            }
        }
    }

    IEnumerator BackToNormal()
    {
        yield return new WaitForSecondsRealtime(3);
        foreach (Animator item in FindObjectsOfType<Animator>())
        {
            item.speed = 1;
        }
    }

    [SerializeField]
    TextMeshProUGUI objectiveText;
    [SerializeField]
    Slider objectiveSlider;

    public static int plants = 0;
    public static int level = 0;

    [SerializeField]
    public int[] LevelRequirements;

    public static void Plant()
    {;
        plants++;
        if(plants >= instance.LevelRequirements[level])
        {
            level++;
            LevelProgressed?.Invoke(level);
        }
        Planted?.Invoke();
    }
    public static void Wither()
    {
        plants--;
        if(plants <= 0)
        {
            Debug.Log("Game over");
            GameOver();
        }
        Withered?.Invoke();
    }
    public static Action GameOver;
    public static Action Withered;
    public static Action Planted;
    public static Action<int> LevelProgressed;

    void ResetStaticVariables()
    {
        EnableTutorial = null;
        GameOver = null;
        Withered = null;
        Planted = null;
        LevelProgressed = null;
        plants = 0;
        level = 0;
    }
}
