using System.Drawing;

namespace Dgml
{
    public static class DirectedGraphNodeExtensions
    {
        public static RectangleF? GetBoundingRect(this DirectedGraphNode self)
        {
            if (string.IsNullOrWhiteSpace(self.Bounds))
                return null;

            string[] tokens = self.Bounds.Split(',');
            float left = float.Parse(tokens[0]);
            float top = float.Parse(tokens[1]);
            float width = float.Parse(tokens[2]);
            float height = float.Parse(tokens[3]);

            return new RectangleF(left, top, width, height);
        }

        public static void SetBoundingRect(this DirectedGraphNode self, RectangleF rect)
        {
            self.Bounds = $"{rect.Left},{rect.Top},{rect.Width},{rect.Height}";
        }

        /// <summary>Mutates a node by shifting its bounding rect.</summary>
        public static void Shift(this DirectedGraphNode self, double dx, double dy)
        {
            RectangleF nodeBounds = self.GetBoundingRect() ?? new RectangleF(0, 0, 0, 0);
            nodeBounds.X += (float)dx;
            nodeBounds.Y += (float)dy;
            self.SetBoundingRect(nodeBounds);
            self.UseManualLocation = "True";
        }
    }
}
