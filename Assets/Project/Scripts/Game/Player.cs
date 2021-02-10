using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public Camera playerCamera;

    [Header("Gameplay")]
    public int initialHealth = 3;

    public int initialScoops = 5;

    public float knockbackForce = 10f;
    public float hurtDuration = 0.5f;
    public Material vanilleMaterial;
    public Material schokoladeMaterial;

    private int vanilleCount = 0;
    private int schokoladeCount = 0;
    public int VanilleAmmo { get { return vanilleCount; } }
    public int SchokoladeAmmo { get { return schokoladeCount; } }


    private int health;
    public int Health { get { return health; } }

    private bool killed;
    public bool Killed { get { return killed; } }

    private bool isHurt;
    public GameObject ShootPoint;
    private AudioSource m_AudioSource;
    public AudioClip m_playerHurt;

    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        health = initialHealth;
        vanilleCount = initialScoops;
        schokoladeCount = initialScoops;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(vanilleCount > 0)
            {
                vanilleCount--;
                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true, "vanille");
                bulletObject.GetComponent<MeshRenderer>().material = vanilleMaterial;
                bulletObject.transform.position = ShootPoint.transform.position + playerCamera.transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
            }   
        }

        if (Input.GetMouseButtonDown(1))
        {
            
            if (schokoladeCount > 0)
            {
                schokoladeCount--;
                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true, "schokolade");
                bulletObject.GetComponent<MeshRenderer>().material = schokoladeMaterial;
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
            }
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {

        if (otherCollider.gameObject.GetComponent<Barrel>() != null)
        {
            //Collect ammo crates.
            Barrel barrel = otherCollider.gameObject.GetComponent<Barrel>();
            if (barrel.type == "vanille")
            {
                vanilleCount = initialScoops;
                barrel.Pickup();
                ObjectPoolingManager.Instance.vanilleBarrelActive = false;
            }
            else if (barrel.type == "schokolade")
            {
                schokoladeCount = initialScoops;
                barrel.Pickup();
                ObjectPoolingManager.Instance.schokoladeBarrelActive = false;
            }
        }

        if (isHurt == false)
        {
            GameObject hazard = null;
            if (otherCollider.gameObject.GetComponent<Enemy>() != null)
            {
                //Touching Enemys
                Enemy enemy = otherCollider.gameObject.GetComponent<Enemy>();
                hazard = enemy.gameObject;
                health -= enemy.damage;
            } 
            else if (otherCollider.GetComponent<Bullet>() != null)
            {
                Bullet bullet = otherCollider.GetComponent<Bullet>();
                if(bullet.ShotByPlayer == false)
                {
                    hazard = bullet.gameObject;
                    health -= bullet.damage;
                    HeartController.Instance.ReduceHeart();
                    m_AudioSource.clip = m_playerHurt;
                    m_AudioSource.Play();
                    bullet.gameObject.SetActive(false);
                }
            }

            if(hazard != null)
            {
                isHurt = true;

                //Perform the knockback effect
                Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
                Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
                GetComponent<ForceReciever>().AddForce(knockbackDirection, knockbackForce);

                StartCoroutine(HurtRoutine());
            }

            if(health <= 0)
            {
                if(killed == false)
                {
                    killed = true;
                    OnKill();
                }
            }
        }
    }

    IEnumerator HurtRoutine()
    {
        yield return new WaitForSeconds(hurtDuration);
        isHurt = false;
    }

    private void OnKill()
    {

    }
}
