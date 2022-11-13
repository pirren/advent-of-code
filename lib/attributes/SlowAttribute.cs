namespace advent_of_code_lib.attributes
{
    /// <summary>
    /// Mark a Day as slow,
    /// skips problem in Run All mode
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SlowAttribute : Attribute { }
}
