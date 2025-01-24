using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopAudio : MonoBehaviour
{
    public AudioClip audioClip; // Le fichier audio WAV à lire
    private AudioSource audioSource;

    void Start()
    {
        // Récupérer ou ajouter un composant AudioSource
        audioSource = GetComponent<AudioSource>();

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