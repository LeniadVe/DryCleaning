using LanguageExt;

namespace DryCleaningTest
{
    internal class ServiceTest
    {
        private Mock<IShopService> shopServiceMock;
        private ShopService shopService;
        WorkHours workingHours;

        [SetUp]
        public void Setup()
        {
            shopServiceMock = new Mock<IShopService>();
            shopService = new ShopService();
            workingHours = new WorkHours { Open = new TimeOnly(9, 0), Close = new TimeOnly(18, 0) };
        }


        [Test]
        public void Get_Valid_ReturnsSuccess()
        {
            // Arrange
            var minutes = 120;
            var dateTime = new DateTime(2024, 11, 8, 10, 0, 0);

            // Act
            var result = shopService.Get(minutes, dateTime);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Match(x => x, e => e.Message), Is.EqualTo("Fri Nov 08 12:00:00 2024"));
        }

        [Test]
        public void Get_Invalid_ReturnsFailure()
        {
            // Arrange
            _ = shopService.UpdateWeek();
            var minutes = 120;
            var dateTime = new DateTime(2024, 11, 8, 10, 0, 0);

            // Act
            var result = shopService.Get(minutes, dateTime);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Match(x => x, e => e.Message), Is.EqualTo("The shop doesn't have opening hours for the indicated date."));
        }

        [Test]
        public void UpdateWeek_Valid_ReturnsSuccess()
        {
            // Arrange

            // Act
            var result = shopService.UpdateWorkingHours(DayOfWeek.Monday, workingHours);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void UpdateWeek_Invalid_ReturnsFailure()
        {
            // Arrange
            shopServiceMock.Setup(s => s.UpdateWorkingHours(It.IsAny<DayOfWeek>(), It.IsAny<WorkHours>()))
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = shopServiceMock.Object.UpdateWeek(workingHours);

            // Assert
            Assert.That(result.IsSuccess, Is.False);
        }

        [Test]
        public void UpdateWorkingHours_Valid_ReturnsSuccess()
        {
            // Arrange

            // Act
            var result = shopService.UpdateWorkingHours(DayOfWeek.Monday, workingHours);

            // Assert
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void UpdateWorkingHours_Invalid_ReturnsFailure()
        {
            // Arrange

            // Act
            var result = shopService.UpdateWorkingHours((DayOfWeek)7, workingHours);

            // Assert
            Assert.That(result.Match(x => x, e => false), Is.False);
        }

        [Test]
        public void UpdateWorkingHours_Object_Valid_ReturnsSuccess()
        {
            // Arrange

            // Act
            var result = shopService.UpdateWorkingHours(DayOfWeek.Monday, workingHours);

            // Assert
            Assert.That(result.Match(x => x, e => false), Is.True);
        }

        [Test]
        public void UpdateWorkingHours_Object_Invalid_ReturnsFailure()
        {
            // Arrange

            // Act
            var result = shopService.UpdateWorkingHours(new KeyValuePair<DayOfWeek, WorkHours>((DayOfWeek)7, workingHours));

            // Assert
            Assert.That(result.Match(x => x, e => false), Is.False);
        }

        [Test]
        public void AddDate_Add_ReturnsSuccess()
        {
            // Arrange
            var service = new ShopService();
            var date = new DateOnly(2024, 11, 8);

            // Act
            var result = service.AddDate(date, workingHours);

            // Assert
            Assert.That(result.IsNull(), Is.False);
            Assert.That(result.Match(x => x != null, e => false), Is.True);
        }

        [Test]
        public void AddDate_Update_ReturnsSuccess()
        {
            // Arrange
            var dates = new List<DateOnly>
            {
                new DateOnly(2024, 11, 8),
                new DateOnly(2024, 11, 9)
            };

            // Act
            var result = shopService.AddDates(dates, workingHours);

            // Assert
            Assert.That(result.IsNull(), Is.False);
            Assert.That(result.Match(x => x, e => false), Is.True);
        }

        [Test]
        public void AddDate_Update_ReturnsFailure()
        {
            // Arrange
            var dates = new List<DateOnly>
            {
                new DateOnly(2024, 11, 8),
                new DateOnly(2024, 11, 9)
            };

            shopServiceMock.Setup(s => s.AddDate(It.IsAny<DateOnly>(), It.IsAny<WorkHours>()))
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = shopServiceMock.Object.AddDates(dates, workingHours);

            // Assert
            Assert.That(result.IsNull(), Is.False);
            Assert.That(result.Match(x => x, e => false), Is.False);
        }

    }
}
