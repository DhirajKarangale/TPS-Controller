using UnityEngine;

public class Effects : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [Header("PS")]
    [SerializeField] ParticleSystem psFall;
    [SerializeField] ParticleSystem psCollect;

    [Header("Audio")]
    [SerializeField] AudioClip clipFall;
    [SerializeField] AudioClip clipCollect;


    internal void Fall(Vector3 pos)
    {
        audioSource.transform.position = pos;
        psFall.transform.position = pos;

        audioSource.volume = 0.75f;
        audioSource.PlayOneShot(clipFall);
        psFall.Play();
    }

    internal void Collect(Vector3 pos)
    {
        audioSource.transform.position = pos;
        psCollect.transform.position = pos;

        audioSource.volume = 0.35f;
        audioSource.PlayOneShot(clipCollect);
        psCollect.Play();
    }
}