﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Tobii.Gaming;

public class FileWriter : MonoBehaviour
{

    private GameObject player;
    private Transform playPos;
    private Demon demon;

    private string spc = " ";

    public int frameFreq = 10;

    public string format0 = "%Frame frameNum: distHoriz distDiag pose time xPos zPos";
    public string format1 = "%Selection trialNum: chamber reward score";
    public string format2 = "%Segment: distHoriz distDiag pose time xPos zPos trialNum trialTime newSegment";
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

    private bool write = false;
    private bool XMazeLoaded = false;

    private float startTime;

    // Start is called before the first frame update
    private void Start()
    {
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
            Debug.LogError("Repeated log filename! Update Config.txt or remove last log file from directory.");
            Application.Quit();
            return;
        }
        catch{}

        writer = new StreamWriter(fileName);
        framePer = 1f / (float)frameFreq;

        writer.WriteLine(partCode + spc + mode + spc + runNum + spc + direction);

        writer.WriteLine(format0);
        writer.WriteLine(format1);
        writer.WriteLine(format2);

        DontDestroyOnLoad(gameObject);

        lastFrame = Time.time;
    }

    public void XMazeInit()
    {
        player = GameObject.FindWithTag("Player");
        playPos = player.GetComponent<Transform>();
        demon = player.GetComponent<Demon>();

        XMazeLoaded = true;

        //WriteFrame();
        trialStart = Time.time;
    }

    public void StartWriting()
    {
        startTime = Time.time;
        write = true;
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
        if(XMazeLoaded)
        {
            float distHori;
            float distDiag;

            if (demon.direction == 1)
            {
                distHori = Mathf.Abs(playPos.position.x - demon.eastXPos);
                distDiag = Mathf.Sqrt(distHori * distHori + Mathf.Pow(playPos.position.z - demon.zPos, 2f));
            }
            else
            {
                distHori = Mathf.Abs(playPos.position.x - demon.westXPos);
                distDiag = Mathf.Sqrt(distHori * distHori + Mathf.Pow(playPos.position.z - demon.zPos, 2f));
            }

            writer.WriteLine("Frame "
            + frame.ToString() + ":" + spc
            + string.Format("{0:N3}", distHori) + spc + string.Format("{0:N3}", distDiag) + spc + string.Format("{0:N3}", playPos.eulerAngles.y)
                + spc + string.Format("{0:N3}", Time.time - startTime) + spc + string.Format("{0:N3}", playPos.position.x) + spc + string.Format("{0:N3}", playPos.position.z));
        }
        else
        {
            writer.WriteLine("Frame "
            + frame.ToString() + ":" + spc
            + string.Format("{0:N3}", 0f) + spc + string.Format("{0:N3}", 0f) + spc + string.Format("{0:N3}", 0f)
                + spc + string.Format("{0:N3}", Time.time - startTime) + spc + string.Format("{0:N3}", 0f) + spc + string.Format("{0:N3}", 0f));
            // Add gaze data here
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
            distDiag = Mathf.Sqrt(distHori * distHori + Mathf.Pow(playPos.position.z - demon.zPos, 2f));
        }
        else
        {
            distHori = Mathf.Abs(playPos.position.x - demon.westXPos);
            distDiag = Mathf.Sqrt(distHori * distHori + Mathf.Pow(playPos.position.z - demon.zPos, 2f));
        }

        if(demon.segment == Demon.segments.Hallway)
        {
            trialStart = Time.time;
        }

        float trialTime = Time.time - trialStart;

        writer.WriteLine("Segment:" + spc + string.Format("{0:N3}", distHori) + spc + string.Format("{0:N3}", distDiag) + spc + string.Format("{0:N3}", playPos.eulerAngles.y)
            + spc + string.Format("{0:N3}", Time.time - startTime) + spc + string.Format("{0:N3}", playPos.position.x) + spc + string.Format("{0:N3}", playPos.position.z)
            + spc + string.Format("{0:N3}", trialTime) + spc + demon.trialNum.ToString() + spc + demon.segment.ToString());
    }

    public void WriteSelect(int select, int reward, int score)
    {
        writer.WriteLine("Selection " + demon.trialNum.ToString() + ":" + spc + select.ToString() + spc + reward.ToString() + spc + score.ToString());
    }

}
