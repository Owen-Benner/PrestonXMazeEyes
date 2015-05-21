using UnityEngine;
using System.Collections;
using System.Collections.Generic;//for List<T>

//Responsible for giving player control to a fps prefab
public class FPSChanger : MonoBehaviour{

	//Are we in learning state or testing state?
	//
	public enum GameStates {Learning, Testing};
	public GameStates CurrentGameState = GameStates.Learning;

	//Parralel lists of our FPSControllers and Enviroments
	//
	//Setup in Inspector
	//
	public GameObject[] Worlds;
	public GameObject[] FPSControllers;

	//Player Spawns
	public GameObject[,] PlayerSpawns;
	//Object spawns
	public GameObject[,] ObjSpawns;
	//GO that holds the player and obj spawns
	public GameObject[] SpawnObjects;

	//
	//Phase goals and spawns
	//

	//Which world will we deal with next?
	//Index into this list is kept by m_currFPSIndex and updates every obj contact
	public List<int> LearningWorldOrder;
	public List<int> TestingWorldOrder;

	//For a given entry, which Player and Object spawns are we using?
	public List<WorldState> LearningSpawns;
	public List<WorldState> TestingSpawns;

	//
	//Which <world, pspawn, objspawn> vector is relevant to us?
	//

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
		GameObject cur_fps = CurrentGameState == GameStates.Learning ?
			FPSControllers[LearningWorldOrder[m_currFPSIndex]] : FPSControllers[TestingWorldOrder[m_currFPSIndex]];

		GameObject cur_world = CurrentGameState == GameStates.Learning ?
			Worlds[LearningWorldOrder[m_currFPSIndex]] : Worlds[TestingWorldOrder[m_currFPSIndex]];

		
		//Get the chosen spawn index
		int playerSpawn = LearningSpawns[LearningWorldOrder[m_currFPSIndex]].PlayerSpawnIndex;

		GameObject cur_dest = PlayerSpawns[LearningWorldOrder[m_currFPSIndex], playerSpawn];

		logger.StartTrial("FPV", cur_dest.transform.position, "--", cur_fps, cur_world.transform.position);
	}

	//Sets fps controller to current index thing
	//TODO
	private void SetControlledFPS(){
		//Enable new fps controller
		//

		List<int> worldMapping;
		List<WorldState> spawnMapping;
		GameObject newGuy;
		int playerSpawn;

		worldMapping = CurrentGameState == GameStates.Learning ? LearningWorldOrder : TestingWorldOrder;
		spawnMapping = CurrentGameState == GameStates.Learning ? LearningSpawns : TestingSpawns;

		//Get the new fps controller's GO
		newGuy = FPSControllers[worldMapping[m_currFPSIndex]];

		//Get the chosen spawn index
		playerSpawn = spawnMapping[worldMapping[m_currFPSIndex]].PlayerSpawnIndex;

		//Move him to the correct spawn, with correct rot too
		newGuy.transform.position = PlayerSpawns[worldMapping[m_currFPSIndex], playerSpawn].transform.position;
		newGuy.transform.rotation = PlayerSpawns[worldMapping[m_currFPSIndex], playerSpawn].transform.rotation;

		//Enable him
		newGuy.SetActive(true);
	}

	private void SetCurrentObj(bool active){
		List<int> worldMapping;
		List<WorldState> spawnMapping;

		worldMapping = CurrentGameState == GameStates.Learning ? LearningWorldOrder : TestingWorldOrder;
		spawnMapping = CurrentGameState == GameStates.Learning ? LearningSpawns : TestingSpawns;

		int objSpawn = spawnMapping[worldMapping[m_currFPSIndex]].ObjSpawnIndex;
		ObjSpawns[worldMapping[m_currFPSIndex], objSpawn].SetActive(active);
	}

	//Get the current obj spawn index and enable it
	private void EnableCurrentObj(){
		SetCurrentObj(true);
	}

	//Get the current obj spawn index and disable it
	private void DisableCurrentObj(){
		SetCurrentObj(false);
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

		//Image file -> Obj Spawn Point
		Images = new int[Worlds.Length, 4]; //4 is number of spawns points in each world

		//Setup list
		//
		m_currFPSIndex = 0;

		//Setup mappings
		//
		LearningWorldOrder = new List<int>();
		TestingWorldOrder = new List<int>();

		//State for choosing spawns when world switching
		LearningSpawns = new List<WorldState>();
		TestingSpawns = new List<WorldState>();


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
	}

	void Start(){
		//Disable all but one in scene
		//
		foreach(GameObject go in FPSControllers){
			go.SetActive(false);
		}
		FPSControllers[LearningWorldOrder[m_currFPSIndex]].SetActive(true);

		//Enable a player controller
		SetControlledFPS();

		//Enable a goal point
		EnableCurrentObj();

		//Include test phase into logs
		//
		logger.StartPhase("learning");

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
	//TODO Use the correct arrays for everything depending on learning/testing
	public void CycleFPSController(){

		switch(CurrentGameState){
			case GameStates.Learning:
				//Disable current fpscontroller
				FPSControllers[LearningWorldOrder[m_currFPSIndex]].SetActive(false);

				//Get the current obj spawn index and disable it
				DisableCurrentObj();

				//
				//Increment index and implement a circular list
				//
				m_currFPSIndex++;
				if(m_currFPSIndex == FPSControllers.Length){
					//Finishing the list is the condition to move to testing phase
					CurrentGameState = GameStates.Testing;
					//Include test phase into logs
					logger.StartPhase("testing");
					m_currFPSIndex = 0;
				}
				break;
			case GameStates.Testing:
				//Disable current fpscontroller
				FPSControllers[TestingWorldOrder[m_currFPSIndex]].SetActive(false);

				//Get the current obj spawn index and disable it
				DisableCurrentObj();

				//
				//Increment index and implement a circular list
				//
				m_currFPSIndex++;
				if(m_currFPSIndex == FPSControllers.Length){
					logger.EndTrial();
					Application.Quit();
				}
				break;
		}

		//Enable new fps controller
		//
		SetControlledFPS();

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
		GameObject cur_fps = CurrentGameState == GameStates.Learning ?
			FPSControllers[LearningWorldOrder[m_currFPSIndex]] : FPSControllers[TestingWorldOrder[m_currFPSIndex]];

		((MonoBehaviour)cur_fps.GetComponent<
		 UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()).enabled = true;
	}

	//Disables user input
	public void DisableInput(){
		GameObject cur_fps = CurrentGameState == GameStates.Learning ?
			FPSControllers[LearningWorldOrder[m_currFPSIndex]] : FPSControllers[TestingWorldOrder[m_currFPSIndex]];

		((MonoBehaviour)cur_fps.GetComponent<
		 UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()).enabled = false;
	}

	//We should start showing the image referenced by
	//spawnNum now!
	public void StartShowingImage(int spawnNum){
		//Only draw image during learning phase
		if(CurrentGameState == GameStates.Learning){
			imageEnabled = true;
			string filename = "Stimuli/object_" + (Images[LearningWorldOrder[m_currFPSIndex], spawnNum]+1).ToString("000");
			imageToDraw = Resources.Load(filename, typeof(Texture2D)) as Texture2D;
		}
	}

	//We should stop showing whatever image now!
	public void StopShowingImage(){
		imageEnabled = false;
		imageToDraw = null;
	}

}

