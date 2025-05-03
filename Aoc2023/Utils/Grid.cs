namespace Aoc2023
{
    public class Grid
    {

        public Dictionary<(int, int), char> gridMap;
        public int rows;
        public List<int> cols;
        public (int, int)[] dxdy;  // directions
        public Grid((int, int)[] dxdy)
        {
            this.rows = 0;
            this.cols = [];
            this.gridMap = new Dictionary<(int, int), char> { };
            this.dxdy = dxdy;
        }

        public void Create(List<string> input)
        {

            this.rows = input.Count;

            for (int i = 0; i < rows; i++)
            {
                this.cols.Add(input[i].Length);

                for (int j = 0; j < cols[i]; j++)
                {
                    this.gridMap[(i, j)] = input[i][j];
                }
            }

        }

        public List<char> NeighborsOfChar((int, int) loc)
        {
            List<char> neighbors = [];

            for (int i = 0; i < dxdy.Length; i++)
            {
                int dx = loc.Item1 + dxdy[i].Item1;
                int dy = loc.Item2 + dxdy[i].Item2;

                if (dx < 0 || dy < 0)
                {
                    continue;
                }

                if (dx >= this.rows || dy >= this.cols[dx])
                {
                    continue;
                }

                neighbors.Add(this.gridMap[(dx, dy)]);
            }

            return neighbors;
        }
        public List<(int, int)> NeighborsOfCoord((int, int) loc)
        {
            List<(int, int)> neighbors = [];

            for (int i = 0; i < dxdy.Length; i++)
            {
                int dx = loc.Item1 + dxdy[i].Item1;
                int dy = loc.Item2 + dxdy[i].Item2;

                if (dx < 0 || dy < 0)
                {
                    continue;
                }

                if (dx >= this.rows || dy >= this.cols[dx])
                {
                    continue;
                }

                neighbors.Add((dx, dy));
            }

            return neighbors;
        }

        public List<(int, int, (int, int))> NeighborsOfCoordWithDirection((int, int, (int, int)) loc)
        {
            List<(int, int, (int, int))> neighbors = [];

            int dx = loc.Item1 + loc.Item3.Item1;
            int dy = loc.Item2 + loc.Item3.Item2;

            if (dx < 0 || dy < 0 || dx >= this.rows || dy >= this.cols[dx])
            {
                return [];
            }

            neighbors.Add((dx, dy, (loc.Item3.Item1, loc.Item3.Item2)));
            return neighbors;
        }
    }


}