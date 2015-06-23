using UnityEngine;
using System.Collections;

public class PauseButton : MonoBehaviour {

	bool paused = false;

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)){
			paused = !paused;
			Time.timeScale = paused ? 0.0f : 1.0f;
		}
	}
}
