using Openworld.InputSystems;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputsHandler : BaseInputs
{
    PlayerController inputActions;
    private void Awake()
    {
        inputActions = new PlayerController();
    }
    public override InputData GenerateInput()
    {
        return new InputData
        {
            movement = inputActions.Movement.Move.ReadValue<Vector2>(),
            run = inputActions.Movement.Run.ReadValue<bool>(),

        };
    }
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
    
}
