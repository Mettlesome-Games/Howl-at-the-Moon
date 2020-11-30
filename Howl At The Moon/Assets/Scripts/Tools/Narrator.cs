using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//aurthor: Nate H
//Narrator watchs for the next wave to be called to speak its lines.
public class Narrator : MonoBehaviour
{
    [SerializeField] AudioClip[] Narrations;
    [SerializeField] UnityEngine.Audio.AudioMixerGroup m_Channel;

    int currentTrack = 0;
    AudioSource NarratorBox;

    private void Awake() 
    {
        NarratorBox = this.gameObject.AddComponent<AudioSource>();
        NarratorBox.outputAudioMixerGroup = m_Channel;
        NarratorBox.playOnAwake = false;
       
        WaveTimer.OnSpawnWaveEvent += Narrate;
  
    }

    IEnumerator PlaySegement() {
        yield return new WaitUntil(() => !NarratorBox.isPlaying);

        if (currentTrack <= (Narrations.Length - 1))
        {
            NarratorBox.PlayOneShot(Narrations[currentTrack]);
            currentTrack++;
        }
    }

    private void Narrate() { StartCoroutine(PlaySegement()); }

    private void OnDestroy() { WaveTimer.OnSpawnWaveEvent -= Narrate; }

}
