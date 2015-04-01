using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

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

	//Serializer and data stream
	private XmlSerializer m_serializer;
	private FileStream m_stream;

	//Holds data to be serialized
	private PlayerDataContainer m_data;

	private Timer logTimer;

	//
	//Helper methods
	//

	//Save current relevant information so it can be serialized
	private void UpdateData(){
		m_data.position = transform.position;
		m_data.rotation = transform.rotation.eulerAngles;
		m_data.time = Time.time;
	}

	//
	//Unity callbacks
	//

	//Setup serializer and data stream
	void Start(){
		m_serializer = new XmlSerializer(typeof(PlayerDataContainer));
		m_stream = new FileStream(XmlLogOutput, FileMode.Create);

		m_data = new PlayerDataContainer();

		logTimer = new Timer();
	}

	//Log our attached transform (position, rotation)
	void Update(){
		if(logTimer.isDone){
			UpdateData();
			m_serializer.Serialize(m_stream, m_data);
			logTimer.SetTimer(LogTimeInterval);
		}
	}

	void OnDestroy(){
		//Close stream and therefore write XML file
		m_stream.Close();
	}
}

