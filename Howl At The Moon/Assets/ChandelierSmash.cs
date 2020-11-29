﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Nate Hales


public class ChandelierSmash : MonoBehaviour
{
    [SerializeField] AudioClip crashSFX;
    [SerializeField] UnityEngine.Audio.AudioMixerGroup m_Channel;
    bool bUsed = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !bUsed) {
           AudioSource mSFXBox = this.gameObject.AddComponent<AudioSource>();
            mSFXBox.outputAudioMixerGroup = m_Channel;
            mSFXBox.clip = crashSFX;
            mSFXBox.time = 1;
            mSFXBox.Play();
            

            WerewolfAI ai = collision.gameObject.GetComponent<WerewolfAI>();
            if (ai != null) { ai.TakeDamage(ai.HP); }

            bUsed = true;
        }
    }
}
