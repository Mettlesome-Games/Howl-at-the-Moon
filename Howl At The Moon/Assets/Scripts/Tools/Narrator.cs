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
        WaveTimer.OnSpawnWaveEvent += PlaySegement;
  
       // print("Number of lines: " + Narrations.Length);
    }

    void PlaySegement() {
        if (currentTrack >= (Narrations.Length - 1))
        {
            NarratorBox.PlayOneShot(Narrations[currentTrack]);
            currentTrack++;
        }
        return;
    }
}
