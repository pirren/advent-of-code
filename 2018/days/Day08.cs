using advent_of_code_lib.attributes;
using advent_of_code_lib.bases;

namespace advent_of_code_2018.days
{
    [ProblemInfo(8, "Memory Maneuver")]
    public class Day08 : SolverBase
    {
        public override object PartOne(string[] data)
        {
            // Part 1: What is the sum of all metadata entries?
            var package = DequeueTree(ParseIndata(data[0]));
            return Metadata(package).Sum();
        }

        public override object PartTwo(string[] data)
        {
            // Part 2: What is the value of the root node?
            var package = DequeueTree(ParseIndata(data[0]));
            return RootNodeValue(package);
        }

        public Package DequeueTree(Queue<int> rawData)
        {
            Package packet = new();

            while (rawData.Any())
            {
                packet = DequeueNextPacket(rawData);
            }

            return packet;
        }

        public Package DequeueNextPacket(Queue<int> rawData)
        {
            Package packet = rawData.PackageHeader();

            // ---------------------------
            // first parse subpackets
            for (int i = 0; i < packet.HeaderSubPackets; i++)
            {
                packet.SubPackets.Add(DequeueNextPacket(rawData));
            }

            // ---------------------------
            // get the metadata
            for (int i = 0; i < packet.HeaderMetaEntries; i++)
            {
                packet.MetaData.Add(rawData.Dequeue());
            }

            return packet;
        }

        public int RootNodeValue(Package packet)
        {
            List<int> metaData = new();

            if (!packet.SubPackets.Any())
            {
                metaData.AddRange(packet.MetaData);
            }
            else
            {
                foreach (var data in packet.MetaData)
                {
                    if (packet.SubPackets.ElementAtOrDefault(data - 1) == null)
                        continue;
                    metaData.Add(RootNodeValue(packet.SubPackets[data - 1]));
                }
            }

            return metaData.Sum();
        }

        public List<int> Metadata(Package packet)
        {
            List<int> metaData = new();
            metaData.AddRange(packet.MetaData);

            foreach (var subpack in packet.SubPackets)
            {
                metaData.AddRange(Metadata(subpack));
            }

            return metaData;
        }

        public class Package
        {
            public int HeaderSubPackets { get; set; }
            public int HeaderMetaEntries { get; set; }

            public List<Package> SubPackets = new();
            public List<int> MetaData = new();
        }


        public Queue<int> ParseIndata(string indata)
        {
            Queue<int> queue = new();
            var data = indata.Split(' ').Select(int.Parse).ToList();
            data.ForEach(x => queue.Enqueue(x));
            return queue;
        }
    }

    public static class Ext8
    {
        public static Day08.Package PackageHeader(this Queue<int> rawData)
        {
            return new Day08.Package
            {
                HeaderSubPackets = rawData.Dequeue(),
                HeaderMetaEntries = rawData.Dequeue()
            };
        }
    }
}
