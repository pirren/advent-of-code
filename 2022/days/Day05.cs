using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2022.days
{
    [ProblemInfo(5, "Supply Stacks")]
    public class Day05 : SolverBase
    {
        public override object PartOne(string[] _)
        {
            // Part 1: After the rearrangement procedure completes, what crate ends up on top of each stack?
            return GetCargoProcedure().Move(CrateMover.Series9000).TopSupplies();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Before the rearrangement process finishes, update your simulation so that the Elves know where they should stand to be ready to unload the final supplies.
            // After the rearrangement procedure completes, what crate ends up on top of each stack?
            return GetCargoProcedure().Move(CrateMover.Series9001).TopSupplies();
        }

        internal enum CrateMover
        {
            Series9000,
            Series9001
        };

        internal record Instruction(int Amount, int From, int To);

        internal class CargoProcedure
        {
            public List<Stack<char>> SupplyStacks { get; set; } = null!;
            public List<Instruction> Instructures { get; set; } = null!;
            public string TopSupplies() => string.Join("", SupplyStacks.Select(x => x.Peek()));

            public CargoProcedure Move(CrateMover movertype)
            {
                foreach(var proc in Instructures) 
                    RunProcedure(proc.From, proc.To, proc.Amount, movertype == CrateMover.Series9001);

                return this;
            }

            private void RunProcedure(int from, int to, int amount = 1, bool movechunks = false)
            {
                if(movechunks)
                {
                    Stack<char> q = new();
                    for (int i = amount; i > 0; i--) q.Push(SupplyStacks[from - 1].Pop());
                    while(q.Count > 0) SupplyStacks[to - 1].Push(q.Pop());
                    return;
                }

                for (int i = amount; i > 0; i--) SupplyStacks[to - 1].Push(SupplyStacks[from - 1].Pop());
            }
        }

        CargoProcedure GetCargoProcedure()
        {
            var data = GetAllData(Folder).Split("\r\n\r\n");

            var cargoData = data[0].Split("\r\n");
            var instructionData = data[1].Split("\r\n");

            var procedure = new CargoProcedure
            {
                SupplyStacks = new List<Stack<char>>(),
                Instructures = new() 
            };

            for (var i = 0; i < cargoData[^1].Length; i++) procedure.SupplyStacks.Add(new Stack<char>());

            for (var i = cargoData.Length - 1; i >= 0; i--) // reverse loop because stack
            {
                var line = cargoData[i];
                for (var j = 0; j < line.Length; j++)
                    if (line[j] != ' ') procedure.SupplyStacks[j].Push(line[j]);
            }

            for (int i = 0; i < instructionData.Length; i++)
            {
                var insdata = instructionData[i].Split();
                procedure.Instructures.Add(new Instruction(
                        int.Parse(insdata[0]),
                        int.Parse(insdata[1]),
                        int.Parse(insdata[2])
                    ));
            }

            return procedure;
        }
    }
}
