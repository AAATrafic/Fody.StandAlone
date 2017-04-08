using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NUnit.Framework;

namespace Fody.StandAlone.Tests
{
    public class InnerWeaverWrapperTest
    {
        [Test]
        public void WeaveTest()
        {
            var weavedAssemblyPath = GetAssemblyPath();
            WeavingParameters weavingParameters;
            using (var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Fody.StandAlone.Tests.FodyWeavers.xml"))
            {
                weavingParameters = WeavingParameters.GetWeavingParameters(manifestResourceStream, GetDirectoryName(), @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2\System.dll;");
            }

            new InnerWeaverWrapper(weavedAssemblyPath).Weave(weavingParameters);

            var weavedAssembly = Assembly.LoadFile(weavedAssemblyPath);
            var weavedType = weavedAssembly.GetTypes().First(type => type.Name == "ClassToInstrument");
            var weavedInstance = Activator.CreateInstance(weavedType);

            weavedType.GetProperty("MyProperty").SetValue(weavedInstance, 3.ToString());

            var toString = weavedInstance.ToString();

            Assert.That(toString, Is.EqualTo("{T: \"ClassToInstrument\", MyProperty: \"3\"}"));
        }

        private static string GetAssemblyPath()
        {
            var directoryName = GetDirectoryName();

            return Path.Combine(directoryName, "AssemblyToInstrument.dll");
        }

        private static string GetDirectoryName()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
