using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel { Master, Music, SFX}

    public float masterVolumePercent { get; private set; }
    public float musicVolumePercent { get; private set; }
    public float SFXVolumePercent { get; private set; }

    AudioSource SFX2DSource;
    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;

    //Transform audioListener;
    //Transform player;

    SoundLibrary soundLibrary;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            soundLibrary = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source ");
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }


            GameObject newSFX2DSource = new GameObject("2D SFX source ");
            SFX2DSource = newSFX2DSource.AddComponent<AudioSource>();
            newSFX2DSource.transform.parent = transform;

            masterVolumePercent = PlayerPrefs.GetFloat("master volume", 1);
            musicVolumePercent = PlayerPrefs.GetFloat("music volume", 1);
            SFXVolumePercent = PlayerPrefs.GetFloat("sfx volume", 1);
        }
    }
    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
            case AudioChannel.SFX:
                SFXVolumePercent = volumePercent;
                break;
        }

        musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        musicSources[1].volume = musicVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("master volume", masterVolumePercent);
        PlayerPrefs.SetFloat("music volume", musicVolumePercent);
        PlayerPrefs.SetFloat("sfx volume", SFXVolumePercent);
        PlayerPrefs.Save();
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();
        StartCoroutine(MusicCrossFade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if(clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, SFXVolumePercent * masterVolumePercent);
        }
    }

    public void PlaySound(string soundName, Vector3 pos)
    {
        PlaySound(soundLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        SFX2DSource.PlayOneShot(soundLibrary.GetClipFromName(soundName), SFXVolumePercent * masterVolumePercent);
    }

    IEnumerator MusicCrossFade(float duration)
    {
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);
            yield return null;
        }
        
    }
}
