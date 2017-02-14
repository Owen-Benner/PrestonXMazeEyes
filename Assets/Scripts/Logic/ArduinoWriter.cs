using UnityEngine; // XXX DEUBG
using System;
using System.Collections;
using System.Threading;
using System.Linq;
using System.IO;
using System.IO.Ports;

public class ArduinoWriter : ILog, IDisposable {

    private string[] portnames;
    private SerialPort[] ports;

    private const int baudrate = 9600;
    private readonly byte[] arduinomsg = new byte[1];

    public ArduinoWriter(){
        portnames = SerialPort.GetPortNames();
        ports = portnames.Select(x => new SerialPort(x, baudrate)).ToArray();
        Debug.Log(string.Format("ArduinoWriter: Found these serial ports: {0}.", string.Join(", ", portnames)));

        // Open every port we found
        foreach(SerialPort port in ports)
            port.Open();
    }

    public void Log(string msg){
        // Write a byte to each port, ignore msg
        Debug.Log("ArduinoWriter: Wrote to all ports");
        foreach(SerialPort port in ports){
            port.Write(arduinomsg, 0, arduinomsg.Length);
        }
    }

    public void Dispose(){  
        Dispose(true);  
        GC.SuppressFinalize(this);  
    } 

    protected virtual void Dispose(bool disposing){  
        if(!disposing)
            return;
        // Close our ports
        foreach(SerialPort port in ports)
            port.Close();
    } 
}


