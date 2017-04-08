using System.Collections.Generic;
using System.Linq;

namespace Fody.StandAlone
{
    public class InnerWeaverWrapper
    {
        private readonly string _assemblyPath;

        public InnerWeaverWrapper(string assemblyPath)
        {
            this._assemblyPath = assemblyPath;
        }

        public void Weave(WeavingParameters weavingParameters)
        {
            var entries = weavingParameters.WeaverEntries.ToList();
            WeaveImpl(entries, weavingParameters.References);
        }

        private void WeaveImpl(List<WeaverEntry> entries, string references)
        {
            var innerWeaver = new InnerWeaver()
            {
                Logger = new Logger(),
                AssemblyFilePath = _assemblyPath,
                Weavers = entries,
                References = references
            };

            innerWeaver.Execute();
        }
    }
}