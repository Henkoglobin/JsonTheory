using FluentAssertions;
using Xunit;

namespace JsonTheory.IntegrationTest {
    public class ComplexTest {
        [Theory]
        [JsonFileData("TestData/ComplexTestData.json")]
        public void NameAndAgeMatch(
            Person person,
            string expectedName,
            int expectedAge
        ) {
            person.Should().NotBeNull();
            expectedName.Should().NotBeNullOrEmpty();
            expectedAge.Should().NotBe(0);

            person.Name.Should().Be(expectedName);
            person.Age.Should().Be(expectedAge);
        }

        [Theory]
        [JsonFileData("TestData/ComplexTestData.json")]
        public void PetDataShouldMatch(
            Person person,
            bool expectedHasPet,
            PetType expectedPetType,
            string expectedPetName
        ) {
            if (expectedHasPet) {
                person.Pet.Should().NotBeNull();

                person.Pet.PetType.Should().Be(expectedPetType);
                person.Pet.Name.Should().Be(expectedPetName);
            } else {
                person.Pet.Should().BeNull();
            }
        }

        #region Model Definition

        public class Person {
            public string Name { get; set; }
            public int Age { get; set; }
            public Pet Pet { get; set; }
        }

        public class Pet {
            public PetType PetType { get; set; }
            public string Name { get; set; }
        }

        public enum PetType {
            Cat,
            Dog,
            Fish
        }

        #endregion
    }
}
