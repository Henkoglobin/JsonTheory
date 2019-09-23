using FluentAssertions;
using Xunit;

namespace JsonTheory.IntegrationTest {
    public class CalculatorTest {
        [Theory]
        [JsonFileData("TestData/CalculatorTestData.json")]
        public void AdditionWorks(int operand1, int operand2, int expected) {
            (operand1 + operand2)
                .Should().Be(expected);
        }
    }
}
