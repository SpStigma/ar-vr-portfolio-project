using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Clips")]
    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

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

    public void Start()
    {
        if(bgmClips.Length > 0)
        {
            PlayBGM(0);
        }
    }

    public void PlayBGM(int index)
    {
        if (index < 0 || index >= bgmClips.Length)
        {
            return;
        }

        AudioClip clip = bgmClips[index];
        if (bgmSource.clip == clip) 
        {
            return;
        }
        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void PlaySFX(int index)
    {
        if (index < 0 || index >= sfxClips.Length)
        {
            return;
        }

        sfxSource.pitch = Random.Range(0.9f, 1.1f);

        sfxSource.PlayOneShot(sfxClips[index]);

        sfxSource.pitch = 1.0f;
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