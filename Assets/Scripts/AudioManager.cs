using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioClip[] clip;
    public AudioClip[] voice;
    public AudioClip[] voiceEng;
    public AudioClip[] animalSound;
    public AudioSource audioBgm;
    public AudioSource audioSfx;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetSfx(int id)
    {
        audioSfx.PlayOneShot(clip[id]);
    }

    public void SetVoice(int id)
    {
        audioSfx.PlayOneShot(voice[id]);
    }

    public void SetVoiceEng(int id)
    {
        audioSfx.PlayOneShot(voiceEng[id]);
    }

    public void SetAnimalSound(int id)
    {
        audioSfx.PlayOneShot(animalSound[id]);
    }

    public void SetBgm(int id)
    {
        audioBgm.clip = clip[id];
        audioBgm.Play();
    }

    public void WinLose(int id)
    {
        audioSfx.PlayOneShot(clip[id]);
    }
}
