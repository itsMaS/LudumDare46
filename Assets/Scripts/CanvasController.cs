using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    Animator rb;
    public float shakeMagnitude;
    public bool shake = false;

    AudioSource landStart;
    AudioSource music;

    bool gameOver = false;
    private void Awake()
    {
        rb = GetComponent<Animator>();
    }
    private void Start()
    {
        landStart = AudioManager.Play("Robot/landStart");
        GameManager.GameOver += GameOver;
        GameManager.Planted += Planted;
        GameManager.Withered += Withered;
        GameManager.LevelProgressed += NextLevel;
    }
    private void Update()
    {
        if(shake)
        {
            CameraController.ShakeCamera(shakeMagnitude);
        }
        if(gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            AudioManager.FadeOut(music, 1f);
        }
    }
    public void PlayLanding()
    {
        music = AudioManager.Play("music",1f,true);
        AudioManager.FadeIn(music,5,1f);
        //AudioManager.Play("Robot/land");
    }
    public void GameOver()
    {
        LeanTween.cancelAll();
        if (PlayerPrefs.GetInt("Highscore") < GameManager.level)
        {
            PlayerPrefs.SetInt("Highscore",GameManager.level);
        }

        gameOverText.text = string.Format($"Your highscore is level [{PlayerPrefs.GetInt("Highscore")}]. Press space to restart.");

        GetComponent<Animator>().SetTrigger("GameOver");
        gameOver = true;
    }

    void Planted()
    {
        UpdateObjective();
    }
    void Withered()
    {
        UpdateObjective();
    }

    void UpdateObjective()
    {
        progressBar.minValue = 0;
        int nextLevelPlants = GameManager.instance.LevelRequirements[GameManager.level];
        progressBar.value = GameManager.plants;
        if(!blockUpdate)
        {
            progressBar.maxValue = nextLevelPlants;
            objectiveText.text = string.Format($"Plant more flowers! [{GameManager.plants}/{nextLevelPlants}]");
        }
    }
    void NextLevel(int level)
    {
        StartCoroutine(BlockLevelUpdate());
    }
    bool blockUpdate = false;
    IEnumerator BlockLevelUpdate()
    {
        blockUpdate = true;
        yield return new WaitForSeconds(0.5f);
        blockUpdate = false;
        UpdateObjective();
    }

    [SerializeField]
    Slider progressBar;
    [SerializeField]
    TextMeshProUGUI objectiveText;
    [SerializeField]
    TextMeshProUGUI gameOverText;
}
