using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EntityPellet: MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Destroy(this.gameObject, 0.1f);
            }
        }
    }
}