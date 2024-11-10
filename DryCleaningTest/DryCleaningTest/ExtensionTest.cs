using DryCleaning.Extensions;

namespace DryCleaningTest
{
    internal class ExtensionTest
    {
        [Test]
        public void DateExtensions_ToDateAndHours_Success()
        {
            // Arrange
            var dateTime = new DateTime(2024, 11, 8, 10, 0, 0); 
            var date = new DateOnly(2024, 11, 8);
            var time = new TimeOnly(10,0);

            // Act
            var result = dateTime.ToDateAndHours();

            // Assert
            Assert.That(result,Is.EqualTo((date,time)));
        }
    }
}
