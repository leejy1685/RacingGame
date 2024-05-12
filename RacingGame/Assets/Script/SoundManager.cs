using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource idle;
    AudioSource running;

    void Start()
    {
        idle = GameObject.Find("idle").GetComponent<AudioSource>();
        running = GameObject.Find("running").GetComponent<AudioSource>();
    }

    public void idlePlay()
    {
        if (!idle.isPlaying)
            idle.Play();
    }
    public void idleStop()
    {
        idle.Stop();
    }

    public void runningPlay()
    {
        if(!running.isPlaying)
            running.Play();
    } 
    public void runningStop()
    {
        running.Stop();
    }

}
