using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Sdk;

namespace JsonTheory {
    public class JsonFileDataAttribute : DataAttribute {
        public string FileName { get; }

        public JsonFileDataAttribute(string fileName) {
            FileName = fileName;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod) {
            var parameters = testMethod
                .GetParameters()
                .Select(param => (param.Name, param.ParameterType))
                .ToList();

            var fileInfo = new FileInfo(FileName);

            if (!fileInfo.Exists) {
                throw new FileNotFoundException(
                    $"Supplied file {FileName} could not be found. Make sure that its output type is set to 'Copy Always' in the Properties pane."
                );
            }

            using (var reader = new StreamReader(fileInfo.OpenRead())) {
                var invocations = JArray.Load(new JsonTextReader(reader));
                foreach (var invocation in invocations) {
                    yield return parameters
                        .Select(param => invocation[param.Name]?.ToObject(param.ParameterType))
                        .ToArray();
                }
            }
        }
    }
}
