using UnityEngine;
using System.Collections;

//Contains more convienient info about
//spawn style prefabs
public class ObjTriggerScript : MonoBehaviour{
	private FPSChanger fps;

	public float NoInputTime = 3.0f;
	private Timer timer;
	private bool active = false;

	void Awake(){
		fps = GameObject.FindWithTag("Logic").GetComponent<FPSChanger>();
		timer = new Timer();
	}

	void Update(){
		if(active && timer.isDone){
			fps.EnableInput();
			fps.CycleFPSController();
			fps.ShowingImage = false;

			active = false;
		}
	}

	void OnTriggerEnter(Collider other){
		//Stop input for x seconds
		//Show Obj
		//renable input
		//Switch fps

		fps.ShowingImage = true;
		fps.DisableInput();
		timer.SetTimer(NoInputTime);
	}
}

