using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    public GameObject player;

    private float distX;
    private float distZ;

    private float face;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<MeshRenderer>().enabled){
            transform.LookAt(player.transform);
            transform.forward *= -1;
        }
    }

}
