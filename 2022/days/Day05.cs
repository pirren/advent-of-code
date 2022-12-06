using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;
using advent_of_code_lib.extensions;

namespace advent_of_code_2022.days
{
    [ProblemInfo(5, "Supply Stacks")]
    public class Day05 : SolverBase
    {
        public override object PartOne(string[] _)
        {
            // Part 1: After the rearrangement procedure completes, what crate ends up on top of each stack?
            return GetCargoProcedure().RunProcedures(1).TopSupplies();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: Before the rearrangement process finishes, update your simulation so that the Elves know where they should stand to be ready to unload the final supplies.
            // After the rearrangement procedure completes, what crate ends up on top of each stack?
            return GetCargoProcedure().RunProcedures(2).TopSupplies();
        }

        internal record Instruction(int Amount, int From, int To);

        internal class CargoProcedure
        {
            public List<Stack<char>> SupplyStacks { get; set; } = null!;
            public List<Instruction> Procedures { get; set; } = null!;
            public string TopSupplies() => string.Join("", SupplyStacks.Select(x => x.Peek()));

            public CargoProcedure RunProcedures(int part)
            {
                if (part == 1)
                    foreach (var proc in Procedures) Move(proc.From, proc.To, proc.Amount);
                else if (part == 2)
                    foreach (var proc in Procedures) MoveMany(proc.From, proc.To, proc.Amount);
                else throw new Exception();

                return this;
            }
            private void MoveMany(int from, int to, int amount = 1)
            {
                Stack<char> q = new();
                for (int i = amount; i > 0; i--) q.Push(SupplyStacks[from - 1].Pop());
                while (q.Count > 0) SupplyStacks[to - 1].Push(q.Pop());
            }

            private void Move(int from, int to, int amount = 1) 
                => Enumerable.Range(0, amount).Reverse().ForEach(_ => SupplyStacks[to - 1].Push(SupplyStacks[from - 1].Pop()));
        }

        CargoProcedure GetCargoProcedure()
        {
            var data = GetAllData(Folder).Split("\r\n\r\n");

            var cargoData = data[0].Split("\r\n");
            var instructionData = data[1].Split("\r\n");

            var procedure = new CargoProcedure
            {
                SupplyStacks = new List<Stack<char>>(),
                Procedures = new() 
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
                procedure.Procedures.Add(new Instruction(
                        int.Parse(insdata[0]),
                        int.Parse(insdata[1]),
                        int.Parse(insdata[2])
                    ));
            }

            return procedure;
        }
    }
}
