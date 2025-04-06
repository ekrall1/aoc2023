namespace Aoc2023
{
    public class Grid
    {

        public Dictionary<(int, int), char> gridMap;
        public int rows;
        public List<int> cols;
        public Grid()
        {
            this.rows = 0;
            this.cols = [];
            this.gridMap = new Dictionary<(int, int), char> { };
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

        public List<char> NeighborsOf((int, int) loc)
        {
            (int, int)[] dxdy = [(0, 1), (1, 0), (-1, 0), (0, -1), (1, 1), (-1, -1), (1, -1), (-1, 1)];
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
    }

}

