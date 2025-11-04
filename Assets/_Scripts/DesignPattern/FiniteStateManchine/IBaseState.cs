namespace _Scripts.DesignPattern.FiniteStateManchine
{
    public interface IBaseState 
    {
        void OnEnter();
        void OnExecute();
        void OnExit();
    }
}
