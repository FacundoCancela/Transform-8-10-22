using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Para usar:
    //1) Cuando quieras tiras un AudioManager.instance.PlaySFX("NombreDelArchivo");

    [SerializeField] private GameObject sfxFolder;
    private AudioSource[] sfxAudioSources;

    public static AudioManager instance;

    //################ #################
    //------------UNITY F--------------
    //################ #################
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        sfxAudioSources = sfxFolder.GetComponentsInChildren<AudioSource>(true);

        DontDestroyOnLoad(gameObject);
    }

    //################ #################
    //----------CLASS METHODS-----------
    //################ #################

    public void PlaySFX(string sfxToPlayFileName)
    {
        sfxToPlayFileName = sfxToPlayFileName + " (UnityEngine.AudioSource)";

        for (int i = 0; i < sfxAudioSources.Length; i++)
        {
            string sfxFileName = sfxAudioSources[i].ToString();

            if (sfxFileName == sfxToPlayFileName)
            {
                AudioClip sfxToPlayClip = sfxAudioSources[i].clip;
                sfxAudioSources[i].PlayOneShot(sfxToPlayClip, 1);
            }
        }
    }
}
