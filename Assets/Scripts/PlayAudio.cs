using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour {

    // Use this for initialization
    [SerializeField] private new AudioSource[] audio;

	void Start () {
        playAudio();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playAudio()
    {
        int num = Random.Range(0, audio.Length);
        bool alreadyPlaying = false;
        for (int x = 0; x < audio.Length; x++)
        {
            if(audio[x].isPlaying)
            {
                alreadyPlaying = false;
                break;
            }
            else
            {
                alreadyPlaying = true;
            }
        }
        if(alreadyPlaying)
        {
            audio[num].Play();
            Debug.Log(audio[num].tag);
        }
        
    }

    public void StopAudio()
    {
        for (int x = 0; x < audio.Length; x++)
        {
            audio[x].Stop();
        }
    }

    //private void HandleAudio()
    //{
    //    string temp = player.playAudio();
    //    if (temp == "")
    //    {
    //        Debug.Log(audio_tag);
    //    }
    //    else
    //    {
    //        audio_tag = temp;
    //    }
    //}

    //private void ValidateAudio()
    //{
    //    Debug.Log("Object Tag" + m_Tag);
    //    Debug.Log("Audio Tag" + audio_tag);
    //    if (m_Tag == audio_tag)
    //    {
    //        Debug.Log("Success");
    //        StopAudio();
    //        HandleAudio();
    //    }
    //    else
    //    {
    //        Debug.Log("Failed");
    //    }
    //}

}
