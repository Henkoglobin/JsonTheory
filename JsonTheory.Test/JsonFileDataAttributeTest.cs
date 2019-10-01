using System.IO;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace JsonTheory.Test {
	public class JsonFileDataAttributeTest {
		private readonly MethodInfo methodInfo = typeof(SampleClass)
			.GetMethod(nameof(SampleClass.DoSomething));

		[Fact]
		public void UnusedFieldsAreIgnored() {
			var attribute = new JsonFileDataAttribute("TestData/UnusedFields.json");

			var testData = attribute.GetData(methodInfo)
				.ToList();

			testData.Should().NotBeEmpty();

			foreach (var testDataLine in testData) {
				testDataLine.Should()
					.HaveCount(2, $"Method {nameof(SampleClass.DoSomething)} has exactly two parameters.");

				foreach (var parameter in testDataLine) {
					parameter.Should().NotBeNull();
				}
			}
		}

		[Fact]
		public void ProducesMeaningfulErrorMessageIfFileIsNotFound() {
			var attribute = new JsonFileDataAttribute("DoesNotExist.json");

			attribute.Invoking(a => a.GetData(methodInfo).ToList())
				.Should()
				.Throw<FileNotFoundException>()
				.WithMessage("*DoesNotExist.json*")
				.WithMessage("*Copy Always*")
				.WithMessage("*Properties*");
		}

		private class SampleClass {
			public void DoSomething(string something, int someInteger) {
				// no-op
			}
		}
	}
}
