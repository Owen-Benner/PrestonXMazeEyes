using System.Text;
using System;
using System.IO;  
using UnityEngine;
using System.Collections;

//Contains more convienient info about
//spawn style prefabs
public class ConfigReader : MonoBehaviour{

	public static int NumEnvironments = 7;
	public static int NumPlayerSpawns = 4;
	public static int NumObjSpawns = 4;
	public static int NumImages = NumEnvironments * NumObjSpawns;

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
			int linesToRead = 2;

			using (reader)
			{
				for(int i = 0; i < linesToRead; i++){
					string line;
					string[] entries;

					//Read a line in
					line = reader.ReadLine();

					//No lines???
					if (line == null) {
						Debug.LogError("Unable to read line " + i + " of config file.");
						return;
					}

					switch(i){
						case 0:
							//
							//Line 1: Environment order
							//

							//Get entires
							entries = line.Split(' ');

							//Validate entries
							if (entries.Length != NumEnvironments) {
								Debug.LogError("Bad input script length at line " + (i + 1));
								return;
							}

							//Split into input ints
							entries = line.Split(' ');
							//Setup order of the environment
							for(int j = 0; j < entries.Length; j++) {
								fps.WorldOrder[j] = int.Parse(entries[j]);
							}

							break;
						case 1:
							//
							//Line 2: Env->Spawn->image mapping
							//

							//Get entires
							entries = line.Split(' ');

							//Validate entries
							if (entries.Length != NumImages) {
								Debug.LogError("Bad input script length at line " + (i + 1));
								return;
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
						case 2:
							//
							//Line 3: For each env, which player spawn to use
							//

							//Get entires
							entries = line.Split(' ');

							//Validate entries
							if (entries.Length != NumEnvironments) {
								Debug.LogError("Bad input script length at line " + (i + 1));
								return;
							}

							for(int j = 0; j < entries.Length; j++) {
								fps.ChosenSpawns[j].PlayerSpawnIndex = int.Parse(entries[j]);
							}

							break;
						case 3:
							//
							//Line 4: For each env, which obj spawn to use
							//


							//Get entires
							entries = line.Split(' ');

							//Validate entries
							if (entries.Length != NumEnvironments) {
								Debug.LogError("Bad input script length at line " + (i + 1));
								return;
							}

							for(int j = 0; j < entries.Length; j++) {
								fps.ChosenSpawns[j].ObjSpawnIndex = int.Parse(entries[j]);
							}
							break;
					}
				}

				//Done reading, close the reader
				//
				reader.Close();
				return;
			}
		}

		//If anything else broke in the try block, we throw an exception with information
		//on what didn't work
		catch (Exception e)
		{
			Debug.LogError("Error reading file!!");
			Debug.LogError(e);
			return;
		}
	}

}

