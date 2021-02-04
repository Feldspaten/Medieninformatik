using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeEnemy : MonoBehaviour
{
    public GameObject ScoopTop;
    public GameObject ScoopBottom;
    public Material vanilleMaterial;
    public Material schokoladeMaterial;
    private int timesHit;

    void Start()
    {
        //ScoopTop.SetActive(false);
        //ScoopBottom.SetActive(false);
        ScoopTop.GetComponent<MeshRenderer>().material = vanilleMaterial;
        ScoopBottom.GetComponent<MeshRenderer>().material = schokoladeMaterial;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
