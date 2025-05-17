namespace Aoc2023
{
    public class Polygon
    {
        public List<(int, int)> Coords { get; set; }

        public Polygon(List<(int, int)> polygon)
        {
            Coords = polygon;
        }

        public bool ContainsPoint((int, int) point)
        {
            int n = Coords.Count;
            int inside = 0;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if ((Coords[i].Item2 > point.Item2) != (Coords[j].Item2 > point.Item2) &&
                    (point.Item1 < (Coords[j].Item1 - Coords[i].Item1) * (point.Item2 - Coords[i].Item2) / (Coords[j].Item2 - Coords[i].Item2) + Coords[i].Item1))
                {
                    inside += 1;
                }
            }
            return inside % 2 != 0;
        }
    }
}