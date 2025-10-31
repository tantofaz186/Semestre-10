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
    public event Action OnMusicEnd;
    private void Start()
    {
        Sword.Instance.OnCut += PlayCutSound;
    }


    public void StopMusic()
    {
        mainAudioSource.loop = false;
        StartCoroutine(InvokeEventWhenMusicEnds());
    }

    public void PlayNext(AudioClip music)
    {
        StartCoroutine(PlayNextMusic(music));
    }
    private IEnumerator PlayNextMusic(AudioClip music)
    {
        mainAudioSource.loop = false;
        yield return new WaitUntil(() => !mainAudioSource.isPlaying);
        mainAudioSource.clip = music;
        mainAudioSource.loop = true;
        mainAudioSource.Play();
    }
    private void PlayMusic(AudioClip musicClip)
    {
        mainAudioSource.loop = true;
        mainAudioSource.clip = musicClip;
        mainAudioSource.Play();
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
    private AudioClip nextClipToPlay;
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
        
        nextClipToPlay = EditorGUILayout.ObjectField("Audio Clip to Play", nextClipToPlay, typeof(AudioClip), false) as AudioClip;
        if (GUILayout.Button("Play Next Music"))
        {
            audioController.PlayNext(nextClipToPlay);
        }
    }
}
#endif