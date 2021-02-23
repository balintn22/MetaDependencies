namespace MergeGraphs.Logic
{
    public partial class Dgml
    {
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
        }
    }
}
