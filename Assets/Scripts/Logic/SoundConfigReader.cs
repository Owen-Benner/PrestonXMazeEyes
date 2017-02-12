using System.Text;
using System;
using System.IO;  
using UnityEngine;
using System.Collections;

// Reads a sound config file and stores info from it
// TODO Audio file should be specifiable
public class SoundConfigReader {
    public static SoundConfigInfo ReadConfig(string filename){
        // TODO Try/catch
        string[] lines = System.IO.File.ReadAllLines(filename);
        return new SoundConfigInfo(
                Single.Parse(lines[0]),
                Single.Parse(lines[1])
                );
    }
}

public class SoundConfigInfo {
    public readonly float lowTime; // Minimum time between sounds being played
    public readonly float hiTime;  // Maximum time between sounds being played
    public SoundConfigInfo(float low, float hi){
        lowTime = low;
        hiTime = hi;
    }
}
