using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Tobii.Research;

public class FileWriter : MonoBehaviour
{

    private GameObject player;
    private Transform playPos;
    private Demon demon;

    private string spc = " ";

    public int frameFreq = 10;

    public string format0 = "%Frame frameNum: distHoriz distDiag pose time xPos"
        + " zPos leftEyeX leftEyeY leftPupil rightEyeX rightEyeY rightPupil";
    public string format1 = "%Selection trialNum: chamber reward score";
    public string format2 = "%Segment: distHoriz distDiag pose time xPos zPos"
        + " trialNum trialTime newSegment";
    public string partCode;

    private string fileName;
    public int runNum;
    public int mode;
    public int direction;

    private float framePer;

    private int frame = 0;
    private float lastFrame;
    private float trialStart;

    StreamReader lastRunReader;
    StreamWriter writer;

    IEyeTracker eyeTracker;

    private bool write = false;
    private bool XMazeLoaded = false;

    private float startTime;

    private static GazeDataEventArgs gaze;

    private void WriteGaze()
    {
        GazePoint point = gaze.LeftEye.GazePoint;
        if(point.Validity == Validity.Valid)
        {
            writer.Write(spc + string.Format("{0:N3}",
                + point.PositionOnDisplayArea.X) + spc + string.Format("{0:N3}",
                point.PositionOnDisplayArea.Y));
        }
        else
        {
            writer.Write(spc + "invalid" + spc + "invalid");
        }

        PupilData pupil = gaze.LeftEye.Pupil;
        if(pupil.Validity == Validity.Valid)
        {
            writer.Write(spc + string.Format("{0:N3}", pupil.PupilDiameter));
        }
        else
        {
            writer.Write(spc + "invalid");
        }

        point = gaze.RightEye.GazePoint;
        if (point.Validity == Validity.Valid)
        {
            writer.Write(spc + string.Format("{0:N3}",
                + point.PositionOnDisplayArea.X) + spc + string.Format("{0:N3}",
                point.PositionOnDisplayArea.Y));
        }
        else
        {
            writer.Write(spc + "invalid" + spc + "invalid");
        }

        pupil = gaze.RightEye.Pupil;
        if (pupil.Validity == Validity.Valid)
        {
            writer.WriteLine(spc + string.Format("{0:N3}",
                pupil.PupilDiameter));
        }
        else
        {
            writer.WriteLine(spc + "invalid");
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        try
        {
            eyeTracker = EyeTrackingOperations.FindAllEyeTrackers()[0];
            eyeTracker.GazeDataReceived += EyeTracker_GazeDataReceived;
        }
        catch
        {
            Debug.LogError("Eye tracker not found!");
        }

        fileName = partCode + "_";
        if (mode == 1)
        {
            fileName += "practice";
        }
        else
        {
            fileName += "task";
        }
        fileName += "_run" + runNum + ".xml";

        try
        {
            lastRunReader = new StreamReader(fileName);
            Debug.LogError("Repeated log filename! Update Config.txt or remove"
                + " last log file from directory.");
            Application.Quit();
            return;
        }
        catch{}

        writer = new StreamWriter(fileName);
        framePer = 1f / (float)frameFreq;

        writer.WriteLine(partCode + spc + mode + spc + runNum + spc
            + direction);

        writer.WriteLine(format0);
        writer.WriteLine(format1);
        writer.WriteLine(format2);

        DontDestroyOnLoad(gameObject);
    }

    private void EyeTracker_GazeDataReceived(object sender, GazeDataEventArgs e)
    {
        gaze = e;
        Debug.Log("Recieved gaze data.");
    }

    public void XMazeInit()
    {
        player = GameObject.FindWithTag("Player");
        playPos = player.GetComponent<Transform>();
        demon = player.GetComponent<Demon>();

        XMazeLoaded = true;
        trialStart = Time.time;

        Debug.Log(Time.time - startTime);
    }

    public void StartWriting()
    {
        startTime = Time.time;
        write = true;
        WriteFrame();
        lastFrame = Time.time;
        Debug.Log("Writing!");
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastFrame >= framePer)
        {
            if(write)
            {
                WriteFrame();
            }
            lastFrame += framePer;
        }
    }

    void WriteFrame()
    {
        if(!write)
        {
            return;
        }

        if(XMazeLoaded)
        {
            float distHori;
            float distDiag;

            if (demon.direction == 1)
            {
                distHori = Mathf.Abs(playPos.position.x - demon.eastXPos);
                distDiag = Mathf.Sqrt(distHori * distHori 
                    + Mathf.Pow(playPos.position.z - demon.zPos, 2f));
            }
            else
            {
                distHori = Mathf.Abs(playPos.position.x - demon.westXPos);
                distDiag = Mathf.Sqrt(distHori * distHori
                    + Mathf.Pow(playPos.position.z - demon.zPos, 2f));
            }

            writer.Write("Frame "
                + frame.ToString() + ":" + spc + string.Format("{0:N3}",
                distHori) + spc + string.Format("{0:N3}", distDiag) + spc
                + string.Format("{0:N3}", playPos.eulerAngles.y) + spc
                + string.Format("{0:N3}", Time.time - startTime) + spc
                + string.Format("{0:N3}", playPos.position.x) + spc
                + string.Format("{0:N3}", playPos.position.z));

            // Add gaze data here
            try
            {
                WriteGaze();
            }
            catch
            {
                writer.WriteLine(spc + "Error");
            }
        }
        else
        {
            writer.Write("Frame "
                + frame.ToString() + ":" + spc + string.Format("{0:N3}", 0f)
                + spc + string.Format("{0:N3}", 0f) + spc
                + string.Format("{0:N3}", 0f) + spc + string.Format("{0:N3}",
                Time.time - startTime) + spc + string.Format("{0:N3}", 0f)
                + spc + string.Format("{0:N3}", 0f));

            // Add gaze data here
            try
            {
                WriteGaze();
            }
            catch
            {
                writer.WriteLine(spc + "Error writing gaze data!");
            }
        }

        frame++;
    }

    public void WriteSegment()
    {
        float distHori;
        float distDiag;

        if(demon.direction == 1)
        {
            distHori = Mathf.Abs(playPos.position.x - demon.eastXPos);
            distDiag = Mathf.Sqrt(distHori * distHori
                + Mathf.Pow(playPos.position.z - demon.zPos, 2f));
        }
        else
        {
            distHori = Mathf.Abs(playPos.position.x - demon.westXPos);
            distDiag = Mathf.Sqrt(distHori * distHori
                + Mathf.Pow(playPos.position.z - demon.zPos, 2f));
        }

        if(demon.segment == Demon.segments.Hallway)
        {
            trialStart = Time.time;
        }

        float trialTime = Time.time - trialStart;

        writer.WriteLine("Segment:" + spc + string.Format("{0:N3}", distHori)
            + spc + string.Format("{0:N3}", distDiag) + spc
            + string.Format("{0:N3}", playPos.eulerAngles.y) + spc
            + string.Format("{0:N3}", Time.time - startTime) + spc
            + string.Format("{0:N3}", playPos.position.x) + spc
            + string.Format("{0:N3}", playPos.position.z) + spc
            + string.Format("{0:N3}", trialTime) + spc
            + demon.trialNum.ToString() + spc + demon.segment.ToString());
    }

    public void WriteSelect(int select, int reward, int score)
    {
        writer.WriteLine("Selection " + demon.trialNum.ToString() + ":" + spc
            + select.ToString() + spc + reward.ToString() + spc
            + score.ToString());
    }

    private void OnApplicationQuit()
    {
        try
        {
            Debug.Log("Terminating eye tracker operation.");
            eyeTracker.GazeDataReceived -= EyeTracker_GazeDataReceived;
            EyeTrackingOperations.Terminate();
        }
        catch{}
    }

}
