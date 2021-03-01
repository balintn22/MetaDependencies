using System;
using System.Drawing;

namespace Dgml
{
    public partial class DirectedGraph
    {
        public RectangleF? GetBoundingRect()
        {
            if (Nodes is null || Nodes.Length == 0)
                return null;

            float? left = null;
            float? top = null;
            float? right = null;
            float? bottom = null;

            foreach (var node in Nodes)
            {
                RectangleF? maybeNodeRect = node.GetBoundingRect();
                if (maybeNodeRect is null)
                    continue;

                RectangleF nodeRect = (RectangleF)maybeNodeRect;

                left = left is null ? nodeRect.Left : Math.Min((float)left, nodeRect.Left);
                top = top is null ? nodeRect.Top : Math.Min((float)top, nodeRect.Top);
                right= right is null ? nodeRect.Right : Math.Max((float)right, nodeRect.Right);
                bottom = bottom is null ? nodeRect.Bottom : Math.Max((float)bottom, nodeRect.Bottom);
            }

            if (left is null || top is null || right is null || bottom is null)
                return null;

            return new RectangleF((float)left, (float)top, (float)(right - left), (float)(bottom - top));
        }
    }
}
