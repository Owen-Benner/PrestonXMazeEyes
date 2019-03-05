using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    public float holdLength;

    public GameObject contextA;
    public GameObject contextB;

    public string contextAName;
    public string contextBName;

    public string mat;

    void Start()
    {
        contextA = GameObject.Find(contextAName);
        contextB = GameObject.Find(contextBName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.SendMessage("BeginHold", holdLength);
            contextA.SendMessage(mat);
            contextB.SendMessage(mat);
        }
    }

}
