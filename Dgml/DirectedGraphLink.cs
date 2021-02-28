namespace Dgml
{
    // This is code manually added to the genrated DirecedGraphLink

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
