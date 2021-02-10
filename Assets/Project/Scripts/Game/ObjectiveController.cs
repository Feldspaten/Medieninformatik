using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveController : MonoBehaviour
{
    public List<Image> iceList;

    private static ObjectiveController instance;
    public static ObjectiveController Instance { get { return instance; } }
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    public void SetIce(string kombination)
    {
        foreach (var ice in iceList)
        {
            ice.enabled = false;
            if (ice.name == kombination)
            {
                ice.enabled = true;
            }
        }
    }
}
