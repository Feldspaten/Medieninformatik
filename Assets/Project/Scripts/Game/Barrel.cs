using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [Header("Gameplay")]
    public int ammo = 5;
    public string type = "";

    private AudioSource audioSource;

    public AudioClip pickupAudioClip;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().enabled = true;
        this.GetComponent<CapsuleCollider>().enabled = true;
        audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
    }

    public void Pickup()
    {
        
        audioSource.PlayOneShot(pickupAudioClip);
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(PickupDestroy(1));

    }

    IEnumerator PickupDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        
        Destroy(this.gameObject);
    }
}
