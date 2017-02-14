using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

// Logs sound events to some XML file
public class SoundLogger : IDisposable, ILog {
    private string filename;
    private List<string> times; // list of time sounds were logged
    
    public SoundLogger(string filename){
        this.filename = filename;
        times = new List<string>();
    }

    public void Log(string msg){
        times.Add(msg);
        Debug.Log(string.Format("Played sound at: {0}.", msg));
    }

    public void Dispose(){  
        Dispose(true);  
        GC.SuppressFinalize(this);  
    }  

    protected virtual void Dispose(bool disposing){  
        if(!disposing)
            return;
        // Build the xml document and save it
        XDocument xdoc = new XDocument(new XElement("SoundTimes",
                    times.Select(time => new XElement("Event", time))));
        xdoc.Save(filename); // Overwrites a file if it exists
    } 
}

