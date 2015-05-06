using System.Text;
using System;
using System.IO;  
using UnityEngine;
using System.Collections;

//Contains more convienient info about
//spawn style prefabs
public class ConfigReader : MonoBehaviour{

	public string FileToRead;

	private FPSChanger fps;

	void Awake(){
		fps = GetComponent<FPSChanger>();
	}

	void Start(){
		Load(FileToRead);

		// Handle any problems that might arise when reading the text
		try
		{
			string line;
			string[] entries;

			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader theReader = new StreamReader(FileToRead, Encoding.Default);

			// Immediately clean up the reader after this block of code is done.
			// You generally use the "using" statement for potentially memory-intensive objects
			// instead of relying on garbage collection.
			// (Do not confuse this with the using directive for namespace at the 
			// beginning of a class!)
			using (theReader)
			{
				line = theReader.ReadLine();

				if (line == null){
					Debug.LogError("Error reading file!!");
					return false;
				}

				entries = line.Split(' ');
				if (entries.Length == 7){
					//Setup order of the environments
					int j = 0;
					foreach(string s in entries){
						fps.WorldOrder[j++] = int.Parse(s);
					}
				}

				line = theReader.ReadLine();

				if (line == null){
					Debug.LogError("Error reading file!!");
					return false;
				}

				entries = line.Split(' ');
				if (entries.Length == 7){
					//Setup mapping of envs to image
					int j = 0;
					foreach(string s in entries){
						fps.Images[j++] = int.Parse(s);
					}
				}

				// Done reading, close the reader
				theReader.Close();
				return true;
			}
		}

		// If anything broke in the try block, we throw an exception with information
		// on what didn't work
		catch (Exception e)
		{
			Debug.LogError("Error reading file!!");
			Debug.LogError(e);
			return false;
		}
	}

}

