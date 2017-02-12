using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AsyncSoundEmitter : MonoBehaviour {

    // Logger, and related variables
    private SoundLogger logger;
    private readonly string configFileName = "soundconfig.txt"; // TODO Use this
    private readonly string outputFileName = "soundoutput.xml";

    private AudioSource myaudio;

    public void Start(){
        logger = new SoundLogger(outputFileName);
        myaudio = GetComponent<AudioSource>();
        float low = 0.8f;
        float high = 1.2f;
        StartCoroutine(PlaySounds(low, high));
    }

    public IEnumerator PlaySounds(float low, float high){
        // Play sounds forever, starting immediatly, delayed uniform time between low and high
        while(true){
            myaudio.Play();
            logger.LogSound(Time.time);
            yield return new WaitForSeconds(RandomHelper.Uniform(low, high));
        }
    }
    
    void OnDestroy(){
        logger.Dispose();
    }
}

