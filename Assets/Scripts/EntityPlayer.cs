using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EntityPlayer: MonoBehaviour
    {

        [SerializeField] 
        private Transform _playerTransform;
        
        private int _score = 0;

        public Transform PlayerTransform => _playerTransform;

        public void Reset()
        {
            _score = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Pellet"))
            {
                _score++;
                Debug.Log(_score);
            }
        }
    }
}