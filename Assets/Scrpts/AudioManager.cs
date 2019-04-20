using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
   public AudioClip Shuffle;
    [SerializeField]
  public  AudioClip[] draw;
    [SerializeField]
 public   AudioClip Match;
    AudioSource AudioSource;
        
        
        void Start() {

        AudioSource = GetComponent<AudioSource>();
    }


   public void PlayAudio(AudioClip clip)
    {
        if (!AudioSource.isPlaying)
        {
            AudioSource.clip = clip;
            AudioSource.Play();
        }

    }
}
