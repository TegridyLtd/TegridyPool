using UnityEngine;
using Tegridy.AudioTools;
namespace Tegridy.PoolGame
{
    public class TegridyPoolBall : MonoBehaviour
    {
        public int ballID;

        public AudioClip[] sfx;
        AudioSource source;

        private void Awake()
        {
            source = gameObject.AddComponent<AudioSource>();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Ball") && other.relativeVelocity.sqrMagnitude > 1)
            {
                TegridyAudioTools.PlayOneShot(sfx, source);
            }
        }
    }
}