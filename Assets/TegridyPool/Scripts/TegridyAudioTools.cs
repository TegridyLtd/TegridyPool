/////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2021 Tegridy Ltd                                          //
// Author: Darren Braviner                                                 //
// Contact: db@tegridygames.co.uk                                          //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// This program is free software; you can redistribute it and/or modify    //
// it under the terms of the GNU General Public License as published by    //
// the Free Software Foundation; either version 2 of the License, or       //
// (at your option) any later version.                                     //
//                                                                         //
// This program is distributed in the hope that it will be useful,         //
// but WITHOUT ANY WARRANTY.                                               //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////
//                                                                         //
// You should have received a copy of the GNU General Public License       //
// along with this program; if not, write to the Free Software             //
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,              //
// MA 02110-1301 USA                                                       //
//                                                                         //
/////////////////////////////////////////////////////////////////////////////

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
