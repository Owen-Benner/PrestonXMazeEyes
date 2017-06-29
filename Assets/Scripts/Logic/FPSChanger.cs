using UnityEngine;
using System.Collections;
using System.Collections.Generic;//for List<T>

//Responsible for giving player control to a fps prefab
public class FPSChanger : MonoBehaviour{

	public float NoInputTime;
	public bool FreeRoam = false;

	//Are we in learning state or testing state?
	//
	public enum GameStates {Learning, Testing};
	public GameStates CurrentGameState;

	//Quickly check if in learning phase or not
	public bool isLearning{
		get{ return CurrentGameState == GameStates.Learning; }
	}

	//Parralel lists of our FPSControllers and Enviroments
	//
	//Setup in Inspector
	//
	public GameObject[] Worlds;
	public GameObject[] FPSControllers;

	//Player Spawns
	public GameObject[,] PlayerSpawns {get;set;}
	//Object spawns
	public GameObject[,] ObjSpawns {get;set;}
	//GO that holds the player and obj spawns
	public GameObject[] SpawnObjects;

	//
	//Phase goals and spawns
	//

	//Which world will we deal with next?
	//Index into this list is kept by m_currFPSIndex and updates every obj contact
	public List<int> LearningWorldOrder {get;set;}
	public List<int> TestingWorldOrder {get;set;}

	//For a given entry, which Player and Object spawns are we using?
	public List<WorldState> LearningSpawns {get;set;}
	public List<WorldState> TestingSpawns {get;set;}

	//Timer for max time player has to find obj during test phase
	public float LearningTimeLimit = 60.0f;
	public float TestingTimeLimit = 20.0f;
	private Timer testTimer;
	public float PreImageShowTime = 3.0f;
	private Timer imageTimer;

	//
	//Which <world, pspawn, objspawn> vector is relevant to us?
	//

	//Index of current active FPSController go
	private int m_currFPSIndex;

	private SquareManager sync;
	private bool wasWhite = false;
	public bool isWhite = false;
	private bool doLog = false;
	
	//
	//Images
	//

	//Image assets, per world, for objs
	//Use this like Images[world, spawn]
	//This should give back which img file to use
	public int[,] Images {get;set;}

	//Are we showing the on screen image right now?
	private bool imageEnabled = false;
	private bool preImageEnabled = false;

	//The image to be displayed on screen
	private Texture imageToDraw;

	//Inspector configurable image overlay borders; percentage of the screen in [0, 1]
	public float Left, Right, Top, Bot;

	//
	//Private data
	//

	//Ref to our logger
	private Logger logger;
	private SoundLogger syncLogger;
	private SoundLogger frameLogger;
	private readonly string syncFileName = "soundoutput.xml";
	private readonly string frameFileName = "frames.xml";
	
	//
	//Helper Methods
	//


	//Begin a trial
	//
	private void BeginLogging(){
		List<int> worldMapping;
		List<WorldState> spawnMapping;
        GameObject cur_dest;
		int playerSpawn;

		spawnMapping = isLearning ? LearningSpawns : TestingSpawns;
		worldMapping = isLearning ? LearningWorldOrder : TestingWorldOrder;

		//Get GOs logger wants
		GameObject cur_fps = FPSControllers[worldMapping[m_currFPSIndex]];
		GameObject cur_world = Worlds[worldMapping[m_currFPSIndex]];
		
		//Get the chosen spawn index
		playerSpawn = spawnMapping[m_currFPSIndex].PlayerSpawnIndex;

		int objSpawn = spawnMapping[m_currFPSIndex].ObjSpawnIndex;
		cur_dest = ObjSpawns[worldMapping[m_currFPSIndex], objSpawn];

		logger.StartTrial(cur_dest.transform.position, cur_fps, cur_world.transform.position);
	}

	private void GroundTransform(Transform t, float offset){
		RaycastHit hit;
		if(Physics.Raycast(t.position, -Vector3.up, out hit)){
			Vector3 temp = t.position;
			temp.y -= hit.distance - t.GetComponent<Collider>().bounds.extents.y + offset;

			t.position = temp;
		}
	}

	//Sets fps controller to current index thing
	private void SetControlledFPS(){
		//Enable new fps controller
		//

		List<int> worldMapping;
		List<WorldState> spawnMapping;
		GameObject newGuy;
		int playerSpawn;

		worldMapping = isLearning ? LearningWorldOrder : TestingWorldOrder;
		spawnMapping = isLearning ? LearningSpawns : TestingSpawns;

		//Get the chosen spawn index
		playerSpawn = spawnMapping[m_currFPSIndex].PlayerSpawnIndex;
		Transform spawnTransform = PlayerSpawns[worldMapping[m_currFPSIndex], playerSpawn].transform;

		//Get the new fps controller's GO
		newGuy = FPSControllers[worldMapping[m_currFPSIndex]];

		//Move him to the correct spawn, with correct rot too
		newGuy.transform.position = spawnTransform.position;
		newGuy.transform.rotation = spawnTransform.rotation;

		//Enable him
		newGuy.SetActive(true);
		
		//Put him on the ground
		GroundTransform(newGuy.transform, 0.0f);
	}

	//Learning
	private void SetCurrentObj(bool active){
		List<int> worldMapping;
		List<WorldState> spawnMapping;

		worldMapping = CurrentGameState == GameStates.Learning ? LearningWorldOrder : TestingWorldOrder;
		spawnMapping = CurrentGameState == GameStates.Learning ? LearningSpawns : TestingSpawns;

		int objSpawn = spawnMapping[m_currFPSIndex].ObjSpawnIndex;
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

	//Testing
	private void EnableAllCurrentObj(){
		foreach(GameObject obj in ObjSpawns){
			obj.SetActive(true);
		}
	}

	private void PreImageViewing(){
		//Show image
		//
		int spawnNum = isLearning ? LearningSpawns[m_currFPSIndex].ObjSpawnIndex :
			TestingSpawns[m_currFPSIndex].ObjSpawnIndex;
		preImageEnabled = true;
		StartShowingImage(spawnNum);

		//Set image timer
		imageTimer.SetTimer(PreImageShowTime);

		//Disable input during this time
		DisableInput();
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
		PlayerSpawns = new GameObject[8,4];
		ObjSpawns = new GameObject[8,4];
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

		//Setup timers
		//

		imageTimer = new Timer();
		testTimer = new Timer();

		//Sync squares

		sync = GameObject.FindWithTag("Logic").GetComponent<SquareManager>();

	}

	void Start(){
		//Disable all but one in scene
		//
		foreach(GameObject go in FPSControllers){
			go.SetActive(false);
		}
		if(isLearning)
			FPSControllers[LearningWorldOrder[m_currFPSIndex]].SetActive(true);
		else
			FPSControllers[TestingWorldOrder[m_currFPSIndex]].SetActive(true);

		//Enable a player controller
		SetControlledFPS();
		syncLogger = new SoundLogger (syncFileName);
		frameLogger = new SoundLogger (frameFileName);
		if(!FreeRoam){
			//Enable a goal point
			if(isLearning)
				EnableCurrentObj();
			else
				EnableAllCurrentObj();

			//Include test phase into logs
			//
			string phaseString = isLearning ? "learning" : "testing";
			logger.StartPhase(phaseString);

			//Show them the goal image
			PreImageViewing();

			//Begin logging
			BeginLogging();
		}
	}

	void Update(){
		if (doLog) {
			//syncLogger.Log (Time.realtimeSinceStartup.ToString ());
			syncLogger.Log (Time.time.ToString ());
			doLog = false;
		}
		frameLogger.Log (Time.time.ToString ());
		if ((wasWhite && !isWhite) || (!wasWhite && isWhite)) {
			wasWhite = !wasWhite;
			doLog = true; // log frame start time at start of next frame; assuming this is relatively close to display time
		}

		//If in testing phase, timeout after a while, also show images before trials
		if(preImageEnabled){
			if(imageTimer.isDone){
				//Start new timer
				//
				float time = isLearning ? LearningTimeLimit : TestingTimeLimit;
				testTimer.SetTimer(time);

				//Stop showing image
				imageEnabled = false;
				preImageEnabled = false;
				imageToDraw = null;

				//Enable player input again
				EnableInput();
			}
		}else{
			if(testTimer.isDone){
				//Move to next trial on timeout
				CycleFPSController();
			}
		}
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

	//Figures out which index a given Object Spawn is
	public int WhatIsIndex(GameObject obj){
		List<int> worldMapping;
		List<WorldState> spawnMapping;

		worldMapping = CurrentGameState == GameStates.Learning ? LearningWorldOrder : TestingWorldOrder;
		spawnMapping = CurrentGameState == GameStates.Learning ? LearningSpawns : TestingSpawns;

		//4 is num obj spawns lol
		for(int i = 0; i < 4; i++){
			if(ObjSpawns[worldMapping[m_currFPSIndex], i] == obj)
				return i;
		}
		return -1;
	}


	//Cycles to the next world in the list
	public void CycleFPSController(int index = -1){

		//Stop current trial
		logger.EndTrial(index);

		switch(CurrentGameState){
			case GameStates.Learning:
				//Disable current fpscontroller
				FPSControllers[LearningWorldOrder[m_currFPSIndex]].SetActive(false);

				//Get the current obj spawn index and disable it
				DisableCurrentObj();

				//Increment index
				//
				m_currFPSIndex++;
				if(m_currFPSIndex == LearningWorldOrder.Count){
					//Include test phase into logs
                    if(!FreeRoam)
                        logger.EndPhase();

					Application.Quit(); //Note: We stop here
                    print("Game ended idiot");
                    Time.timeScale = 0;

					/*
					logger.StartPhase("testing");

					//Enable all obj colliders
					EnableAllCurrentObj();

					//Reset index
					m_currFPSIndex = 0;
					*/
				}

				//Enable correct obj collider
				//
				if(!FreeRoam)
					EnableCurrentObj();

				break;
			case GameStates.Testing:
				//Disable current fpscontroller
				FPSControllers[TestingWorldOrder[m_currFPSIndex]].SetActive(false);

				//Increment index
				//
				m_currFPSIndex++;
				if(m_currFPSIndex == TestingWorldOrder.Count){
					logger.EndPhase();
					Application.Quit(); //Note: We stop here
				}

				break;
		}

		//Enable new fps controller
		//
		SetControlledFPS();

		//Show them the preimage
		PreImageViewing();

		//Start logging new trial
		BeginLogging();
	}

	//Enables user input
	bool once = false;
	public void EnableInput(){
		GameObject cur_fps = CurrentGameState == GameStates.Learning ?
			FPSControllers[LearningWorldOrder[m_currFPSIndex]] : FPSControllers[TestingWorldOrder[m_currFPSIndex]];

		UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsc = (cur_fps.GetComponent<
				UnityStandardAssets.Characters.FirstPerson.FirstPersonController>());

		//Reset the rotation ourselves because the mouselook script on the FPSC overrides it
		if(once){
			fpsc.SetRotation(fpsc.transform.rotation);
		}

		fpsc.enabled = true;
		once = true;
	}

	//Disables user input
	public void DisableInput(){
		GameObject cur_fps = isLearning ?
			FPSControllers[LearningWorldOrder[m_currFPSIndex]] : FPSControllers[TestingWorldOrder[m_currFPSIndex]];

		((MonoBehaviour)cur_fps.GetComponent<
		 UnityStandardAssets.Characters.FirstPerson.FirstPersonController>()).enabled = false;
	}

	//We should start showing the image referenced by
	//spawnNum now!
	public void StartShowingImage(int spawnNum){
		imageEnabled = true;
		List<int> worldMapping = CurrentGameState == GameStates.Learning ? LearningWorldOrder : TestingWorldOrder;
		string filename = "Stimuli/object_" + (Images[worldMapping[m_currFPSIndex], spawnNum]).ToString("000");
		imageToDraw = Resources.Load(filename, typeof(Texture2D)) as Texture2D;
	}

	//We should stop showing whatever image now!
	public void StopShowingImage(){
		imageEnabled = false;
		imageToDraw = null;
	}

	void OnDestroy(){
  	    syncLogger.Dispose ();
		frameLogger.Dispose ();
	}

}

