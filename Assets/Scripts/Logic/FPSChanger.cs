using UnityEngine;
using System.Collections;

//Responsible for giving player control to a fps prefab
public class FPSChanger : MonoBehaviour{

	//
	//Private data
	//

	//List of our FPSControllers
	//
	//Populated by users
	public GameObject[] FPSControllers;
	//index of current active go
	private int m_currIndex;

	//Timer for switching FPSController
	//
	private Timer m_switchTimer;
	public float SwitchTime;

	//
	//Unity callbacks
	//

	void Awake(){
		//Make sure we're cool
		if(FPSControllers == null){
			Debug.LogError("FPSControllers is empty!!");
			Application.Quit();
		}

		//init index
		m_currIndex = 0;

		//Disable all but one in scene
		foreach(GameObject go in FPSControllers){
			go.SetActive(false);
		}
		FPSControllers[0].SetActive(true);
	}
	
	//Update is called once per frame
	void Update(){
		//Timer to switch controlled guy
		if(m_switchTimer.isDone){
			CycleFPSController();
			m_switchTimer.SetTimer(SwitchTime);
		}
	}

	//
	//Public Methods
	//

	//Sets currently selected FPSController to next in list
	public void CycleFPSController(){
		//Disable current FSPCont
		FPSControllers[m_currIndex++].SetActive(false);

		//loop back around list if needed
		if(m_currIndex == FPSControllers.Length)
			m_currIndex = 0;

		//Enable this guy
		FPSControllers[m_currIndex].SetActive(true);
	}

}

