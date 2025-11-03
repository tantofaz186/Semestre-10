using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] List<AudioClipWithTempo> mainMusicClips;
    [SerializeField] AudioClip cutSound;
    public static event Action OnMusicEnd;
    public static event Music OnMusicStart;
    public delegate void Music(AudioClipWithTempo music);
    private void Start()
    {
        Sword.Instance.OnCut += PlayCutSound;
        PlayMusic(mainMusicClips[0]);
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

        nextClipToPlay = EditorGUILayout.ObjectField("Audio Clip to Play", nextClipToPlay, typeof(AudioClipWithTempo), false) as AudioClipWithTempo;
        if (GUILayout.Button("Play Next Music"))
        {
            audioController.PlayNext(nextClipToPlay);
        }
    }
}
#endif