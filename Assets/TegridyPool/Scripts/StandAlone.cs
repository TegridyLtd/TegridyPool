using UnityEngine;
namespace Tegridy.PoolGame
{
    public class StandAlone : MonoBehaviour
    {
        public UITegridyPoolMenu gui;
        void Start()
        {
            TegridyPool pool = FindObjectOfType<TegridyPool>();
            pool.StartUp();
            pool.OpenMainMenu(gui, null);
        }
    }
}