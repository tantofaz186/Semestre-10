using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "AudioClipWithTempo", menuName = "AudioClipWithTempo", order = 0)]
public class AudioClipWithTempo : ScriptableObject
{
    public AudioClip audioClip;
    public List<int> spawningTempos;
    public float musicTempo;
    public float musicLoopTime;
}
