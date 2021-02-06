using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public float lifeDuration = 2f;
    public int damage = 5;
    private string currentFarbe;
    public string CurrentFarbe => currentFarbe;
    public GameObject bulletGameObject;
    public Material schokoladeMaterial;
    public Material vanilleMaterial;


    private float lifeTimer;

    private bool shotByPlayer;
    
    public bool ShotByPlayer { get { return shotByPlayer; } set { shotByPlayer = value; } }


    void OnEnable()
    {
        lifeTimer = lifeDuration;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //make the bullet move
        transform.position += transform.forward * speed * Time.deltaTime;

        //Bullet destroy check
        lifeTimer -= Time.deltaTime;
        if(lifeTimer <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMaterial(string farbe)
    {
        if (farbe == "vanille")
        {
            bulletGameObject.GetComponent<MeshRenderer>().material = vanilleMaterial;
            currentFarbe = farbe;
        }else if (farbe == "schokolade")
        {
            bulletGameObject.GetComponent<MeshRenderer>().material = schokoladeMaterial;
            currentFarbe = farbe;
        }
        
    }
}
