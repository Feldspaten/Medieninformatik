using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    private static ObjectPoolingManager instance;
    public static ObjectPoolingManager Instance { get { return instance; } }
    public GameObject bulletPrefab;
    public int bulletAmount = 20;

    private List<GameObject> bullets;
    // Start is called before the first frame update
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
    }

    public GameObject GetBullet(bool shotByPlayer)
    {
        foreach(GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                bullet.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
                return bullet;
            }
        }
        GameObject prefabInstace = Instantiate(bulletPrefab);
        prefabInstace.transform.SetParent(transform);
        prefabInstace.GetComponent<Bullet>().ShotByPlayer = shotByPlayer;
        bullets.Add(prefabInstace);
        return prefabInstace;
    }
}
