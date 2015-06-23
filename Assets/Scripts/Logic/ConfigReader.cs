using System.Text;
using System;
using System.IO;  
using UnityEngine;
using System.Collections;

public class ConfigReader : MonoBehaviour{

	private static int NumEnvironments = 7;
	private static int NumPlayerSpawns = 4;
	private static int NumObjSpawns = 4;
	private static int NumImages = NumEnvironments * NumObjSpawns;

	public string TestConfig = "config.txt";

	private FPSChanger fps;
	private Logger logger;

	void Awake() {
		fps = GetComponent<FPSChanger>();
		logger = GetComponent<Logger>();
	}

	void Start() {

		//Initialize our logger
		logger.InitLogger();

		try
		{
			StreamReader reader = new StreamReader(TestConfig, Encoding.Default);

			using (reader)
			{
				string line;
				string[] entries;

				//Read run number
				line = reader.ReadLine();
				logger.RunNumber = int.Parse(line);

				//Read subject name
				line = reader.ReadLine();
				logger.SubjectName = line;

				//Read collision sphere size
				line = reader.ReadLine();
				float size = float.Parse(line);
				foreach(SphereCollider sc in
						Resources.FindObjectsOfTypeAll(typeof(SphereCollider)) as SphereCollider[]){
					sc.radius = size;
				}

				//Read Movement Speed
				line = reader.ReadLine();
				float speed = float.Parse(line);
				foreach(UnityStandardAssets.Characters.FirstPerson.FirstPersonController  fpsc in
						Resources.FindObjectsOfTypeAll(typeof(UnityStandardAssets.Characters.FirstPerson.FirstPersonController))
						as UnityStandardAssets.Characters.FirstPerson.FirstPersonController[]){
					fpsc.SetMovementSpeed(speed);
				}

				//Are we in learning, testing, or free roam mode?
				line = reader.ReadLine();
				bool isLearning;
				switch(int.Parse(line)){
					case 0:
						fps.CurrentGameState = FPSChanger.GameStates.Learning;
						isLearning = true;
						break;
					case 1:
						fps.CurrentGameState = FPSChanger.GameStates.Learning;
						fps.FreeRoam = true;
						isLearning = true;
						break;
					default:
						fps.CurrentGameState = FPSChanger.GameStates.Testing;
						isLearning = false;
						break;
				}


				//Read global time limit
				line = reader.ReadLine();
				fps.LearningTimeLimit = float.Parse(line);
				fps.TestingTimeLimit = float.Parse(line);

				//Read no input time
				line = reader.ReadLine();
				fps.NoInputTime = float.Parse(line);
				fps.PreImageShowTime = fps.NoInputTime;

				//
				//Env->Spawn->image mapping
				//

				//Get entires
				line = reader.ReadLine();
				entries = line.Split(' ');

				//Validate entries
				if (entries.Length != NumImages) {
					Application.Quit();
				}

				//Setup mapping of envs to image
				//

				//Envs
				for(int j = 0; j < entries.Length; j++) {
					int env = j / NumPlayerSpawns;
					int spawn  = j % NumPlayerSpawns;
					int img = int.Parse(entries[j]);
					fps.Images[env, spawn] = img;
				}


				//
				//Configure testing/learning phase environments
				//

				ConfigureEnvironment(isLearning, reader);

				//
				//Done reading, close the reader
				//
				reader.Close();
			}
		}

		catch (Exception e)
		{
			Debug.LogError("Error reading file!!");
			Debug.LogError(e);
			Application.Quit();
		}
	}

	private void ConfigureEnvironment(bool isLearning, StreamReader reader){
		int i, end;
		i = isLearning ? 0 : 3;
		end = i + 3;

		string line;
		string[] entries;

		//
		//Read environment configuration
		//
		for(; i < end; i++){
			//Read a line in
			line = reader.ReadLine();

			//No lines???
			if (line == null) {
				Debug.LogError("Unable to read line " + i + " of config file... Out of lines!");
				Application.Quit();
			}

			switch(i){
					//
					//Learning phase setup
					//

				case 0:
					//
					//Line 2: Environment order
					//

					//Get entires
					entries = line.Split(' ');

					//Setup order of the environment
					for(int j = 0; j < entries.Length; j++) {
						int worldIndex = int.Parse(entries[j]);
						fps.LearningWorldOrder.Add(worldIndex);
					}

					break;
				case 1:
					//
					//Line 3: For each entry, which player spawn to use
					//

					//Get entires
					entries = line.Split(' ');

					//Validate entries
					if (entries.Length != fps.LearningWorldOrder.Count) {
						Debug.LogError("Bad input script length at line " + (i + 1));
						Application.Quit();
					}

					for(int j = 0; j < entries.Length; j++) {
						WorldState ws = new WorldState();
						ws.PlayerSpawnIndex = int.Parse(entries[j]);
						fps.LearningSpawns.Add(ws);
					}

					break;
				case 2:
					//
					//Line 4: For each env, which obj spawn to use
					//


					//Get entires
					entries = line.Split(' ');

					//Validate entries
					if (entries.Length != fps.LearningWorldOrder.Count) {
						Debug.LogError("Bad input script length at line " + (i + 1));
						Application.Quit();
					}

					for(int j = 0; j < entries.Length; j++) {
						//Note we modify the existing List because we already
						//populated it
						fps.LearningSpawns[j].ObjSpawnIndex = int.Parse(entries[j]);
					}
					break;

					//
					//Testing phase setup
					//

				case 3:
					//
					//Line 5: Testing environment order
					//

					//Get entires
					entries = line.Split(' ');

					//Setup order of the environment
					for(int j = 0; j < entries.Length; j++) {
						int worldIndex = int.Parse(entries[j]);
						fps.TestingWorldOrder.Add(worldIndex);
					}
					
					break;
				case 4:
					//
					//Line 6: For each entry, which player spawn to use
					//

					//Get entires
					entries = line.Split(' ');

					//Validate entries
					if (entries.Length != fps.TestingWorldOrder.Count) {
						Debug.LogError("Bad input script length at line " + (i + 1));
						Application.Quit();
					}

					for(int j = 0; j < entries.Length; j++) {
						WorldState ws = new WorldState();
						ws.PlayerSpawnIndex = int.Parse(entries[j]);
						fps.TestingSpawns.Add(ws);
					}

					break;
				case 5:
					//
					//Line 7: For each env, which obj spawn to use
					//

					//Get entires
					entries = line.Split(' ');

					//Validate entries
					if (entries.Length != fps.TestingWorldOrder.Count) {
						Debug.LogError("Bad input script length at line " + (i + 1));
						Application.Quit();
					}

					for(int j = 0; j < entries.Length; j++) {
						//Note we modify the existing List because we already
						//populated it
						fps.TestingSpawns[j].ObjSpawnIndex = int.Parse(entries[j]);
					}

					break;
			}
		}

	}

}

