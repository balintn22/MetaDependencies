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
    }
}
