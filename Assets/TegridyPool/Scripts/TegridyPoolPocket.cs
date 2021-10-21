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