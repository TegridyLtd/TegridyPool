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

using UnityEngine;
using Tegridy.AudioTools;
using Tegridy.Tools;
namespace Tegridy.PoolGame
{
    public class TegridyPoolPocket : MonoBehaviour
    {
        TegridyPool control;

        public AudioClip[] sfxPotted;
        public AudioClip[] sfxCue;

        AudioSource source;
        private void Awake()
        {
            control = FindObjectOfType<TegridyPool>();
            source = gameObject.AddComponent<AudioSource>();
        }
        void OnTriggerStay2D(Collider2D other)
        {
            if (other.GetComponent<TegridyPoolBall>() != null)
            {
                control.pottedlist.Add(other.gameObject.GetComponent<TegridyPoolBall>().ballID);
                TegridyAudioTools.PlayOneShot(sfxPotted, source);
                Destroy(other.gameObject);
            }
            else if (other.GetComponent<TegridyPoolCueBall>() != null)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                control.pottedlist.Add(99);
                TegridyAudioTools.PlayOneShot(sfxCue, source);
                other.SetActive(false);
            }
        }
    }
}