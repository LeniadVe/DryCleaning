using DryCleaning.Factories;

namespace DryCleaningTest
{
    internal class FactoryTest
    {

        private Mock<Shop> shopMock;

        [SetUp]
        public void Setup()
        {
            shopMock = new Mock<Shop>();
        }

        [Test]
        public void InitializeWeek_Success()
        {
            // Arrange
            var shop = new Shop();
            var days = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();

            // Act
            shop = shopMock.Object.InitializeWeek();

            // Assert
            Assert.That(shop, Is.Not.Null);
            Assert.That(shop.Week.Keys.All(x => days.Contains(x)), Is.True);
        }        
    }
}
