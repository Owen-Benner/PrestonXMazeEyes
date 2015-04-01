using UnityEngine;
using System.Collections;
using System.Xml;

//Logs to a file every fixed update
//TODO
//	Use a queued thread system:
//		push updates to the thread,
//		it flushes every so and so time
//	Use xml
//	Integrate with existing matlab workflow in Preston_Nav/tools/
public class Logger : MonoBehaviour {

	//Path to output
	public string XmlLogOutput = "temp.xml";

	//Log timer interval
	public float LogTimeInterval = .8f;

	//XmlWriter
	private XmlWriter m_writer;

	//Timer
	private Timer logTimer;

	//
	//Helper methods
	//

	//Output current state to Xml
	private void WriteData(){
		//Writing this tick...
		m_writer.WriteStartElement("tick");

		m_writer.WriteElementString("pos", transform.position.ToString());
		m_writer.WriteElementString("rot", transform.rotation.ToString());
		m_writer.WriteElementString("time", Time.time.ToString());

		//...done!
		m_writer.WriteEndElement();
	}

	//
	//Unity callbacks
	//

	void Start(){
		//Setup timer
		logTimer = new Timer();

		//Xml
		//

		//Setup XmlWriter with indenting enabled (uses hot C# syntax for Object Initializer)
		//TODO This outputs bad xml, for some reason!!! TODO
		//TODO Try/catch/finally
		m_writer = XmlWriter.Create(XmlLogOutput, new XmlWriterSettings(){Indent = true});

		//Start our document
		m_writer.WriteStartDocument();
		m_writer.WriteStartElement("Test output");
	}

	//Log whatever
	void Update(){
		if(logTimer.isDone){
			WriteData();
			logTimer.SetTimer(LogTimeInterval);
		}
	}

	//TODO exceptions/using statements
	void OnDestroy(){
		//Elements
		m_writer.WriteEndElement();
		m_writer.WriteEndDocument();

		//Close file
		m_writer.Close();
	}
}

