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
