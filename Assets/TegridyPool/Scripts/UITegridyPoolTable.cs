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
