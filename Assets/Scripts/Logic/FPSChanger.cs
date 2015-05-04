using UnityEngine;
using System.Collections;

//Responsible for giving player control to a fps prefab
public class FPSChanger : MonoBehaviour{


	//Parralel lists of our FPSControllers and Enviroments
	//
	public GameObject[] Worlds;
	public GameObject[] FPSControllers;

	//Index of current active go
	private int m_currIndex;

	//
	//Private data
	//

	//Timer for switching FPSController
	//
	private Timer m_switchTimer;
	public float SwitchTime;

	//Ref to our logger
	private Logger logger;

	//
	//Unity callbacks
	//

	//Sets everything up
	void Start(){
		//Make sure we're cool
		//
		if(FPSControllers == null){
			Debug.LogError("List of FPSControllers is empty!!");
			Application.Quit();
		}
		if(Worlds == null){
			Debug.LogError("List of Worlds is empty!!");
			Application.Quit();
		}

		//Setup list
		//
		m_currIndex = 0;

		//Disable all but one in scene
		//
		foreach(GameObject go in FPSControllers){
			go.SetActive(false);
		}
		FPSControllers[0].SetActive(true);

		//Setup timer
		//
		m_switchTimer.SetTimer(SwitchTime);

		//Setup logger
		//
		logger = GetComponent<Logger>();
		if(logger == null){
			Debug.LogError("Couldn't find logger!!");
			return;
		}
		logger.StartTrial("FPV", Vector2.zero, "--", FPSControllers[m_currIndex], Worlds[m_currIndex].transform.position);
	}
	
	//Cycles to the next fps controller
	void Update(){
		//Timer to switch controlled guy
		if(m_switchTimer.isDone){
			//TODO Add fade to black

			//Update game
			CycleFPSController();
			//Reset timer
			m_switchTimer.SetTimer(SwitchTime);
		}
	}

	//
	//Public Methods
	//

	//Sets currently selected FPSController to next in list
	public void CycleFPSController(){
		//Disable current
		FPSControllers[m_currIndex].SetActive(false);
		//Worlds[m_currIndex].SetActive(false);

		//Increment index
		m_currIndex++;

		//loop back around list if needed
		if(m_currIndex == FPSControllers.Length)
			m_currIndex = 0;

		//Enable this guy
		FPSControllers[m_currIndex].SetActive(true);
		//Worlds[m_currIndex].SetActive(true);

		//Update logger
		logger.EndTrial();
		logger.StartTrial("FPV", Vector2.zero, "--", FPSControllers[m_currIndex], Worlds[m_currIndex].transform.position);
	}
}

