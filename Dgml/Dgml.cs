namespace Dgml
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

    public partial class DirectedGraphLink
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            DirectedGraphLink other = (DirectedGraphLink)obj;
            return (Source == other.Source) && (Target == other.Target);
        }

        public override int GetHashCode()
        {
            return $"{Source}-{Target}".GetHashCode();
        }
    }
}
