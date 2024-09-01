using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ControlMusic : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void ControlMusica(float sliderMusic){
        audioMixer.SetFloat("Music", Mathf.Log10(sliderMusic) * 20);
    }
}
