using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer ����")]
    public AudioMixer audioMixer;
    public AudioMixerGroup bgmMixer;
    public AudioMixerGroup sfxMixer;

    [Header("BGM ����")]
    public AudioClip[] bgmClips;
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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Init();
    }

    void Start()
    {
        InitSliders();

        masterVolSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmVolSlider.onValueChanged.AddListener(SetBgmVolume);
        sfxVolSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetBgmForScene(scene.name);
    }

    /// <summary>
    /// �� �̸��� ���� ������ BGM ����
    /// </summary>
    /// <param name="sceneName">���� �� �̸�</param>
    private void SetBgmForScene(string sceneName)
    {
        AudioClip selectedBgm = null;

        if (sceneName == "Home" || sceneName == "Village" || sceneName == "Title")
        {
            selectedBgm = bgmClips[0];
        }
        else if (sceneName == "Forest")
        {
            selectedBgm = bgmClips[1];
        }
        else
        {
            Debug.LogWarning($"'{sceneName}'�� ���� BGM ������ �����ϴ�. �⺻���� ����մϴ�.");
        }

        if (selectedBgm != null && bgmPlayer.clip != selectedBgm)
        {
            bgmPlayer.clip = selectedBgm;
            bgmPlayer.Play();
        }
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
        bgmPlayer.outputAudioMixerGroup = bgmMixer;

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
    ///  �����̴� ���� ���ú��� ���� �� ����
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.001f, 1f);
        audioMixer.SetFloat("MasterVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVol", volume);
    }

    public void SetBgmVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.001f, 1f);
        audioMixer.SetFloat("BGMVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BGMVol", volume);
    }

    public void SetSfxVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.001f, 1f);
        audioMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public float GetVolume(string volumeName)
    {
        audioMixer.GetFloat(volumeName, out float value);
        return Mathf.Pow(10, value / 20);
    }
}