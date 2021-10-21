using UnityEngine;
using UnityEngine.UI;
namespace Tegridy.PoolGame
{
    public class TegridyPoolCue : MonoBehaviour
    {
        public string cueName;
        public string cueDescription;
        public Sprite cuePic;

        [Header("Cue Config")]
        public float forceMultiplier = 2.5f;
        public float accuracy = 1f;
        public float maxDist = -3;

        [HideInInspector] public float strength;
        [HideInInspector] public float luck;
        [HideInInspector] public float dexterity;

        [HideInInspector] public int playerID = 0;
        [HideInInspector] public bool active = false;

        Transform cue;
        Transform cueBall;
        [HideInInspector] public Image powerBar;

        float dist = 0;
        float minDist = 0;
        float ballRadius = 0;

        AudioSource audioSource;
        Rigidbody2D cueRigid;
        TegridyPool control;
        public void StartUp(TegridyPool con)
        {
            control = con;
            cueBall = control.cueBall.transform;
            cue = this.gameObject.transform;
            cueRigid = cueBall.GetComponent<Rigidbody2D>();
            ballRadius = cueBall.GetComponent<CircleCollider2D>().radius;
            minDist = -(ballRadius + ballRadius / 2);
            dist = Mathf.Clamp(maxDist / 2, maxDist, minDist);
            float value = dist / (maxDist / 100);
            powerBar.fillAmount = value / 100;
            audioSource = GetComponent<AudioSource>();
        }
        void Update()
        {
            if (active)
            {
                //replace with new input system
                if (Input.GetAxis("Mouse ScrollWheel") != 0)
                {
                    dist += Input.GetAxis("Mouse ScrollWheel");
                    dist = Mathf.Clamp(dist, maxDist, minDist);
                    float value = dist / (maxDist / 100);
                    powerBar.fillAmount = value / 100;
                }
                Vector3 mPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));

                cue.rotation = Quaternion.LookRotation(Vector3.forward, mPos - cueBall.position);
                cue.position = cueBall.position;
                cue.localPosition += cue.up * dist;

                if (Input.GetMouseButtonUp(0))
                {
                    HitBall();
                }
            }
        }
        private void HitBall()
        {
            Vector3 forceDir;
            if (control.configUsingStats)
            {
                float shotBonus = (Random.Range(0, 1.0000f) - (dexterity + luck + accuracy / 3) / 100);
                if (shotBonus > 0) { shotBonus = 0; }

                Vector3 standardVector = (cueBall.position - cue.position).normalized * -(dist - minDist - 0.02f) * (forceMultiplier + strength);
                forceDir = new Vector3(standardVector.x + shotBonus, standardVector.y + shotBonus, 0);
            }
            else forceDir = (cueBall.position - cue.position).normalized * -(dist - minDist - 0.02f) * forceMultiplier;
            cueRigid.AddForce(forceDir, ForceMode2D.Impulse);
            audioSource.Play();

            control.TakeShot(playerID);
        }
    }
}