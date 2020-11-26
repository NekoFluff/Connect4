using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private InputActions inputActions;
    [SerializeField] public Board inputHander;

    void Awake()
    {
        this.inputActions = new InputActions();
        this.inputActions.Enable();
        this.inputActions.PlayerActionMap.Movement.performed += OnMovement;
        this.inputActions.PlayerActionMap.Drop.performed += OnDrop;
    }

    void OnMovement(InputAction.CallbackContext context) 
    {
        float movement = context.ReadValue<float>();

        if (movement > 0) {
            inputHander.MoveRight();
        } else if (movement < 0) {
            inputHander.MoveLeft();
        }

    }

    void OnDrop(InputAction.CallbackContext context) 
    {
        float drop = context.ReadValue<float>();

        if (drop > 0) {
            inputHander.Drop();
        }
    }

    void OnEnable()
    {
        this.inputActions.Enable();
    }

    void OnDisable()
    {
        this.inputActions.Disable();
    }
}
