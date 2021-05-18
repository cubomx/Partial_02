using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour
{
    static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void playSound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
