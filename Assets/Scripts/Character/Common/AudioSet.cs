using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSet<T>
{
    [SerializeField] private T name;
    [SerializeField] private AudioSource source;

    public T Name => name;
    public AudioSource Source => source;
}
