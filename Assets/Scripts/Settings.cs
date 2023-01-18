using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    private bool active = false;

    [Header("Component UI")]
    public Slider sliderSfx;
    public Slider sliderBgm;
    public GameObject muteMusicOn;
    public GameObject muteMusicOff;
    public GameObject muteSfxOn;
    public GameObject muteSfxOff;
    public GameObject setupLang = null;

    [Header("First Setup Game")]
    [SerializeField] Animator mainMenu = null;
    [SerializeField] Animator button = null;
    [SerializeField] AudioSource audioBgm = null;

    [Space][Space]

    [Header("Data Value")]
    public float sfxValue;
    public float bgmValue;
    public string muteMusic;
    public string muteSfx;
    public int lang;

    public void Start()
    {
        // Get value in PlayerPrefs data
        sliderSfx.value = PlayerPrefs.GetFloat("sfxValue", sfxValue);
        sliderBgm.value = PlayerPrefs.GetFloat("bgmValue", bgmValue);

        // Get value in PlayerPrefs data
        MuteMusic(PlayerPrefs.GetString("muteBgm", muteMusic));
        MuteSfx(PlayerPrefs.GetString("muteSfx", muteSfx));

        if (!PlayerPrefs.HasKey("Language"))
        {
            if (setupLang != null) setupLang.SetActive(true);
        }
        else
        {
            if (setupLang != null) setupLang.SetActive(false);
            ChangeLanguage(PlayerPrefs.GetInt("Language", lang));

            if (mainMenu != null) mainMenu.enabled = true;
            if (button != null) button.enabled = true;
            if (audioBgm != null) audioBgm.enabled = true;
        }
    }

    public void Update()
    {
        AudioManager.instance.audioBgm.volume = sliderBgm.value;
        AudioManager.instance.audioSfx.volume = sliderSfx.value;

        MuteMusic(PlayerPrefs.GetString("muteBgm", muteMusic));
        MuteSfx(PlayerPrefs.GetString("muteSfx", muteSfx));
    }

    public void ChangeVolume(bool Sfx)
    {
        if (Sfx)
        {
            sfxValue = AudioManager.instance.audioSfx.volume = sliderSfx.value;
            PlayerPrefs.SetFloat("sfxValue", sfxValue);
        }
        else
        {
            bgmValue = AudioManager.instance.audioBgm.volume = sliderBgm.value;
            PlayerPrefs.SetFloat("bgmValue", bgmValue);
        }
    }

    public void MuteMusic(string muteVolume)
    {
        if (muteVolume == "True")
        {
            muteMusicOn.SetActive(false);
            muteMusicOff.SetActive(true);
            sliderBgm.interactable = false;
            AudioManager.instance.audioBgm.mute = true;
            muteMusic = muteVolume;
            PlayerPrefs.SetString("muteBgm", muteMusic);
        }
        if (muteVolume == "False")
        {
            muteMusicOn.SetActive(true);
            muteMusicOff.SetActive(false);
            sliderBgm.interactable = true;
            AudioManager.instance.audioBgm.mute = false;
            muteMusic = muteVolume;
            PlayerPrefs.SetString("muteBgm", muteMusic);
        }
    }

    public void MuteSfx(string muteVolume)
    {
        if (muteVolume == "True")
        {
            muteSfxOn.SetActive(false);
            muteSfxOff.SetActive(true);
            sliderSfx.interactable = false;
            AudioManager.instance.audioSfx.mute = true;
            muteSfx = muteVolume;
            PlayerPrefs.SetString("muteSfx", muteSfx);
        }
        if (muteVolume == "False")
        {
            muteSfxOn.SetActive(true);
            muteSfxOff.SetActive(false);
            sliderSfx.interactable = true;
            AudioManager.instance.audioSfx.mute = false;
            muteSfx = muteVolume;
            PlayerPrefs.SetString("muteSfx", muteSfx);
        }
    }

    public void ChangeLanguage(int id)
    {
        if (active == true) return;
        StartCoroutine(SetLanguage(id));
    }
    IEnumerator SetLanguage(int id)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
        PlayerPrefs.SetInt("Language", id);
        active = false;
    }
}
