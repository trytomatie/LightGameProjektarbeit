using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class WeakenEnemies : MonoBehaviour
    {

        private void Start()
        {
            Destroy(gameObject, 1f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<StatusManager>() != null)
            {
                StatusManager sm = other.GetComponent<StatusManager>();
                if(sm.faction == StatusManager.Faction.Enemy)
                {
                    sm.Hp = 3;
                    sm.maxHp = 3;
                }
            }
        }
    }
}