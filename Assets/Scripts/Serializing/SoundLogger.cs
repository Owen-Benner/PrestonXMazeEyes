using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

// Logs sound events to some XML file
public class SoundLogger {
    public SoundLogger(){
        // TODO Everything
    }

    public void LogSound(float time){
        // TODO write to a file or something
        Debug.Log(string.Format("Played sound at: {0}.", time));
    }
}

