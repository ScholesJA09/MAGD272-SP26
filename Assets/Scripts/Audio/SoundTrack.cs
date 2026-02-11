using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct soundtrackOptions
{
    public AudioClip audioClip;
    public int playCount;
}

[RequireComponent(typeof(AudioSource))]
public class SoundTrack : MonoBehaviour
{
    public static SoundTrack soundTrack;
    [Header("Do you want a soundtrack for your game? Add it below.")]
    public soundtrackOptions[] soundtrack = new soundtrackOptions[] { };

    AudioSource soundTrackSource;

    int m_currentTrackIndex;

    int m_currentSceneIndex;

    private void Awake()
    {
        if (soundTrack == null)
        {
            soundTrack = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // grabs the current scene's index
        m_currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.sceneLoaded += sceneChanged;

        soundTrackSource = GetComponent<AudioSource>();
    }

    public void StopSoundtrack(){
        if(soundTrackSource.isPlaying){
            soundTrackSource.Stop();
        }
    }
    private void Start()
    {
        soundTrackSource.loop = false;
        if (soundtrack.Length > 0)
        {
            selectAndPlayNewTrack();
            soundTrackSource.Play();
        }
    }

    private void Update()
    {
        if (!soundTrackSource.isPlaying && soundtrack.Length > 0)
        {
            selectAndPlayNewTrack();
        }
    }

    void sceneChanged(Scene scene, LoadSceneMode mode)
    {
        m_currentSceneIndex = scene.buildIndex;
    }

    void selectAndPlayNewTrack()
    {
        selectNewTrack(randInts());
        int rand;
        do
        {
            rand = UnityEngine.Random.Range(0, soundtrack.Length);
        } while (soundtrack[rand].playCount > soundtrack[m_currentTrackIndex].playCount);

        print("found new track");
        soundTrackSource.clip = soundtrack[rand].audioClip;
        soundtrack[rand].playCount++;
        m_currentTrackIndex = rand;
        soundTrackSource.Play();
        return;

    }

    int randInts()
    {
        List<int> rands = new List<int>();
        int i = 0;
        if(soundtrack.Length > 3)
        {
            while (i < 3)
            {
                int tmpInt = Random.Range(0, soundtrack.Length);
                if (!rands.Contains(tmpInt))
                {
                    rands.Add(tmpInt);
                    i++;
                }
            }
            rands.Sort();
            print(rands[2]);
            return rands[2];
        } else
        {
            return Random.Range(0, soundtrack.Length);
        }

    }

    void selectNewTrack(int num)
    {
        print("selecting new track");
        print("comparing " + num + " " + soundtrack[m_currentTrackIndex].playCount);
        if (soundtrack[m_currentTrackIndex].playCount < 2)
        {
            print("found new track");
            soundTrackSource.clip = soundtrack[num].audioClip;
            soundtrack[num].playCount++;
            m_currentTrackIndex = num;
            return;
        }

        if (num <= soundtrack[m_currentTrackIndex].playCount)
        {
            print("found new track");
            soundTrackSource.clip = soundtrack[num].audioClip;
            soundtrack[num].playCount++;
            m_currentTrackIndex = num;
            return;
        }
    }
}