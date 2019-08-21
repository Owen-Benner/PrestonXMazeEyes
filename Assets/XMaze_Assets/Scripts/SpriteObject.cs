using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteObject : MonoBehaviour
{

    public Material spr1;
    public Material spr2;
    public Material spr3;
    public Material spr4;
    public Material spr5;
    public Material spr6;

    void Start()
    {
        this.Sprite(0);
    }

    void Sprite(int num)
    {
        if(num == 0)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else if(num == 1)
        {
            GetComponent<MeshRenderer>().material = spr1;
            GetComponent<MeshRenderer>().enabled = true;
        }
        else if(num == 2)
        {
            GetComponent<MeshRenderer>().material = spr2;
            GetComponent<MeshRenderer>().enabled = true;
        }
        else if(num == 3)
        {
            GetComponent<MeshRenderer>().material = spr3;
            GetComponent<MeshRenderer>().enabled = true;
        }
        else if(num == 4)
        {
            GetComponent<MeshRenderer>().material = spr4;
            GetComponent<MeshRenderer>().enabled = true;
        }
        else if(num == 5)
        {
            GetComponent<MeshRenderer>().material = spr5;
            GetComponent<MeshRenderer>().enabled = true;
        }
        else if(num == 6)
        {
            GetComponent<MeshRenderer>().material = spr6;
            GetComponent<MeshRenderer>().enabled = true;
        }
        //Debug.Log(name + ", Sprite: " + num);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.SendMessage("GiveReward", gameObject);
        }
    }

}
