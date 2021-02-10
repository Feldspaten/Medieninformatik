using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [Header("Game")]
    public Player player;
    public GameObject enemyContainer;
    [Header("UI")]
    public Text schokoText;
    public Text vanilleText;
    public Text healthText;
    public Text enemyText;
    public Text infoText;
    public Text endpunkteText;

    public bool IsGameEnded = false;
    private int conesActive = 0;
    private int maxCones = 1;
    private float gameTime = 60;
    private float timeRemaining;
    private int initialEnemyCount;

    public int Points = 0;

    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    private bool timerIsRunning;
    private bool IsMenuOpen = false;

    public GameObject GameCanvas;
    public GameObject RestartCanvas;
    public Canvas canvas;

    public AudioClip gameWonAudioClip;
    public AudioClip gameLostAudioClip;

    private AudioSource audioSource;

    void Awake()
    {
        GameCanvas.SetActive(true);
        timerIsRunning = true;
        timeRemaining = gameTime;
        instance = this;
    }
    private void Start()
    {
        Time.timeScale = 1f;
        infoText.gameObject.SetActive(false);
        initialEnemyCount = enemyContainer.GetComponentsInChildren<Enemy>().Length;
        audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsGameEnded)
        {
            if (!IsMenuOpen)
            {
                IsMenuOpen = true;
                Time.timeScale = 0f;
                GameObject.Find("Player").GetComponent<Player>().enabled = false;
                GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = false;
                GameCanvas.SetActive(false);
                RestartCanvas.SetActive(true);
                if (player.Killed)
                {
                    endpunkteText.text = $"Deine Punkte: {Points}\nVersuche es erneut!";
                }
                else
                {
                    endpunkteText.text = $"Deine Punktzahl: {Points}\nGut gemacht!";
                }
               
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

        }
        else
        {
            UpdateText();

            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    timeRemaining = 0;
                    timerIsRunning = false;
                    IsGameEnded = true;
                    audioSource.PlayOneShot(gameWonAudioClip);
                }
            }
            enemyText.text = "Fertige Eishörnchen:\n" + Points;

            if (conesActive < maxCones)
            {
                SelectCone();
            }

            if (player.Killed == true)
            {
                infoText.gameObject.SetActive(true);
                infoText.text = $"Du hast verloren!\nDeine Punktzahl ist: {Points}";
                audioSource.PlayOneShot(gameLostAudioClip);
                IsGameEnded = true;
                timerIsRunning = false;
            }
        }
    }

    public void SelectCone()
    {
        var cones = ObjectPoolingManager.Instance.Cones;
        int randomCone = Random.Range(1, cones.Count);
        var sorten = SetSorten();
        cones.ElementAt(randomCone).gameObject.GetComponent<ConeEnemy>().SetActive(sorten);
        cones.RemoveAt(randomCone);
        conesActive++;
    }

    public string[] SetSorten()
    {
        string sort = "";
        string[] sorten = new string[2];
        for (int i = 0; i < 2; i++)
        {
            var s = Random.Range(0, 2);
            if (s == 0)
            {
                sorten[i] = "vanille";
                sort = "V" + sort;
            }
            else
            {
                sorten[i] = "schokolade";
                sort = "S" + sort;
            }
        }
        ObjectiveController.Instance.SetIce(sort);
        return sorten;
    }

    private void UpdateText()
    {
        vanilleText.text = "" + player.VanilleAmmo;
        if (player.VanilleAmmo == 0)
        {
            vanilleText.color = Color.red;
        }
        else
        {
            vanilleText.color = Color.black;
        }
        schokoText.text = "" + player.SchokoladeAmmo;
        if (player.SchokoladeAmmo == 0)
        {
            schokoText.color = Color.red;
        }
        else
        {
            schokoText.color = Color.black;
        }
    }
}
