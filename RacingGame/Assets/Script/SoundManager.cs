using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource idle;
    AudioSource running;
    AudioSource ready;
    AudioSource go;
    AudioSource hurryUp;
    AudioSource Congre;
    AudioSource Boost;
    AudioSource Drift;
    AudioSource Goal;
    AudioSource BGM;

    void Awake()
    {
        idle = GameObject.Find("idle").GetComponent<AudioSource>();
        running = GameObject.Find("running").GetComponent<AudioSource>();
        ready = GameObject.Find("Ready").GetComponent<AudioSource>();
        go = GameObject.Find("Go").GetComponent<AudioSource>();
        hurryUp = GameObject.Find("HurryUp").GetComponent<AudioSource>();
        Congre = GameObject.Find("Congre").GetComponent<AudioSource>();
        Boost = GameObject.Find("Boost").GetComponent<AudioSource>();
        Drift = GameObject.Find("Drift").GetComponent<AudioSource>();
        Goal = GameObject.Find("GoalSound").GetComponent<AudioSource>();
        BGM = GameObject.Find("BGM").GetComponent<AudioSource>();

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
    public void readyPlay()
    {
        if (!ready.isPlaying)
            ready.Play();
    }
    public void readyStop()
    {
        ready.Stop();
    }
    public void goPlay()
    {
        if (!go.isPlaying)
            go.Play();
    }
    public void goStop()
    {
        go.Stop();
    }
    public void hurryUpPlay()
    {
        if (!hurryUp.isPlaying)
            hurryUp.Play();
    }
    public void hurryUpStop()
    {
        hurryUp.Stop();
    }
    public void CongrePlay()
    {
        if (!Congre.isPlaying)
            Congre.Play();
    }
    public void CongreStop()
    {
        Congre.Stop();
    }
    public IEnumerator BoostPlay()
    {
        if (!Boost.isPlaying)
        {
            Boost.Play();
            yield return new WaitForSeconds(1f);
            Boost.Stop();
        }
    }
    public void BoostStop()
    {
        Boost.Stop();
    }
    public void DriftPlay()
    {
        if (!Drift.isPlaying)
            Drift.Play();
    }
    public void DriftStop()
    {
        Drift.Stop();
    }
    public void GoalPlay()
    {
        if (!Goal.isPlaying)
            Goal.Play();
    }
    public void GoalStop()
    {
        Goal.Stop();
    }
    public void BGMPlay()
    {
        if (!BGM.isPlaying)
            BGM.Play();
    }
    public void BGMStop()
    {
        BGM.Stop();
    }

}
