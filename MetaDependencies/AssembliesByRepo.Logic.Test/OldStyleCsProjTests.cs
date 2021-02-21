using AssInfo.Logic.Space;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml.Serialization;

namespace AssInfo.Logic.Test
{
    [TestClass]
    public class OldStyleCsProjTests
    {
        [TestMethod]
        [DeploymentItem("OldStyleCsProjSample.xml")]
        public void AssInfoFrom_HappyCase()
        {
            ProjInfo result = OldStyleCsProj.ProjInfoFrom("OldStyleCsProjSample.xml");

            result.AssName.Should().Be("AssembliesByRepo");
            result.AssType.Should().Be(ProjInfo.AssTypes.Exe);
            result.CsProjPath.Should().Be("OldStyleCsProjSample.xml");
        }

        #region Deserialization tests

        [TestMethod]
        [DeploymentItem("OldStyleCsProjSample.xml")]
        public void ParseOldStyleCsProj_HappyCase()
        {
            using (var fileStream = File.Open("OldStyleCsProjSample.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OldStyleCsProj.Project));
                var parsed = (OldStyleCsProj.Project)serializer.Deserialize(fileStream);
            }
        }

        [TestMethod]
        [DeploymentItem("SdkStyleCsProjSample.xml")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseOldStyleCsProj_ShouldFail_WhenParsingSdkStyle()
        {
            using (var fileStream = File.Open("SdkStyleCsProjSample.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OldStyleCsProj.Project));
                var parsed = (OldStyleCsProj.Project)serializer.Deserialize(fileStream);
            }
        }

        #endregion Deserialization tests
    }
}
