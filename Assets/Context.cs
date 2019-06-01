using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context : MonoBehaviour
{

    public Material wood;
    public Material brick;
    public Material stone;
    public Material metal;
    public Material gray;

    void Wood()
    {
        GetComponent<MeshRenderer>().material = wood;
    }

    void Stone()
    {
        GetComponent<MeshRenderer>().material = stone;
    }

    void Metal()
    {
        GetComponent<MeshRenderer>().material = metal;
    }

    void Brick()
    {
        GetComponent<MeshRenderer>().material = brick;
    }

    void Gray()
    {
        GetComponent<MeshRenderer>().material = gray;
    }

}
