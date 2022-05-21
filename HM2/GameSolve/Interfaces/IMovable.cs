namespace HM2.GameSolve.Interfaces
{
    public interface IMovable
    {
        public HM2.GameSolve.Structures.Vector getPosition();
        public HM2.GameSolve.Structures.Vector setPosition(HM2.GameSolve.Structures.Vector newValue);
        public HM2.GameSolve.Structures.Vector getVelocity();
    }
}

