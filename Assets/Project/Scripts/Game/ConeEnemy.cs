using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ConeEnemy : MonoBehaviour
{
    public GameObject ScoopTop;
    public GameObject ScoopBottom;
    public Material vanilleMaterial;
    public Material schokoladeMaterial;
    public GameObject arrowGameObject;

    [Header("Visuals")]
    public GameObject arrowContainer;

    public float arrowRotationSpeed = 20f;
    private int timesHit = 0;
    private bool isSelected = false;
    public bool wasSelected = false;
    private Player player;
    private int falseHits = 0;
    private NavMeshAgent agent;
    private float walkTime = 2f;
    private string[] sorten;
    private bool killed = false;
    private AudioSource audioSource;

    public AudioClip filledAudioClip;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>();
        ScoopTop.SetActive(false);
        ScoopBottom.SetActive(false);
        agent.SetDestination(RandomNavmeshLocation(4f));
        audioSource = GameObject.Find("Player").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (killed)
        {
            transform.Translate(Vector3.up * 3 * Time.deltaTime, Space.World);
        }
        else
        {
            if (walkTime < 0)
            {
                agent.SetDestination(RandomNavmeshLocation(4f));
                walkTime = 1f;
            }
            else
            {
                walkTime -= Time.deltaTime;
            }
            arrowContainer.transform.Rotate(Vector3.up * arrowRotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider.GetComponent<Bullet>() != null)
        {
            Bullet bullet = otherCollider.GetComponent<Bullet>();
            if (bullet.ShotByPlayer == true)
            {
                bullet.gameObject.SetActive(false);
                if (isSelected)
                {
                    
                    if (bullet.CurrentFarbe == sorten[timesHit])
                    {
                        timesHit++;
                        switch (timesHit)
                        {
                            case 1:
                                SetFarbeActive(ScoopBottom, bullet.CurrentFarbe);
                                break;
                            case 2:
                                SetFarbeActive(ScoopTop, bullet.CurrentFarbe);
                                OnKill();
                                break;
                        }
                    }
                    else
                    {
                        timesHit = 0;
                        ScoopBottom.SetActive(false);
                        ScoopTop.SetActive(false);
                        GameObject bulletLeft = ObjectPoolingManager.Instance.GetBullet(false, bullet.CurrentFarbe);
                        GameObject bulletCenter = ObjectPoolingManager.Instance.GetBullet(false, bullet.CurrentFarbe);
                        GameObject bulletRight = ObjectPoolingManager.Instance.GetBullet(false, bullet.CurrentFarbe);
                        bulletLeft.transform.position = transform.position + new Vector3(0.25f, 1.7f, 0.6f);
                        bulletLeft.transform.forward = (player.transform.position - transform.position).normalized;
                        bulletLeft.transform.RotateAround(bulletLeft.transform.position, Vector3.up, -20f);

                        bulletCenter.transform.position = transform.position + new Vector3(0.25f, 1.7f, 0.6f);
                        bulletCenter.transform.forward = (player.transform.position - transform.position).normalized;

                        bulletRight.transform.position = transform.position + new Vector3(0.25f, 1.7f, 0.6f);
                        bulletRight.transform.forward = (player.transform.position - transform.position).normalized;
                        bulletRight.transform.RotateAround(bulletLeft.transform.position, Vector3.up, 20f);
                    }
                    


                }
                else
                {
                    falseHits++;
                    switch (falseHits)
                    {
                        case 1:
                            GameObject bulletSingle = ObjectPoolingManager.Instance.GetBullet(false, bullet.CurrentFarbe);
                            bulletSingle.transform.position = transform.position + new Vector3(0.25f, 1.7f, 0.6f);
                            bulletSingle.transform.forward = (player.transform.position - transform.position).normalized;
                            break;
                        default:
                            GameObject bulletLeft = ObjectPoolingManager.Instance.GetBullet(false, bullet.CurrentFarbe);
                            GameObject bulletCenter = ObjectPoolingManager.Instance.GetBullet(false, bullet.CurrentFarbe);
                            GameObject bulletRight = ObjectPoolingManager.Instance.GetBullet(false, bullet.CurrentFarbe);
                            bulletLeft.transform.position = transform.position + new Vector3(0.25f, 1.7f, 0.6f);
                            bulletLeft.transform.forward = (player.transform.position - transform.position).normalized;
                            bulletLeft.transform.RotateAround(bulletLeft.transform.position, Vector3.up, -20f);

                            bulletCenter.transform.position = transform.position + new Vector3(0.25f, 1.7f, 0.6f);
                            bulletCenter.transform.forward = (player.transform.position - transform.position).normalized;

                            bulletRight.transform.position = transform.position + new Vector3(0.25f, 1.7f, 0.6f);
                            bulletRight.transform.forward = (player.transform.position - transform.position).normalized;
                            bulletRight.transform.RotateAround(bulletLeft.transform.position, Vector3.up, 20f);
                            break;
                    }
                    if (falseHits == 0)
                    {

                    }
                }
            }
        }
    }

    private void SetFarbeActive(GameObject scoopGameObject, string farbe)
    {
        if (farbe == "vanille")
        {
            scoopGameObject.GetComponent<MeshRenderer>().material = vanilleMaterial;
        }else if (farbe == "schokolade")
        {
            scoopGameObject.GetComponent<MeshRenderer>().material = schokoladeMaterial;
        }
        scoopGameObject.SetActive(true);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void SetActive(string[] s)
    {
        sorten = s;
        arrowGameObject.SetActive(true);
        isSelected = true;
        wasSelected = true;
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    protected void OnKill()
    {
        isSelected = false;
        killed = true;
        audioSource.PlayOneShot(filledAudioClip);
        GameController.Instance.Points++;
        GameController.Instance.SelectCone();
        agent.enabled = false;
        //this.enabled = false;
        this.arrowContainer.SetActive(false);
    }
}

