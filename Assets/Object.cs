using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{

    public Material spr1;
    public Material spr2;
    public Material spr3;
    public Material spr4;
    public Material spr5;
    public Material spr6;
    public Material sprNull;

    void Start()
    {
        //this.Clear();
    }

    void Clear()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Sprite1()
    {
        GetComponent<MeshRenderer>().material = spr1;
        GetComponent<MeshRenderer>().enabled = true;
    }

    void Sprite2()
    {
        GetComponent<MeshRenderer>().material = spr2;
        GetComponent<MeshRenderer>().enabled = true;
    }

    void Sprite3()
    {
        GetComponent<MeshRenderer>().material = spr3;
        GetComponent<MeshRenderer>().enabled = true;
    }

    void Sprite4()
    {
        GetComponent<MeshRenderer>().material = spr4;
        GetComponent<MeshRenderer>().enabled = true;
    }

    void Sprite5()
    {
        GetComponent<MeshRenderer>().material = spr5;
        GetComponent<MeshRenderer>().enabled = true;
    }

    void Sprite6()
    {
        GetComponent<MeshRenderer>().material = spr6;
        GetComponent<MeshRenderer>().enabled = true;
    }

}
