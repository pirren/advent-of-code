using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using Serilog;
using System.Reflection;

namespace advent_of_code_lib.extensions
{
    public static class CommonExtensions
    {
        public static ProblemInfoAttribute GetProblemInfo(this SolverBase solver)
        {
            var attr = solver.GetType().GetCustomAttribute(typeof(ProblemInfoAttribute));

            if (attr == null || attr is not ProblemInfoAttribute)
            {
                Log.Error($"Solver {solver.GetType().Name} has no ProblemInfo Attribute set.");
                throw new Exception($"Solver {solver.GetType().Name} has no ProblemInfoAttribute set.");
            }

            return (ProblemInfoAttribute)attr;
        }

        /// <summary>
        /// Solver is slow
        /// </summary>
        /// <param name="solver"></param>
        /// <returns></returns>
        public static bool IsSlow(this SolverBase solver)
        {
            var attr = solver.GetType().GetCustomAttribute(typeof(SlowAttribute));
            return attr != null && attr is SlowAttribute;
        }

        /// <summary>
        /// Get null forgiven T objects and skip all nulls
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> enumerable) where T : class
            => enumerable.Where(x => x != null).Select(s => s!);

        /// <summary>
        /// Parse string to int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
            => int.Parse(str);

        /// <summary>
        /// Parse char to int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this char ch)
            => int.Parse(ch.ToString());

        /// <summary>
        /// Parse string to float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(this string str)
            => float.Parse(str);

        /// <summary>
        /// The distance between two points measured along axes at right angles
        /// </summary>
        /// <param name="p"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static int Manhattan(this (int x, int y) p, (int x, int y) other)
            => Math.Abs(other.x - p.x) + Math.Abs(other.y - p.y);

        /// <summary>
        /// Get and remove element from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T Pop<T>(this List<T> list, T target)
        {
            list.Remove(target);
            return target;
        }

        /// <summary>
        /// Get element at given position and remove it from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static T PopAt<T>(this List<T> list, int idx)
        {
            T r = list[idx];
            list.RemoveAt(idx);
            return r;
        }

        /// <summary>
        /// Invokes action on all T objects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source) action(item);
        }

        /// <summary>
        /// Char range from start char to end char
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IEnumerable<char> Range(this char start, char end)
        {
            return Enumerable.Range(start, end - start + 1).Select(i => (char)i);
        }

        /// <summary>
        /// Char range from start char to end char
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static IEnumerable<int> Range(this int start, int end)
        {
            return Enumerable.Range(start, end);
        }

        /// <summary>
        /// Number at position n
        /// </summary>
        /// <param name="value"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static int NumberAt(this int value, int n)
        {
            var result = value / (int)Math.Pow(10, n);
            result %= 10;
            return result;
        }
    }
}
