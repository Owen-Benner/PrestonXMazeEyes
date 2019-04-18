using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    AsyncOperation async;

    // Start is called before the first frame update
    void Start()
    {
        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("5"))
        {
            async.allowSceneActivation = true;
        }
    }
}
