using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

//Holds the data that should be serialized to the ouput stream
//
//TODO
//	would this make the kind of xml output I want?
//	how are multiple players handled? only one is enabled makes it easier, probably
 
public class PlayerDataContainer{
	//Attributes are automatic
	//

	//Position of player
	public Vector3 position;
	//Rotation of player
	public Vector3 rotation; //use quaternion instead? no: its not human readable
	//Timestamp
	public float time;
}

