using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visuals")]
    public Camera playerCamera;

    [Header("Gameplay")]
    public int initialHealth = 100;
    public int initialAmmo = 12;
    public float knockbackForce = 10f;
    public float hurtDuration = 0.5f;


    private int ammo;
    public int Ammo { get { return ammo; } }

    private int health;
    public int Health { get { return health; } }

    private bool killed;
    public bool Killed { get { return killed; } }

    private bool isHurt;
    // Start is called before the first frame update
    void Start()
    {
        health = initialHealth;
        ammo = initialAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(ammo > 0)
            {
                ammo--;
                var bulletObject = ObjectPoolingManager.Instance.GetBullet(true, "vanille");
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
            }   
        }

        if (Input.GetMouseButtonDown(1))
        {
            
            if (ammo > 0)
            {
                ammo--;
                GameObject bulletObject = ObjectPoolingManager.Instance.GetBullet(true, "schokolade");
                bulletObject.transform.position = playerCamera.transform.position + playerCamera.transform.forward;
                bulletObject.transform.forward = playerCamera.transform.forward;
            }
        }
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        
        if (otherCollider.gameObject.GetComponent<AmmoCrate>() != null)
        {
            //Collect ammo crates.
            AmmoCrate ammoCrate = otherCollider.gameObject.GetComponent<AmmoCrate>();
            ammo += ammoCrate.ammo;
            Destroy(ammoCrate.gameObject);
        }



        //if (isHurt == false)
        //{
        //    GameObject hazard = null;
        //    if (otherCollider.gameObject.GetComponent<Enemy>() != null)
        //    {
        //        //Touching Enemys
        //        Enemy enemy = otherCollider.gameObject.GetComponent<Enemy>();
        //        hazard = enemy.gameObject;
        //        health -= enemy.damage;
        //    } 
        //    else if (otherCollider.GetComponent<Bullet>() != null)
        //    {
        //        Bullet bullet = otherCollider.GetComponent<Bullet>();
        //        if(bullet.ShotByPlayer == false)
        //        {
        //            hazard = bullet.gameObject;
        //            health -= bullet.damage;
        //            bullet.gameObject.SetActive(false);
        //        }
        //    }
        //    else if (otherCollider.gameObject.GetComponent<ConeEnemy>() != null)
        //    {
        //        Debug.Log("Hörnchen getroffen");
        //    }

        //    if (hazard != null)
        //    {
        //        isHurt = true;

        //        //Perform the knockback effect
        //        Vector3 hurtDirection = (transform.position - hazard.transform.position).normalized;
        //        Vector3 knockbackDirection = (hurtDirection + Vector3.up).normalized;
        //        GetComponent<ForceReciever>().AddForce(knockbackDirection, knockbackForce);

        //        StartCoroutine(HurtRoutine());
        //    }

        //    if(health <= 0)
        //    {
        //        if(killed == false)
        //        {
        //            killed = true;
        //            OnKill();
        //        }
        //    }
        //}
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
