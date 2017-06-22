using UnityEngine;
using System.Collections;

public class UnPause : MonoBehaviour {
	bool active = false;
	
	private FPSChanger fps;

	void Awake(){
		fps = GameObject.Find("Logic").GetComponent<FPSChanger>();
	}

	void OnTriggerEnter(Collider notused){
		active = true;
		Time.timeScale = 0.0f;
	}

	void Update(){
		if(!active) return;

		if(Input.GetKeyDown("p")){
			active = false;
			Time.timeScale = 1.0f;
			fps.CycleFPSController();
		}
	}
}

