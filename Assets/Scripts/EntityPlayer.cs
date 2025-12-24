using System;
using Oculus.Interaction.Locomotion;
using UnityEngine;
using CharacterController = Oculus.Interaction.Locomotion.CharacterController;

namespace DefaultNamespace
{
    public class EntityPlayer: MonoBehaviour
    {
        
        
        private CharacterController _characterController;
        private FirstPersonLocomotor _locomotor;
        
        
        private int _score = 0;

        private void Awake()
        {
            _locomotor = GetComponent<FirstPersonLocomotor>();
            _characterController = GetComponent<CharacterController>();
        }

        public void SetPosition(Vector3 pos)
        {
            _characterController.SetPosition(pos);
        }

        public void OnGameReset()
        {
            _score = 0;
            _locomotor.ResetPlayerToCharacter();
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