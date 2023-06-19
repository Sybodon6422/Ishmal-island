using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    public Sound[] sounds;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void PlaySound(string name)
    {
        Sound sound = FindSoundByName(name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }

        sound.source.Play();
    }

    public void StopSound(string name)
    {
        Sound sound = FindSoundByName(name);
        if (sound == null)
        {
            Debug.LogWarning("Sound with name " + name + " not found!");
            return;
        }

        sound.source.Stop();
    }

    private Sound FindSoundByName(string name)
    {
        return System.Array.Find(sounds, sound => sound.name == name);
    }
}