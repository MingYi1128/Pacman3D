using System;
using Oculus.Interaction.Locomotion;
using UnityEngine;
using CharacterController = Oculus.Interaction.Locomotion.CharacterController;

public class EntityPlayer : MonoBehaviour
{
    public delegate void EventPlayerDamaged();
    public event EventPlayerDamaged OnPlayerDamaged;

    public delegate void EventPlayerEatPellet(EntityPellet pellet);
    public event EventPlayerEatPellet OnPlayerEatPellet;
    

    private CharacterController _characterController;
    private FirstPersonLocomotor _locomotor;
    


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
        _locomotor.ResetPlayerToCharacter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pellet"))
        {
            OnPlayerEatPellet.Invoke(other.GetComponent<EntityPellet>());
        }

        if (other.CompareTag("Enemy"))
        {
            OnPlayerDamaged.Invoke();
        }
    }
}