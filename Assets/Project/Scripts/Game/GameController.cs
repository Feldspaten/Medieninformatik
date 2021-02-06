using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Game")]
    public Player player;
    public GameObject enemyContainer;
    [Header("UI")]
    public Text ammoText;
    public Text healthText;
    public Text enemyText;
    public Text infoText;

    private int conesActive = 0;
    private int maxCones = 1;

    private int initialEnemyCount;

    private void Start()
    {
        infoText.gameObject.SetActive(false);
        initialEnemyCount = enemyContainer.GetComponentsInChildren<Enemy>().Length;
    }
    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health " + player.Health;
        ammoText.text = "Ammo: " + player.Ammo;

        if (conesActive < maxCones)
        {
            SelectCone();
            conesActive++;
        }

        int aliveEnemies = 0;
        foreach(Enemy enemy in enemyContainer.GetComponentsInChildren<Enemy>())
        {
            if(enemy.Killed == false)
            {
                aliveEnemies++;
            }
        }
        enemyText.text = "Enemies: " + aliveEnemies;

        if(aliveEnemies == 0)
        {
            infoText.gameObject.SetActive(true);
            //infoText.text = "You win!\nGood Job!";
        }

        if(player.Killed == true)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = "You lose :(\nTry again!";
        }
    }

    public void SelectCone()
    {
        var cones = ObjectPoolingManager.Instance.Cones;
        int randomCone = Random.Range(1, cones.Count);
        cones.ElementAt(randomCone).gameObject.GetComponent<ConeEnemy>().SetActiveArrow();
    }
}
