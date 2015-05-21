using System.Text;
using System;
using System.IO;  
using UnityEngine;
using System.Collections;

//Contains more convienient info about
//spawn style prefabs
public class ConfigReader : MonoBehaviour{

	private static int NumEnvironments = 7;
	private static int NumPlayerSpawns = 4;
	private static int NumObjSpawns = 4;
	private static int NumImages = NumEnvironments * NumObjSpawns;
	private static int linesToRead = 7;

	public string FileToRead = "config.txt";

	private FPSChanger fps;

	void Awake() {
		fps = GetComponent<FPSChanger>();
	}

	void Start() {

		//Handle any problems that might arise when reading the text
		try
		{
			StreamReader reader = new StreamReader(FileToRead, Encoding.Default);

			using (reader)
			{
				for(int i = 0; i < linesToRead; i++){
					string line;
					string[] entries;

					//Read a line in
					line = reader.ReadLine();

					//No lines???
					if (line == null) {
						Debug.LogError("Unable to read line " + i + " of config file... Out of lines!");
						Application.Quit();
					}

					switch(i){
						case 0:
							//
							//Line 1: Env->Spawn->image mapping
							//

							//Get entires
							entries = line.Split(' ');

							//Validate entries
							if (entries.Length != NumImages) {
								Debug.LogError("Bad input script length at line " + (i + 1));
								Application.Quit();
							}

							//Setup mapping of envs to image
							//

							//Envs
							for(int j = 0; j < entries.Length; j++) {
								int env = j / NumPlayerSpawns;
								int spawn  = j % NumPlayerSpawns;
								int img = int.Parse(entries[j]);
								//print(env + ", " + spawn + ", " + img);
								fps.Images[env, spawn] = img;
							}

							break;

							//
							//Learning phase setup
							//

						case 1:
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
						case 2:
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
						case 3:
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

						case 4:
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
						case 5:
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
						case 6:
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

				//Done reading, close the reader
				//
				reader.Close();
				Application.Quit();
			}
		}

		//If anything else broke in the try block, we throw an exception with information
		//on what didn't work
		catch (Exception e)
		{
			Debug.LogError("Error reading file!!");
			Debug.LogError(e);
			Application.Quit();
		}
	}

}

