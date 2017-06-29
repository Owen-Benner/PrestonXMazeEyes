using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(AudioSource))]
public class AsyncSoundEmitter : MonoBehaviour {

    // Config file
    private readonly string configFileName = "soundconfig.txt";

	public Transform SquareManager;

    public void Start(){
        SoundConfigInfo info = SoundConfigReader.CreateConfig(configFileName);
		print(string.Format("lo:{0}, hi:{1}, dur:{2}", info.lowTime, info.hiTime, info.durTime));
		StartCoroutine(PlaySounds(info.lowTime, info.hiTime, info.durTime));
    }

	public IEnumerator PlaySounds(float low, float high, float dur){
        // Play sounds forever, starting immediatly, delayed uniform time between low and high
        while(true){
			SquareManager.SendMessage ("Blink", dur);
            yield return new WaitForSeconds(RandomHelper.Uniform(low, high));
        }
    }
}

