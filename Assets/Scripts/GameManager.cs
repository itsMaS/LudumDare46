using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject popup;
    [SerializeField]
    bool debugMode = false;

    public static GameManager instance;

    private void Awake()
    {
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
                item.speed = 10;
            }
            StartCoroutine(BackToNormal());
        }
    }
    public static void MakePopup(string message, Vector3 position)
    {
        Instantiate(GameManager.instance.popup, position, Quaternion.identity).GetComponent<Popup>().Initialize(message);
    }
    private void Update()
    {
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
        yield return new WaitForSecondsRealtime(2);
        foreach (Animator item in FindObjectsOfType<Animator>())
        {
            item.speed = 1;
        }
    }
}
