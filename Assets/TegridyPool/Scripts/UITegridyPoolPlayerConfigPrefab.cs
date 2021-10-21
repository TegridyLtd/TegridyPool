using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace Tegridy.PoolGame
{
    public class UITegridyPoolPlayerConfigPrefab : MonoBehaviour
    {
        public TextMeshProUGUI playerName;
        public TextMeshProUGUI playerDescription;
        public Button playerChange;
        public Button cueChange;

        [Header("player fill images")]
        public Image playerCharacter;
        public Image playerStrength;
        public Image playerDexterity;
        public Image playerLuck;

        [Header("Cue fill images")]
        public TextMeshProUGUI cueName;
        public TextMeshProUGUI cueDescription;
        public Image cue;
        public Image cueAccuracy;
        public Image cueForce;
    }
}
