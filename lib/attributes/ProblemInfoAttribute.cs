namespace advent_of_code_lib.attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public class ProblemInfoAttribute : Attribute
    {
        public ProblemInfoAttribute(int day, string problemName)
        {
            Day = day;
            ProblemName = problemName;
        }

        public int Day { get; }
        public string ProblemName { get; }
    }
}
