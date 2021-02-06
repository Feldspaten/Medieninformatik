using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectPoolingManager : MonoBehaviour
{
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance { get { return instance; } }
    public GameObject bulletPrefab;
    public GameObject conePrefab;
    public int bulletAmount = 20;
    public int coneAmount = 10;

    private List<GameObject> bullets;

    private List<GameObject> cones;
    public List<GameObject> Cones => cones;

    public List<GameObject> spawnPoints;
   
    void Awake()
    {
        instance = this;
        bullets = new List<GameObject>(bulletAmount);
        for(int i = 0; i < bulletAmount; i++)
        {
            GameObject prefabInstace = Instantiate(bulletPrefab);
            prefabInstace.transform.SetParent(transform);
            prefabInstace.SetActive(false);
            bullets.Add(prefabInstace);
        }

        cones = new List<GameObject>(coneAmount);
        for (int i = 0; i < coneAmount; i++)
        {
            GameObject prefabInstace = Instantiate(conePrefab);
            prefabInstace.transform.SetParent(transform);
            prefabInstace.SetActive(false);
            cones.Add(prefabInstace);
        }
        SpawnCones();
    }

    public GameObject GetBullet(bool shotByPlayer, string farbe)
    {
        foreach(GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
                bullet.GetComponent<Bullet>().SetMaterial(farbe);
                return bullet;
            }
        }
        GameObject prefabInstace = Instantiate(bulletPrefab);
        prefabInstace.transform.SetParent(transform);
        prefabInstace.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
        bullets.Add(prefabInstace);
        return prefabInstace;
    }

    public List<GameObject> GetBullets(int amount, bool shotByPlayer, string farbe)
    {
        List<GameObject> bulletList = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            foreach (GameObject bullet in bullets)
            {
                if (!bullet.activeInHierarchy)
                {
                    bullet.SetActive(true);
                    bullet.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
                    bullet.GetComponent<Bullet>().SetMaterial(farbe);
                    bullets.Add(bullet);
                }
            }
            GameObject prefabInstace = Instantiate(bulletPrefab);
            prefabInstace.transform.SetParent(transform);
            prefabInstace.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
            bullets.Add(prefabInstace);
            bulletList.Add(prefabInstace);
        }
        return bulletList;
    }

    public void SpawnCones()
    {
        foreach (var cone in cones)
        {
            int spawn = Random.Range(0, coneAmount - 1);
            var obj = cone.gameObject.GetComponent<ConeEnemy>();
            obj.SetPosition(spawnPoints.ElementAt(spawn).transform.position);
            cone.SetActive(true);
        }
    }
}
