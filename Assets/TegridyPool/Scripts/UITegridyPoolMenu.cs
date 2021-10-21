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
using UnityEngine.UI;
using TMPro;
namespace Tegridy.PoolGame
{
    public class UITegridyPoolMenu : MonoBehaviour
    {
        [Header("Main Menu")]
        public Image mainMenuScreen;
        public TextMeshProUGUI title;
        public Button startLocal;
        public Button back;

        [Header("SinglePlayer")]
        public Image tableSelectScreen;
        public TextMeshProUGUI tableSelectTitle;
        public Button tableSelectback;
        public RectTransform scrollContent;
        public UITegridyPoolTablePrefab tablePrefab;
        public int prefabSpacing;

        [Header("Player Setup")]
        public Image configScreen;
        public TextMeshProUGUI configTitle;
        public Button configBack;
        public Button configStart;
        public RectTransform configScrollContent;
        public UITegridyPoolPlayerConfigPrefab configPreb;
        public int configPrefabSpacing;
    }
}