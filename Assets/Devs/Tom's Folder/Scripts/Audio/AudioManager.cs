using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource audioSource;

    public List<SoundEffects> Sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
        {
            Destroy(instance);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string label, float Volume)
    {
        // PREDICATE
        SoundEffects sfx = Sounds.Find(sfx => sfx.Name == label);
        if (sfx != null)
        {
            audioSource.volume = Volume;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(sfx.Clip);
        }
    }
}
