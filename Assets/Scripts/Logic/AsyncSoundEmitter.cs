using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AsyncSoundEmitter : MonoBehaviour {

    private SoundLogger logger;

    private AudioSource audio;

    public void Start(){
        logger = new SoundLogger();
        audio = GetComponent<AudioSource>();
        // TODO Read some kind of configuration file to get args to PlaySounds()
        float low = 0.8f;
        float high = 1.2f;
        StartCoroutine(PlaySounds(low, high));
    }

    public IEnumerator PlaySounds(float low, float high){
        while(true){
            audio.Play();
            logger.LogSound(Time.time);
            yield return new WaitForSeconds(RandomHelper.Uniform(low, high));
        }
    }
}

