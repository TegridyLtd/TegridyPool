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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Tegridy.PoolGame
{
    [System.Serializable]
    public class PlayerHolder
    {
        public TextMeshProUGUI playerName;
        public Image charPic;
        public TextMeshProUGUI turnsTotal;
        public TextMeshProUGUI turnsLeft;
        public TextMeshProUGUI fouls;
        public TextMeshProUGUI ballsRemain;

        public Image playerTurn;
        public TextMeshProUGUI playerTurnText;


        public Image playerPowerBar;
        public List<Image> remainingBalls;
    }

    public class UITegridyPoolTable : MonoBehaviour
    {
        public TextMeshProUGUI title;
        public Button exit;

        [Header("FoulMessage")]
        public Image takingShot;
        public TextMeshProUGUI takingShotText;

        [Header("FoulMessage")]
        public Image foulScreen;
        public TextMeshProUGUI foulTitle;
        public TextMeshProUGUI foulDescription;
        public PlayerHolder[] playerOverlays;
    }
}
