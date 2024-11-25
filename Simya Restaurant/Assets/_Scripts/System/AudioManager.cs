using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer ����")]
    public AudioMixer audioMixer;
    public AudioMixerGroup bgmMixer;
    public AudioMixerGroup sfxMixer;

    [Header("BGM ����")]
    public AudioClip bgmClip;
    public float bgmVolume;
    private AudioSource bgmPlayer;

    [Header("SFX ����")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;
    private AudioSource[] sfxPlayers;
    private int sfxChannelIndex;

    [Header("Vol UI")]
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider bgmVolSlider;
    [SerializeField] private Slider sfxVolSlider;

    /// <summary>
    /// ���� ȿ���� ����
    /// </summary>
    /// <returns></returns>
    public enum SFX
    {
        TestSFX_1,
        TestSFX_2,
        TestSFX_3,
        TestSFX_4
    }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    void Start()
    {
        InitSliders();

        masterVolSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxVolSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    /// <summary>
    /// �����̴� �ʱ�ȭ (����� �ͼ��� ���� �� �ݿ�)
    /// </summary>
    /// <returns></returns>
    void InitSliders()
    {
        float volume;

        if (audioMixer.GetFloat("MasterVol", out volume))
        {
            masterVolSlider.value = Mathf.Pow(10, volume / 20);
        }

        if (audioMixer.GetFloat("BGMVol", out volume))
        {
            bgmVolSlider.value = Mathf.Pow(10, volume / 20);
        }

        if (audioMixer.GetFloat("SFXVol", out volume))
        {
            sfxVolSlider.value = Mathf.Pow(10, volume / 20);
        }
    }

    /// <summary>
    /// ����� �÷��̾� �ʱ�ȭ
    /// </summary>
    /// <returns></returns>
    void Init()
    {
        // BGM Player �ʱ�ȭ
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        bgmPlayer.outputAudioMixerGroup = bgmMixer;

        PlayBgm(true);

        // SFX Player �ʱ�ȭ
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
            sfxPlayers[index].outputAudioMixerGroup = sfxMixer;
        }
    }

    /// <summary>
    /// BGM ���
    /// </summary>
    /// <returns></returns>
    public void PlayBgm(bool isBgmPlay)
    {
        if (isBgmPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    /// <summary>
    /// SFX ���
    /// </summary>
    /// <returns></returns>
    public void PlaySfx(SFX sfx)
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + sfxChannelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
            {
                continue;
            }

            sfxChannelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }


    /// <summary>
    /// �����̴� ���� ���ú� ������ ��ȯ
    /// </summary>
    /// <returns></returns>
    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.001f, 1f);
        audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
    }

    public void SetBgmVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.001f, 1f);
        audioMixer.SetFloat("BGMVol", Mathf.Log10(volume) * 20);
    }

    public void SetSfxVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.001f, 1f);
        audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }
}