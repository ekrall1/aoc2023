namespace Aoc2023
{
    public class Grid
    {

        public Dictionary<(int, int), char> gridMap;
        public Dictionary<(int, int), string> stringGridMap;
        public int rows;
        public List<int> cols;
        public (int, int)[] dxdy;  // directions
        public Grid((int, int)[] dxdy)
        {
            this.rows = 0;
            this.cols = [];
            this.gridMap = new Dictionary<(int, int), char> { };
            this.stringGridMap = new Dictionary<(int, int), string> { };
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

        public IEnumerable<DijkstraGraph.NodeState> NeighborsOfCoordDay17(DijkstraGraph.NodeState loc, int min, int max)
        {
            if (loc.X == 0 && loc.Y == 0 && loc.DX == 0 && loc.DY == 0 && loc.Steps == 0)
            {
                var directions = new[] { (1, 0), (0, 1) };

                foreach (var (dx, dy) in directions)
                {
                    int newX = loc.X + dx;
                    int newY = loc.Y + dy;

                    if (newX < rows && newY < cols[newX])
                    {
                        yield return new DijkstraGraph.NodeState(X: newX, Y: newY, DX: dx, DY: dy, Steps: 1);
                    }
                }
                yield break;
            }

            if (loc.Steps < max)
            {
                foreach (var d in new[] { (loc.DX, loc.DY) })
                {

                    int dx = loc.X + d.Item1;
                    int dy = loc.Y + d.Item2;

                    if (dx < 0 || dy < 0)
                        continue;

                    if (dx >= this.rows || dy >= this.cols[dx])
                        continue;

                    var newNeighbor = new DijkstraGraph.NodeState(X: dx, Y: dy, DX: loc.DX, DY: loc.DY, Steps: loc.Steps + 1);
                    yield return newNeighbor;
                }
            }
            if (loc.Steps >= min)
            {
                foreach (var d in new[] { (-loc.DY, loc.DX), (loc.DY, -loc.DX) })
                {
                    int dx = loc.X + d.Item1;
                    int dy = loc.Y + d.Item2;

                    if (dx < 0 || dy < 0)
                        continue;

                    if (dx >= this.rows || dy >= this.cols[dx])
                        continue;

                    var newNeighbor = new DijkstraGraph.NodeState(X: dx, Y: dy, DX: d.Item1, DY: d.Item2, Steps: 1);
                    yield return newNeighbor;
                }
            }

        }
    }


}