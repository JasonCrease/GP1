namespace GP1
{
    public interface IFitnessFunction
    {
        string[] Variables { get; }
        double Evaluate(Program program);
    }
}
