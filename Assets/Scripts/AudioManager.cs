using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    #region Singleton
    private void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
            Destroy(gameObject);

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public void PlayAudioOnce(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}


