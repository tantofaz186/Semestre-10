using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] List<AudioClipWithTempo> mainMusicClips;
    [SerializeField] AudioClip cutSound;
    [SerializeField] private AudioMixer mixer;

    public static event Action OnMusicEnd;
    public static event Music OnMusicStart;

    public delegate void Music(AudioClipWithTempo music);

    private IEnumerator Start()
    {
        yield return null;
        Sword.Instance.OnCut += PlayCutSound;
        PlayMusic(mainMusicClips[0]);
        ReloadVolume();
        StartCoroutine(NextMusicAtRandom());
    }
    private void OnDestroy()
    {
        Sword.Instance.OnCut -= PlayCutSound;
    }

    int PickRandomTimeForNextMusic => Random.Range(3, 9) * 2;

    private IEnumerator ChangeMusicAtRandom()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(PickRandomTimeForNextMusic);
            PlayNext(mainMusicClips[Random.Range(0, mainMusicClips.Count)]);
            yield return null;
        }
    }
    private int currentMusicIndex = 0;
    private IEnumerator NextMusicAtRandom()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(PickRandomTimeForNextMusic);
            PlayNext(mainMusicClips[++currentMusicIndex % mainMusicClips.Count]);
            yield return null;
        }
    }
    public void ReloadVolume()
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume")) * 20);
        mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume")) * 20);
    }


    public void StopMusic()
    {
        mainAudioSource.loop = false;
        StartCoroutine(InvokeEventWhenMusicEnds());
    }

    public void PlayNext(AudioClipWithTempo music)
    {
        StartCoroutine(PlayNextMusic(music));
    }

    private IEnumerator PlayNextMusic(AudioClipWithTempo music)
    {
        mainAudioSource.loop = false;
        yield return new WaitUntil(() => !mainAudioSource.isPlaying);
        PlayMusic(music);
    }

    private void PlayMusic(AudioClipWithTempo musicClip)
    {
        mainAudioSource.clip = musicClip.audioClip;
        mainAudioSource.loop = true;
        mainAudioSource.Play();
        OnMusicStart?.Invoke(musicClip);
    }

    private IEnumerator InvokeEventWhenMusicEnds()
    {
        yield return new WaitUntil(() => !mainAudioSource.isPlaying);
        OnMusicEnd?.Invoke();
    }

    private void PlayCutSound(Plane plane)
    {
        PlayCutSound();
    }

    public void PlayCutSound()
    {
        sfxAudioSource.PlayOneShot(cutSound);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioController))]
public class AudioControllerEditor : Editor
{
    AudioController audioController;
    private AudioClipWithTempo nextClipToPlay;

    private void OnEnable()
    {
        audioController = (AudioController)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Stop Music"))
        {
            audioController.StopMusic();
        }

        nextClipToPlay =
            EditorGUILayout.ObjectField("Audio Clip to Play", nextClipToPlay, typeof(AudioClipWithTempo), false) as AudioClipWithTempo;
        if (GUILayout.Button("Play Next Music"))
        {
            audioController.PlayNext(nextClipToPlay);
        }

        if (GUILayout.Button("Reload Volume"))
        {
            audioController.ReloadVolume();
        }
    }
}
#endif