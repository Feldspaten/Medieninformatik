using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoops : MonoBehaviour
{
    public bool active = false;
    public GameObject scoopGameObject;
    void Start()
    {
        scoopGameObject.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
