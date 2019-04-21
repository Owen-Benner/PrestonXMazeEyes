using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private AsyncOperation async;
    private bool counting = false;
    public float countDown = 4f;

    // Start is called before the first frame update
    void Start()
    {
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(counting)
        {
            countDown -= Time.deltaTime;
            if(countDown <= 0)
            {
                async.allowSceneActivation = true;
            }
        }

        if(Input.GetKey("5"))
        {
            counting = true;
        }
    }
}
