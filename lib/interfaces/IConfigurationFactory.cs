namespace advent_of_code_lib.interfaces
{
    public interface IConfigurationFactory
    {
        T Build<T>() where T : class;
    }
}
