using System.Collections;
using UnityEngine;
namespace Tegridy.AudioTools 
{
    public class TegridyAudioTools
    {
        public static void PlayClip(AudioClip clip, AudioSource source)
        {
            if (source.isPlaying == false)
            {
                source.clip = clip;
                source.Play();
            }
        }
        public static void PlayRandomClip(AudioClip[] clip, AudioSource source)
        {
            if (clip.Length > 0 && source.isPlaying == false)
            {
                source.clip = clip[Random.Range(0, clip.Length)];
                source.Play();
            }
        }
        public static void PlayRandomClipAnyway(AudioClip[] clip, AudioSource source)
        {
            if (clip.Length > 0)
            {
                source.clip = clip[Random.Range(0, clip.Length)];
                source.Play();
            }
        }
        public static void PlayOneShot(AudioClip[] clip, AudioSource source)
        {
            if (clip.Length > 0)
            {
                source.PlayOneShot(clip[Random.Range(0, clip.Length)]);
            }
        }
        public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
        {
            float startVolume = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
        }
        public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
        {
            float startVolume = 0.2f;
            while (audioSource.volume < 1.0f)
            {
                audioSource.volume += startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
        }
    }
}
