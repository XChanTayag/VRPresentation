using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {

    public AudioSource audio;


    // Use this for initialization

    private void Awake()
    {
        this.audio = GetComponent<AudioSource>();
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayAudio(AudioClip clip)
    {
        audio.clip = clip;
        audio.Play();

    }

    public void StopAudio()
    {
        audio.Stop();
    }
}
