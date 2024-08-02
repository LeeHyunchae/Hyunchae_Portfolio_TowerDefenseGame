using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EAudioClip
{
    BGM,
    DEAD,
    HIT_ONE,
    HIT_TWO,
    LEVELUP,
    LOSE,
    MELEE_ONE,
    MELEE_TWO,
    RANGE,
    SELECT,
    WIN
}

public class SoundManager : MonoBehaviour
{
    private const int CHANNELCOUNT = 20;
    private static SoundManager instance;

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private Transform sfxGroupTransform;
    [SerializeField] private GameObject originSfxObejct;
    [SerializeField] private AudioClip[] allAudioClip;

    private float bgmVolume = 0.25f;

    private AudioSource[] sfxAudioSourceArr = new AudioSource[CHANNELCOUNT];
    private float sfxVolume = 0.75f;

    private int curSfxIndex;

    public static SoundManager getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SoundManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Init()
    {
        bgmAudioSource.clip = allAudioClip[(int)EAudioClip.BGM];
        bgmAudioSource.loop = true;
        bgmAudioSource.volume = bgmVolume;
        bgmAudioSource.playOnAwake = true;
        bgmAudioSource.Play();

        for(int i = 0; i < CHANNELCOUNT; i++)
        {
            sfxAudioSourceArr[i] = Instantiate<GameObject>(originSfxObejct, sfxGroupTransform).GetComponent<AudioSource>();
            sfxAudioSourceArr[i].loop = false;
            sfxAudioSourceArr[i].volume = sfxVolume;
            sfxAudioSourceArr[i].playOnAwake = false;
        }
    }

    public void PlaySFX(EAudioClip clipType)
    {
        AudioClip clip = allAudioClip[(int)clipType];

        if (clip == null)
        {
            return;
        }

        AudioSource audioSource = sfxAudioSourceArr[curSfxIndex];

        audioSource.clip = clip;
        audioSource.Play();

        curSfxIndex = (curSfxIndex++) % CHANNELCOUNT;
    }
}
