using advent_of_code_2022.days;
using advent_of_code_lib.bases;
using System;
using System.Collections.Generic;
using Xunit;

namespace advent_of_code.tests
{
    public class UnitTest1
    {
        protected static string indata => nameof(indata);
        protected static string sample => "sample-indata";

        #region Day01
        [Fact]
        public void Day01_1()
        {
            var day = Days[1];
            var result = day!.PartOne(Array.Empty<string>());
            Assert.True((int)result == 72602);
        }

        [Fact]
        public void Day01_2()
        {
            var day = Days[1];
            var result = day!.PartTwo(Array.Empty<string>());
            Assert.True((int)result == 207410);
        }
        #endregion

        #region Day02
        [Fact]
        public void Day02_1()
        {
            var day = Days[2];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((int)result == 13675);
        }

        [Fact]
        public void Day02_2()
        {
            var day = Days[2];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 14184);
        }
        #endregion

        #region Day03
        [Fact]
        public void Day03_1()
        {
            var day = Days[3];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((int)result == 7821);
        }

        [Fact]
        public void Day03_2()
        {
            var day = Days[3];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 2752);
        }
        #endregion

        #region Day04
        [Fact]
        public void Day04_1()
        {
            var day = Days[4];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((int)result == 433);
        }

        [Fact]
        public void Day04_2()
        {
            var day = Days[4];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 852);
        }
        #endregion

        #region Day05
        [Fact]
        public void Day05_1()
        {
            var day = Days[5];
            var result = day!.PartOne(Array.Empty<string>());
            Assert.True(result.ToString() == "SBPQRSCDF");
        }

        [Fact]
        public void Day05_2()
        {
            var day = Days[5];
            var result = day!.PartTwo(Array.Empty<string>());
            Assert.True(result.ToString() == "RGLVRCQSB");
        }
        #endregion

        #region Day06
        [Fact]
        public void Day06_1()
        {
            var day = Days[6];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((int)result == 1598);
        }

        [Fact]
        public void Day06_2()
        {
            var day = Days[6];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 2414);
        }
        #endregion

        #region Day07
        [Fact]
        public void Day07_1()
        {
            var day = Days[7];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((int)result == 1232307);
        }

        [Fact]
        public void Day07_2()
        {
            var day = Days[7];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 7268994);
        }
        #endregion

        #region Day08
        [Fact]
        public void Day08_1()
        {
            var day = Days[8];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((int)result == 1835);
        }

        [Fact]
        public void Day08_2()
        {
            var day = Days[8];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 263670);
        }
        #endregion

        #region Day09
        [Fact]
        public void Day09_1()
        {
            var day = Days[9];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((int)result == 6090);
        }

        [Fact]
        public void Day09_2()
        {
            var day = Days[9];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 2566);
        }
        #endregion

        #region Day10
        [Fact]
        public void Day10_1()
        {
            var day = Days[10];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((int)result == 11720);
        }

        [Fact]
        public void Day10_2()
        {
            var day = Days[10];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((string)result == "ERCREPCJ");
        }
        #endregion

        #region Day11
        [Fact]
        public void Day11_1()
        {
            var day = Days[11];
            var result = day!.PartOne(Array.Empty<string>());
            Assert.True((long)result == 57838);
        }

        [Fact]
        public void Day11_2()
        {
            var day = Days[11];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(Array.Empty<string>());
            Assert.True((long)result == 15050382231);
        }
        #endregion

        #region Day12
        [Fact]
        public void Day12_1()
        {
            var day = Days[12];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((long)result == 0);
        }

        [Fact]
        public void Day12_2()
        {
            var day = Days[12];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 0);
        }
        #endregion

        #region Day14
        [Fact]
        public void Day14_1()
        {
            var day = Days[14];
            var data = day.GetAllLines(indata);
            var result = day!.PartOne(data);
            Assert.True((long)result == 755);
        }

        [Fact]
        public void Day14_2()
        {
            var day = Days[14];
            var data = day.GetAllLines(indata);
            var result = day!.PartTwo(data);
            Assert.True((int)result == 29805);
        }
        #endregion

        protected Dictionary<int, SolverBase> Days = new()
        {
            { 1, new Day01 { Folder = indata } },
            { 2, new Day02 { Folder = indata } },
            { 3, new Day03 { Folder = indata } },
            { 4, new Day04 { Folder = indata } },
            { 5, new Day05 { Folder = indata } },
            { 6, new Day06 { Folder = indata } },
            { 7, new Day07 { Folder = indata } },
            { 8, new Day08 { Folder = indata } },
            { 9, new Day09 { Folder = indata } },
            { 10, new Day10 { Folder = indata } },
            { 11, new Day11 { Folder = indata } },
            { 12, new Day12 { Folder = indata } },
            { 14, new Day14 { Folder = indata } }
        };
    }
}