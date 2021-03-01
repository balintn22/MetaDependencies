using System.Drawing;

namespace Dgml
{
    // This is code manually added to the genrated DirecedGraphNode

    public partial class DirectedGraphNode
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            DirectedGraphNode other = (DirectedGraphNode)obj;
            return (Id == other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public RectangleF? GetBoundingRect()
        {
            if (string.IsNullOrWhiteSpace(Bounds))
                return null;

            string[] tokens = Bounds.Split(',');
            float left = float.Parse(tokens[0]);
            float top = float.Parse(tokens[1]);
            float width = float.Parse(tokens[2]);
            float height = float.Parse(tokens[3]);

            return new RectangleF(left, top, width, height);
        }

        public void SetBoundingRect(RectangleF rect)
        {
            Bounds = $"{rect.Left},{rect.Top},{rect.Width},{rect.Height}";
        }
    }
}
