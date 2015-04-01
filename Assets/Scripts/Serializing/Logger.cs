using UnityEngine;
using System.Collections;

//Logs to a file every fixed update
//TODO
//	Use a queued thread system:
//		push updates to the thread,
//		it flushes every so and so time
//	Use xml
//	Integrate with existing matlab workflow in Preston_Nav/tools/
public class Logger : MonoBehaviour {
	//Path to output
	public string XmlLogOutput;

	//Serializer and data stream
	private XmlSerializer m_serializer;
	private FileStream m_stream;

	//Holds data to be serialized
	private PlayerDataContainer m_data;

	//Setup serializer and data stream
	void Start(){
		m_serializer = new XmlSerializer(typeof(PlayerDataContainer));
		m_stream = new FileSream(XmlLogOutput, FileMode.Create);
	}

	//Log our attached transform (position, rotation)
	void FixedUpdate(){
		/* Don't do this yet... */
		//serializer.Serialize(m_stream, m_data);
	}

	void OnDestroy(){
		//Close stream and therefore write XML file
		m_stream.Close();
	}
}

