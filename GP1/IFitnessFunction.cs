namespace GP1
{
    public interface IFitnessFunction
    {
        // Fitness such that engine should stop
        double TerminatingFitness { get; }

        string[] Variables { get; }

        double Evaluate(Program program);
    }
}
