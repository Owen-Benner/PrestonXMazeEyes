using UnityEngine;
using System.Collections;

//Responsible for giving player control to a fps prefab
public class FPSChanger : MonoBehaviour{


	//Parralel lists of our FPSControllers and Enviroments
	//
	//Setup in Inspector
	//
	public GameObject[] Worlds;
	public GameObject[] FPSControllers;

	//Goals and spawns
	//
	//Player Spawns
	public GameObject[,] PlayerSpawns;
	//Object spawns
	public GameObject[,] ObjSpawns;
	//GO that holds the player and obj spawns
	public GameObject[] SpawnObjects;

	//For a given world, which spawn are we using?
	public WorldState[] ChosenSpawns;

	//Maps m_currFPSIndex to an entry in Worlds and FPSControllers
	public int[] WorldOrder;

	//Index of current active FPSController go
	private int m_currFPSIndex;

	//
	//Images
	//

	//Image assets, per world, for objs
	//Use this like Images[world, spawn]
	//This should give back which img file to use
	public int[,] Images;

	//Are we showing the on screen image right now?
	private bool imageEnabled = false;

	//The image to be displayed on screen
	private Texture imageToDraw;

	//Inspector configurable image overlay borders; percentage of the screen in [0, 1]
	public float Left, Right, Top, Bot;

	//
	//Private data
	//

	//Ref to our logger
	private Logger logger;

	//
	//Helper Methods
	//


	//Begin a trial
	//
	private void BeginLogging(){
		logger.StartTrial("FPV", Vector2.zero, "--",
				FPSControllers[WorldOrder[m_currFPSIndex]], Worlds[WorldOrder[m_currFPSIndex]].transform.position);
	}

	//Sets fps controller to current index thing
	private void ChangeFPSController(){
		//Enable new fps controller
		//

		GameObject newGuy;
		int playerSpawn;

		//Get the new fps controller's GO
		newGuy = FPSControllers[WorldOrder[m_currFPSIndex]];

		//Get the chosen spawn index
		playerSpawn = ChosenSpawns[WorldOrder[m_currFPSIndex]].PlayerSpawnIndex;

		//Move him to the correct spawn, with correct rot too
		newGuy.transform.position = PlayerSpawns[WorldOrder[m_currFPSIndex], playerSpawn].transform.position;
		newGuy.transform.rotation = PlayerSpawns[WorldOrder[m_currFPSIndex], playerSpawn].transform.rotation;

		//Enable him
		newGuy.SetActive(true);
	}

	private void DisableCurrentObj(){
		//Get the current obj spawn index and disable it
		int objSpawn = ChosenSpawns[WorldOrder[m_currFPSIndex]].ObjSpawnIndex;
		ObjSpawns[WorldOrder[m_currFPSIndex], objSpawn].SetActive(false);
	}

	private void EnableCurrentObj(){
		//Get the chosen spawn index and enable it
		int newObjSpawn = ChosenSpawns[WorldOrder[m_currFPSIndex]].ObjSpawnIndex;
		ObjSpawns[WorldOrder[m_currFPSIndex], newObjSpawn].SetActive(true);
	}

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
		m_currFPSIndex = 0;

		//Setup mapping
		//
		WorldOrder = new int[Worlds.Length];
		Images = new int[Worlds.Length, 4]; //4 is number of spawns points in each world

		//Setup logger
		//
		logger = GetComponent<Logger>();
		if(logger == null){
			Debug.LogError("Couldn't find logger!!");
			Application.Quit();
		}

		//Setup Spawn arrays
		//

		//Player spawns
		PlayerSpawns = new GameObject[7,4];
		ObjSpawns = new GameObject[7,4];
		GameObject[] spawns = SpawnObjects;
		for(int i = 0; i < Worlds.Length; i++){
			Transform pspawns = spawns[i].transform.GetChild(0);
			int j = 0;
			foreach(Transform child in pspawns){
				PlayerSpawns[i, j++] = child.gameObject;
			}

			Transform objspwn = spawns[i].transform.GetChild(1);
			j = 0;
			foreach(Transform c in objspwn){
				ObjSpawns[i, j++] = c.gameObject;
			}
		}

		//Setup state for choosing spawns when world switching
		//
		ChosenSpawns = new WorldState[Worlds.Length];

	}

	void Start(){
		//Disable all but one in scene
		//
		foreach(GameObject go in FPSControllers){
			go.SetActive(false);
		}
		FPSControllers[WorldOrder[m_currFPSIndex]].SetActive(true);

		//Enable a player controller
		ChangeFPSController();

		//Enable a goal point
		EnableCurrentObj();

		//Begin logging
		BeginLogging();
	}

	//If we want to show an image, we do!
	void OnGUI() {
		if(imageEnabled && imageToDraw != null) {
			GUI.DrawTexture(new Rect(Left * Screen.width, Top * Screen.width, Right * Screen.width, Bot * Screen.width),
					imageToDraw, ScaleMode.StretchToFill);
		}
	}

	//
	//Public Methods
	//

	//Cycles to the next world in the list
	public void CycleFPSController(){

		//Disable current fpscontroller
		FPSControllers[WorldOrder[m_currFPSIndex]].SetActive(false);

		//Get the current obj spawn index and disable it
		DisableCurrentObj();

		//
		//Increment index and implement a circular list
		//
		m_currFPSIndex++;
		if(m_currFPSIndex == FPSControllers.Length)
			m_currFPSIndex = 0;

		//Enable new fps controller
		//
		ChangeFPSController();

		//Enable correct obj collider
		//
		EnableCurrentObj();

		//Update logger
		//
		logger.EndTrial();
		BeginLogging();
	}

	//Enables user input
	public void EnableInput(){
		((MonoBehaviour)FPSControllers[WorldOrder[m_currFPSIndex]].
		 GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()).enabled = true;
	}

	//Disables user input
	public void DisableInput(){
		((MonoBehaviour)FPSControllers[WorldOrder[m_currFPSIndex]].
		 GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()).enabled = false;
	}

	//We should start showing the image referenced by
	//spawnNum now!
	bool once = true;
	public void StartShowingImage(int spawnNum){
		imageEnabled = true;
		string filename = "Stimuli/object_" + (Images[WorldOrder[m_currFPSIndex], spawnNum]+1).ToString("000");
		imageToDraw = Resources.Load(filename, typeof(Texture2D)) as Texture2D;
	}

	//We should stop showing whatever image now!
	public void StopShowingImage(){
		imageEnabled = false;
		imageToDraw = null;
	}

}

