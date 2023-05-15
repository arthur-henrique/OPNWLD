using UnityEngine;
namespace Openworld.InputSystems
{
    public struct InputData
    {
        public Vector2 movement;
        public bool run;
        public bool jump;
        public bool attack;
        public bool aim;
    }

    public interface IInput
    {
        InputData GenerateInput();
    }

    public abstract class BaseInputs : MonoBehaviour, IInput
    {
        public abstract InputData GenerateInput();
    }
}
