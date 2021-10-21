using UnityEngine;
namespace Tegridy.PoolGame
{
    public class TegridyPoolCueBall : MonoBehaviour
    {
        // Start is called before the first frame update
        TegridyPool control;
        private void Awake()
        {
            control = FindObjectOfType<TegridyPool>();
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Ball") && other.relativeVelocity.sqrMagnitude > 1)
            {
                control.touchedList.Add(other.gameObject.GetComponent<TegridyPoolBall>().ballID);
            }
        }
    }
}
