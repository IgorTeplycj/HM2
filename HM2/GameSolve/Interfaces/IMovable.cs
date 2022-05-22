namespace HM2.GameSolve.Interfaces
{
    public interface IMovable
    {
        HM2.GameSolve.Structures.Vector getPosition();
        HM2.GameSolve.Structures.Vector setPosition(HM2.GameSolve.Structures.Vector newValue);
        HM2.GameSolve.Structures.Vector getVelocity();
    }
}

