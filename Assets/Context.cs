using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Context : MonoBehaviour
{

    public Material wood;
    public Material brick;
    public Material gray;

    void Wood()
    {
        GetComponent<MeshRenderer>().material = wood;
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
