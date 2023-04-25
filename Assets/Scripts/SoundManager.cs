using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioClip clip;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}