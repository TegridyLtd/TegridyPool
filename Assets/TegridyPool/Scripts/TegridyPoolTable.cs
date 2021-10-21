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
namespace Tegridy.PoolGame
{
    public class TegridyPoolTable : MonoBehaviour
    {
        public string tableName;
        public string tableDescription;
        public Sprite tablePic;
        public UITegridyPoolTable tableUI;
        public int players;
        public Transform cueBall;
        public GameObject ballHolder;
        public GameObject pocketHolder;

        [Header("Audio")]
        public AudioClip[] ballHit;
        public AudioClip[] tableMusic;
        public AudioClip[] ballPocket;
        public AudioClip[] ballWall;

        public AudioClip[] foul;


        public AudioClip[] lost;
        public AudioClip[] won;

        public AudioClip[] playerChanged;
    }
}
