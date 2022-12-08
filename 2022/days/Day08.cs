using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2022.days
{
    [ProblemInfo(8, "Treetop Tree House")]
    public class Day08 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: how many trees are visible from outside the grid?
            return VisibleTrees(Map(data));
            //return VisibleTrees(GetMap(data), data[0].Length, data.Length);
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What is the highest scenic score possible for any tree?
            var map = Map(data);
            return ScenicScores(map).Max();
        }

        private int VisibleTrees(int[][] map)
        {
            int visible = 0;
            
            for (int i = 1; i < map.Length - 1; i++)
            {
                for (int j = 1; j < map[0].Length - 1; j++)
                {
                    var t = map[i][j];
                    if (t == 0) 
                        continue; // there can be no lower

                    if (0.Range(i).All(y => map[y][j] < t))
                        visible++;
                    else if (0.Range(j).All(x => map[i][x] < t))
                        visible++;
                    else if ((i + 1).Range(map.Length - 1 - i).All(y => map[y][j] < t))
                        visible++;
                    else if((j + 1).Range(map[0].Length - 1 - j).All(x => map[i][x] < t))
                        visible++;
                }
            }
            return visible + (map.Length * 2) + (map[1].Length * 2) - 4;
        }

        private HashSet<int> ScenicScores(int[][] map)
        {
            HashSet<int> scores = new();
            for (int i = 1; i < map.Length - 1; i++)
            {
                for (int j = 1; j < map[0].Length - 1; j++)
                {
                    var t = map[i][j];
                    if (t == 0) continue; // there can be no lower

                    int dy = 1, dx = 1, ddy = 1, ddx = 1;
                    for (int y = i - 1; y > 0; y--)
                    {
                        if (map[y][j] >= t) break;
                        dy++;
                    }
                    for (int x = j - 1; x > 0; x--)
                    {
                        if (map[i][x] >= t) break;
                        dx++;
                    }
                    for (int y = i + 1; y < map.Length - 1; y++)
                    {
                        if (map[y][j] >= t) break;
                        ddy++;
                    }
                    for (int x = j + 1; x < map[0].Length - 1; x++)
                    {
                        if (map[i][x] >= t) break;
                        ddx++;
                    }
                    scores.Add(dx * dy * ddx * ddy);
                }
            }
            return scores;
        }

        private int[][] Map(string[] indata) => indata.Select(x => x.Select(ch => (int)char.GetNumericValue(ch)).ToArray()).ToArray();
    }
}
