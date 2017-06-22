using UnityEngine;
using System.Collections;

//Contains more convienient info about
//spawn style prefabs
public class SpawnInfo : MonoBehaviour{
	public Transform[] PlayerSpawnPoints;
	public Transform[] ObjectSpawnPoints;

	//Setup the spawn points
	void Awake(){
		//Create arrays
		PlayerSpawnPoints = new Transform[4];
		ObjectSpawnPoints = new Transform[4];

		//Fill arrays
		int i = 0;
		foreach(Transform child in transform){
			if(i++ == 0){
				//Player spawns
				int j = 0;
				foreach(Transform pspawn in child)
					PlayerSpawnPoints[j++] = pspawn;
			}else if(i++ == 1){
				//Obj spawns
				int j = 0;
				foreach(Transform ospawn in child)
					ObjectSpawnPoints[j++] = ospawn;
			}
		}
	}
}

