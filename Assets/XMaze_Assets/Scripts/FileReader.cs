using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileReader : MonoBehaviour
{

    public GameObject player;
    public Demon demon;
    public FileWriter writer;

    public GameObject[] sprites;

    private int mode;
    private int direction;

    private string modeStr;
    private string partStr;
    private string runStr;
    private string speedStr;
    private string fovStr;
    private string radiusStr;
    private string visibleStr;
    private string selectStr;
    private string returnStr;
    private string totalStr;
    private string startStr;

    private string contextStr;
    private string holdStr;
    private string leftObjStr;
    private string leftRewStr;
    private string rightObjStr;
    private string rightRewStr;

    private string[] contextArr;
    private string[] holdArr;
    private string[] leftObjArr;
    private string[] leftRewArr;
    private string[] rightObjArr;
    private string[] rightRewArr;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        writer = GameObject.Find("FileWriter").GetComponent<FileWriter>();

        try
        {
            StreamReader reader = new StreamReader("Config.txt");

            modeStr = reader.ReadLine();
            partStr = reader.ReadLine();
            runStr = reader.ReadLine();
            speedStr = reader.ReadLine();
            fovStr = reader.ReadLine();
            radiusStr = reader.ReadLine();
            visibleStr = reader.ReadLine();
            selectStr = reader.ReadLine();
            returnStr = reader.ReadLine();
            totalStr = reader.ReadLine();
            startStr = reader.ReadLine();

            contextStr = reader.ReadLine();
            holdStr = reader.ReadLine();
            leftObjStr = reader.ReadLine();
            leftRewStr = reader.ReadLine();
            rightObjStr = reader.ReadLine();
            rightRewStr = reader.ReadLine();

            reader.Close();
        }
        catch(Exception e)
        {
			Debug.LogError("Error reading file!!");
			Debug.LogError(e);
			Application.Quit();
		}

        try
        {
            mode = int.Parse(modeStr);
            writer.mode = mode;
            writer.partCode = partStr;
            writer.runNum = int.Parse(runStr);
            direction = int.Parse(startStr);
            writer.direction = direction;
        }
        catch(Exception e)
        {
			Debug.LogError("Error parsing file!!");
			Debug.LogError(e);
			Application.Quit();
		}

        DontDestroyOnLoad(gameObject);
    }

    public void XMazeInit()
    {
        player = GameObject.FindWithTag("Player");
        demon = player.GetComponent<Demon>();
        sprites = GameObject.FindGameObjectsWithTag("Sphere");

        try
        {
            demon.mode = mode;
            player.GetComponent<SimpleMovement>().moveSpeed = float.Parse(speedStr);

            foreach(Camera c in player.GetComponentsInChildren<Camera>())
            {
                c.fieldOfView = float.Parse(fovStr);
            }

            foreach(GameObject sprite in sprites)
            {
                sprite.GetComponent<SphereCollider>().radius = float.Parse(radiusStr) / sprite.transform.localScale.x;
            }

            demon.visibleTime = float.Parse(visibleStr);
            demon.selectTime = float.Parse(selectStr);
            demon.returnTime = float.Parse(returnStr);
            demon.totalTime = float.Parse(totalStr);

            demon.direction = direction;
            if(int.Parse(startStr) == 1)
            {
                player.transform.position = new Vector3(demon.westXPos, demon.yPos, demon.zPos);
                player.transform.Rotate(0f, 0f, 0f);
            }
            else if(int.Parse(startStr) == 2)
            {
                player.transform.position = new Vector3(demon.eastXPos, demon.yPos, demon.zPos);
                player.transform.Rotate(0f, 180f, 0f);
            }

            //Debug.Log("Halfway done parsing!");

            contextArr = contextStr.Split(' ');
            holdArr = holdStr.Split(' ');
            leftObjArr = leftObjStr.Split(' ');
            leftRewArr = leftRewStr.Split(' ');
            rightObjArr = rightObjStr.Split(' ');
            rightRewArr = rightRewStr.Split(' ');

            demon.contexts = new int[contextArr.Length];
            demon.holds = new float[holdArr.Length];
            demon.leftObjects = new int[leftObjArr.Length];
            demon.leftRewards = new int[leftRewArr.Length];
            demon.rightObjects = new int[rightObjArr.Length];
            demon.rightRewards = new int[rightRewArr.Length];

            for(int i = 0; i < contextArr.Length; i++)
            {
                demon.contexts[i] = int.Parse(contextArr[i]);
            }
            for(int i = 0; i < holdArr.Length; i++)
            {
                demon.holds[i] = float.Parse(holdArr[i]);
            }
            for(int i = 0; i < leftObjArr.Length; i++)
            {
                demon.leftObjects[i] = int.Parse(leftObjArr[i]);
            }
            for(int i = 0; i < leftRewArr.Length; i++)
            {
                demon.leftRewards[i] = int.Parse(leftRewArr[i]);
            }
            for(int i = 0; i < rightObjArr.Length; i++)
            {
                demon.rightObjects[i] = int.Parse(rightObjArr[i]);
            }
            for(int i = 0; i < rightRewArr.Length; i++)
            {
                demon.rightRewards[i] = int.Parse(rightRewArr[i]);
            }
        }
        catch(Exception e)
        {
			Debug.LogError("Error parsing file!!");
			Debug.LogError(e);
			Application.Quit();
		}
    }

}
