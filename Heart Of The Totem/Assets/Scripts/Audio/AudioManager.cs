using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        bgmSource = transform.Find("BGM").GetComponent<AudioSource>();
        sfxSource = transform.Find("SFX").GetComponent<AudioSource>();
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void StopAllSounds()
    {
        bgmSource.Stop();
        sfxSource.Stop();
    }
}

