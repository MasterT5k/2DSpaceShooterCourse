using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Image _liveImage;
    [SerializeField]
    private Sprite[] _livesSprites;


    private GameManager _gameManager;
    
    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        if (_gameManager == null)
            Debug.LogError("Game Manager is NULL.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
            EditorApplication.ExitPlaymode();
        }
    }

    public void ChangeScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void ChangeLives(int currentlives)
    {
        _liveImage.sprite = _livesSprites[currentlives];

        if (currentlives == 0)
            GameOverSequence();
    }

    public void ChangeAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo + "/15";
    }

    public void UpdateThrusterBar(float elapsedTime, float burnLength)
    {
        _thrusterSlider.value = burnLength -= elapsedTime;
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();

        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);

        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER!";
            yield return new WaitForSeconds(0.5f);

            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
