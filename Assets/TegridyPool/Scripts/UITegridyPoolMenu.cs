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