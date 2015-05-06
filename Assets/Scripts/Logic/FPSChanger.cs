using UnityEngine;
using System.Collections;

//Responsible for giving player control to a fps prefab
//TODO Implement choosing the spawn points on change
public class FPSChanger : MonoBehaviour{


	//Parralel lists of our FPSControllers and Enviroments
	//
	public GameObject[] Worlds;
	public GameObject[] FPSControllers;

	//Index of current active go
	private int m_currIndex;

	//Maps m_currIndex to an entry in Worlds and FPSControllers
	public int[] WorldOrder;
	public int[] Images;

	//
	//Images
	//

	public bool ShowingImage{get; set;}
	public Texture ToDraw;
	public float Left, Right, Top, Bot;

	//
	//Private data
	//

	//Ref to our logger
	private Logger logger;

	//
	//Unity callbacks
	//

	//Sets everything up
	void Awake(){
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

		//Setup mapping
		//
		WorldOrder = new int[Worlds.Length];
		Images = new int[Worlds.Length];

		//Setup logger
		//
		logger = GetComponent<Logger>();
		if(logger == null){
			Debug.LogError("Couldn't find logger!!");
			return;
		}
	}

	void Start(){
		//Disable all but one in scene
		//
		foreach(GameObject go in FPSControllers){
			go.SetActive(false);
		}
		FPSControllers[0].SetActive(true);

		//Setup image
		//
		ToDraw = Resources.Load("Stimuli/object_" + Images[WorldOrder[m_currIndex]].ToString("000") + ".png") as Texture;

		//Begin a trial
		logger.StartTrial("FPV", Vector2.zero, "--", FPSControllers[m_currIndex], Worlds[m_currIndex].transform.position);
	}
	
	//
	//Public Methods
	//

	//Sets currently selected FPSController to next in list
	public void CycleFPSController(){
		//Disable current
		FPSControllers[WorldOrder[m_currIndex]].SetActive(false);
		//Worlds[m_currIndex].SetActive(false);

		//Increment index
		m_currIndex++;

		//loop back around list if needed
		if(m_currIndex == FPSControllers.Length)
			m_currIndex = 0;

		//Enable this guy
		FPSControllers[WorldOrder[m_currIndex]].SetActive(true);
		ToDraw = Resources.Load("Stimuli/object_" + Images[WorldOrder[m_currIndex]].ToString("000") + ".png") as Texture;
		//Worlds[m_currIndex].SetActive(true);

		//Update logger
		logger.EndTrial();
		logger.StartTrial("FPV", Vector2.zero, "--",
				FPSControllers[WorldOrder[m_currIndex]], Worlds[WorldOrder[m_currIndex]].transform.position);
	}

	//Disables user input
	public void DisableInput(){
		
		((MonoBehaviour)FPSControllers[m_currIndex].GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()).enabled = false;
	}

	//Enables user input
	public void EnableInput(){
		((MonoBehaviour)FPSControllers[m_currIndex].GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()).enabled = true;
	}

	//If we want to show an image, we do!
	void OnGUI() {
		if(ShowingImage && ToDraw != null) {
			GUI.DrawTexture(new Rect(Left * Screen.width, Top * Screen.width, Right * Screen.width, Bot * Screen.width),
					ToDraw, ScaleMode.ScaleToFit, true, 10.0F);
		}
	}
}

