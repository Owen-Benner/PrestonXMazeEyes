using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    public Transform camera;

    private float distX;
    private float distZ;

    private float face;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindWithTag("Player").transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<MeshRenderer>().enabled){
            transform.LookAt(camera);
            transform.forward *= -1;
        }
    }

}
