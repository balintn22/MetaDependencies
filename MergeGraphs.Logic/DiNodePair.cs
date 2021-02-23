namespace MergeGraphs.Logic
{
    /// <summary>
    /// Represents a directed pair of nodes.
    /// </summary>
    public class DiNodePair
    {
        public string StartId;
        public string EndId;

        public DiNodePair(string startId, string endId)
        {
            StartId = startId;
            EndId = endId;
        }

        public override string ToString()
        {
            return $"{StartId} --> {EndId}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != GetType())
                return false;
            DiNodePair other = (DiNodePair)obj;
            return (StartId == other.StartId) && (EndId == other.EndId);
        }

        public override int GetHashCode()
        {
            return $"{StartId}_{EndId}".GetHashCode();
        }
    }
}
