using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopAudio : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;

    [Range(0f, 1f)]public float volume = 1f;

    public AudioClip[] SFXHits;
    AudioSource mSFXSource;

    public AudioClip SFXShoot;
    AudioSource mSFXShootSource;


    public AudioClip SFXHurt;
    AudioSource mSFXHurtSource;

    int mCurrentSFXHit = 0;

    public static LoopAudio Instance;

    void Start()
    {
        Instance = this;

        // Récupérer ou ajouter un composant AudioSource
        audioSource = GetComponent<AudioSource>();

        mSFXSource = gameObject.AddComponent<AudioSource>();

        mSFXShootSource = gameObject.AddComponent<AudioSource>();

        mSFXHurtSource = gameObject.AddComponent<AudioSource>();

        // Assigner l'audio clip
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.loop = true; // Activer la lecture en boucle
            audioSource.Play(); // Commencer la lecture
        }
        else
        {
            Debug.LogError("Aucun fichier audio assigné au script !");
        }
    }

    public void PlaySFXHit()
    {
        if (SFXHits.Length > 0)
        {
            mSFXSource.volume = 0.7f;

            mSFXSource.PlayOneShot(SFXHits[mCurrentSFXHit]);
            mCurrentSFXHit++;
            if (mCurrentSFXHit >= SFXHits.Length)
            {
                mCurrentSFXHit = 0;
            }
        }
    }

    public void PlaySFXShoot()
    {
        mSFXShootSource.volume = 0.5f;
        mSFXShootSource.pitch = Random.Range(0.5f, 0.8f);
        mSFXShootSource.PlayOneShot(SFXShoot);
    }

    public void PlaySFXHurt()
    {
        mSFXHurtSource.volume = 0.5f;
        mSFXHurtSource.pitch = Random.Range(1.5f, 1.7f);
        mSFXHurtSource.PlayOneShot(SFXHurt);
    }

    private void Update()
    {
        if(audioSource.volume != volume)
        {
            audioSource.volume = volume;
        }
    }

    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PlayAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}