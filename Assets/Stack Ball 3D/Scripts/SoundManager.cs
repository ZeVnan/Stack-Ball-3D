using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    #region Serialize Field
    [SerializeField]
    public bool sound = true;
    #endregion

    #region Field
    private AudioSource audioSource { get; set; }

    #endregion

    #region Overrided/Impletented Method
    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }
    #endregion
    
    #region Custom Method
    public void ToggleSound()
    {
        sound = !sound;
    }
    public void PlaySoundFX(AudioClip clip, float volume)
    {
        if (sound)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
    #endregion
}
