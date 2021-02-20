using AssembliesByRepo.Logic.Space;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Xml.Serialization;

namespace AssembliesByRepo.Logic.Test
{
    [TestClass]
    public class SdkStyleCsProjTests
    {
        [TestMethod]
        [DeploymentItem("SdkStyleCsprojSample.xml")]
        public void AssInfoFrom_HappyCase()
        {
            AssInfo result = SdkStyleCsProj.AssInfoFrom("SdkStyleCsprojSample.xml");

            result.AssName.Should().Be("AssembliesByRepo.Logic.Name");
            result.AssType.Should().Be(AssInfo.AssTypes.Dll);
            result.CsProjPath.Should().Be("SdkStyleCsprojSample.xml");
        }

        #region Deserialization tests

        [TestMethod]
        [DeploymentItem("SdkStyleCsprojSample.xml")]
        public void ParseSdkStyleCsProj_HappyCase()
        {
            using (var fileStream = File.Open("SdkStyleCsprojSample.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SdkStyleCsProj.Project));
                var parsed = (SdkStyleCsProj.Project)serializer.Deserialize(fileStream);
            }
        }

        [TestMethod]
        [DeploymentItem("OldStyleCsprojSample.xml")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseSdkStyleCsProj_ShouldFail_WhenParsingOldStyle()
        {
            using (var fileStream = File.Open("OldStyleCsprojSample.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SdkStyleCsProj.Project));
                var parsed = (SdkStyleCsProj.Project)serializer.Deserialize(fileStream);
            }
        }

        #endregion Deserialization tests
    }
}
