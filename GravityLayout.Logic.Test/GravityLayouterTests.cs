using Dgml;
using GravityLayout.Logic.Physics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GravityLayout.Logic.Test.Physics
{
    [TestClass]
    public class GravityLayouterTests
    {
        [TestMethod]
        [DeploymentItem("DgmlWithBounds.dgml")]
        public void Layout_ForDgmlWithBounds_RunsWithoutErrors()
        {
            var repo = new DgmlRepo();
            DirectedGraph graph = repo.Load("DgmlWithBounds.dgml");

            var layouter = new GravityLayouter(100, 0.01, Rope.Characteristics.Linear, 1);
            layouter.Layout(graph);
            
            repo.Save(graph, @"c:\balint\waste\output.dgml");
        }

        [TestMethod]
        [DeploymentItem("DgmlWithoutBounds.dgml")]
        public void Layout_ForDgmlWithoutBounds_RunsWithoutErrors()
        {
            var repo = new DgmlRepo();
            DirectedGraph graph = repo.Load("DgmlWithoutBounds.dgml");
        }
    }
}
