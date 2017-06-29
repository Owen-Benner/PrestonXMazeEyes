using System.Text;
using System;
using System.IO;  
using UnityEngine;
using System.Collections;

// Reads a sound config file and stores info from it
// TODO Audio file should be specifiable
public class SoundConfigReader {
    public static SoundConfigInfo CreateConfig(string filename){
        // TODO Try/catch
        string[] lines = System.IO.File.ReadAllLines(filename);
        return new SoundConfigInfo(
                Single.Parse(lines[0]),
                Single.Parse(lines[1]),
				Single.Parse(lines[2])
                );
    }
}

public class SoundConfigInfo {
    public readonly float lowTime; // Minimum time between sounds being played
    public readonly float hiTime;  // Maximum time between sounds being played
	public readonly float durTime; // Time sound is held on
	public SoundConfigInfo(float low, float hi, float duration){
        lowTime = low;
        hiTime = hi;
		durTime = duration;
    }
}
