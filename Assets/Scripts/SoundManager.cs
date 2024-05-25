using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        
    }

    public void PlaySoundFXclip(AudioClip audioclip, Transform spawnTransform, float volume) {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioclip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip.length; 

        Destroy(audioSource.gameObject, clipLength);
    }

}
