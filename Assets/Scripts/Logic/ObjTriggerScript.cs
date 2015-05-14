using UnityEngine;
using System.Collections;

//Contains more convienient info about
//spawn style prefabs
public class ObjTriggerScript : MonoBehaviour{
	//Inspector set int for which of the 4 spawns I am
	public int SpawnNumber = -1;

	//Time player can't input
	public float NoInputTime = 3.0f;

	//A timer
	private Timer timer;
	//Are we actively blocking input and showing the image?
	private bool active = false;

	//Reference to main configuration script
	private FPSChanger fps;

	//
	//Unity callbacks
	//

	void Awake(){
		fps = GameObject.FindWithTag("Logic").GetComponent<FPSChanger>();
		timer = new Timer();
	}

	void Update(){
		if(active && timer.isDone){
			fps.EnableInput();
			fps.StopShowingImage();
			active = false;

			fps.CycleFPSController();
		}
	}

	void OnTriggerEnter(Collider other){
		if(!active){
			fps.StartShowingImage(SpawnNumber);
			fps.DisableInput();
			timer.SetTimer(NoInputTime);
			active = true;
		}
	}
}

