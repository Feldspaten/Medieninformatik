using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    public List<Image> heartList;

    private static HeartController instance;
    public static HeartController Instance { get { return instance; } }

    private int heartAmount = 2;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
        public void ReduceHeart()
    {
        heartList.ElementAt(heartAmount).enabled = false;
        heartAmount--;
    }
}
