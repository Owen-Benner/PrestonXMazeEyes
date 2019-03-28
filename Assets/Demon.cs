using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : MonoBehaviour
{

    public int mode;

    public float selectTime;
    public float turnTime;
    public float slideTime;
    public float totalTime;

    public float startPos;

    public int[] contexts;
    public float[] holds;

    public int[] leftObjects;
    public int[] leftRewards;
    public int[] rightObjects;
    public int[] rightRewards;

    public enum segment
    {
        Hallway, HoldA, Selection,
        Reward, Return, HoldB,
    };

    public GameObject contextN;
    public GameObject contextS;

    public GameObject objectNE;
    public GameObject objectSE;
    public GameObject objectSW;
    public GameObject objectNW;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
