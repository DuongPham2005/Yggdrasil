using UnityEngine;

public class ZoneAudioTrigger : MonoBehaviour
{
    public AudioSource zoneAudio;
    public float targetVolume = 1f;
    public float fadeSpeed = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        while (zoneAudio.volume < targetVolume)
        {
            zoneAudio.volume += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        zoneAudio.volume = targetVolume;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        while (zoneAudio.volume > 0f)
        {
            zoneAudio.volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        zoneAudio.volume = 0f;
    }
}
