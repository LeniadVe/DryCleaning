using System.Net;

namespace DryCleaningTest
{
    internal class ControllerTest
    {
        private ShopService shopService;
        private ShopController controller;
        [SetUp]
        public void Setup()
        {
            shopService = new ShopService();
            controller = new ShopController(shopService);
        }

        [Test]
        public void DaysSchedule_ReturnOk()
        {
            //Arrange
            var openingHour = "09:00";
            var closingHour = "19:00";

            //Act
            var result = controller.DaysSchedule(openingHour, closingHour) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(result.Value, Is.EqualTo("Days schedule updated successfully."));
        }

        [Test]
        public void DaysSchedule_OpeningHour_ReturnFailure()
        {
            //Arrange
            var openingHour = "09:61";
            var closingHour = "19:00";

            //Act
            var result = controller.DaysSchedule(openingHour, closingHour) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("incorrect"));
        }

        [Test]
        public void DaysSchedule_ClosingHour_ReturnFailure()
        {
            //Arrange
            var openingHour = "09:00";
            var closingHour = "19:60";

            //Act
            var result = controller.DaysSchedule(openingHour, closingHour) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("incorrect"));
        }

        [Test]
        public void DaysSchedule_OpeningClosingHour_ReturnFailure()
        {
            //Arrange
            var openingHour = "19:00";
            var closingHour = "09:00";

            //Act
            var result = controller.DaysSchedule(openingHour, closingHour) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("cannot be equal to or earlier than"));
        }

        [Test]
        public void Days_ReturnOk()
        {
            //Arrange
            var day = "monday";
            var openingHour = "09:00";
            var closingHour = "19:00";

            //Act
            var result = controller.Days(day, openingHour, closingHour) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(result.Value, Contains.Substring("successfully"));
        }

        [Test]
        public void Days_OpeningHour_ReturnFailure()
        {
            //Arrange
            var day = "monday";
            var openingHour = "09:61";
            var closingHour = "19:00";

            //Act
            var result = controller.Days(day, openingHour, closingHour) as ObjectResult;


            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("incorrect"));
        }

        [Test]
        public void Days_ClosingHour_ReturnFailure()
        {
            //Arrange
            var day = "monday";
            var openingHour = "09:00";
            var closingHour = "19:60";

            //Act
            var result = controller.Days(day, openingHour, closingHour) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("incorrect"));
        }

        [Test]
        public void Days_OpeningClosingHour_ReturnFailure()
        {
            //Arrange
            var day = "monday";
            var openingHour = "19:00";
            var closingHour = "09:00";

            //Act
            var result = controller.Days(day, openingHour, closingHour) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("cannot be equal to or earlier than"));
        }
        [Test]
        public void Days_DayOfWeek_ReturnFailure()
        {
            //Arrange
            var day = "lunes";
            var openingHour = "19:00";
            var closingHour = "09:00";

            //Act
            var result = controller.Days(day, openingHour, closingHour) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("is not valid"));
        }

        [Test]
        public void Dates_ValidDate_ReturnsSuccess()
        {
            // Arrange
            var date = "2024-11-08";
            var opening = "09:00";
            var closing = "17:00";

            // Act
            var result = controller.Dates(date, opening, closing) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(result.Value, Contains.Substring("successfully"));
        }

        [Test]
        public void Dates_InvalidDate_ReturnsBadRequest()
        {
            // Arrange 
            var date = "2024-02-30";
            var opening = "09:00";
            var closing = "17:00";

            // Act
            var result = controller.Dates(date, opening, closing) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest)); 
            Assert.That(result.Value, Contains.Substring("incorrect"));
        }

        [Test]
        public void Dates_InvalidTimeRange_ReturnsBadRequest()
        {
            // Arrange
            var date = "2024-11-03";
            var opening = "18:00";
            var closing = "09:00";

            // Act
            var result = controller.Dates(date, opening, closing) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("cannot be equal to or earlier than"));
        }

        [Test]
        public void DaysClose_ReturnOk()
        {
            //Arrange
            var days = "monday,sunday";

            //Act
            var result = controller.DaysClose(days) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(result.Value, Contains.Substring("successfully"));
        }

        [Test]
        public void DaysClose_ReturnFailure()
        {
            //Arrange
            var days = "lunes,sunday";

            //Act
            var result = controller.DaysClose(days) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("is not valid"));
        }

        [Test]
        public void DaysClose_Empty_ReturnFailure()
        {
            //Arrange
            var days = "";

            //Act
            var result = controller.DaysClose(days) as ObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("is not valid"));
        }

        [Test]
        public void DatesClose_ValidDate_ReturnsSuccess()
        {
            // Arrange
            var date = "2024-11-08";

            // Act
            var result = controller.DatesClose(date) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(result.Value, Contains.Substring("successfully"));
        }

        [Test]
        public void DatesClose_ValidDates_ReturnsOk()
        {
            // Arrange 
            var dates = "2024-02-03,2024-11-08";

            // Act
            var result = controller.DatesClose(dates) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK)); 
            Assert.That(result.Value, Contains.Substring("successfully"));
        }

        [Test]
        public void DatesClose_InvalidTimeRange_ReturnsBadRequest()
        {
            // Arrange
            var dates = "2024-02-30";

            // Act
            var result = controller.DatesClose(dates) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("incorrect"));
        }

        [Test]
        public void DatesClose_DateEmpty_ReturnsBadRequest()
        {
            // Arrange
            var dates = "";

            // Act
            var result = controller.DatesClose(dates) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("incorrect"));
        }

        [Test]
        public void Get_ReturnsSuccess()
        {
            // Arrange
            var minutes = 120;
            var date = "2024-11-08 09:00";

            // Act
            var result = controller.Get(minutes,date) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(result.Value, Is.EqualTo("Fri Nov 08 11:00:00 2024"));
        }

        [Test]
        public void Get_ValidDates_ReturnsBadRequest()
        {
            // Arrange 
            var minutes = -120;
            var date = "2024-11-08 09:00";

            // Act
            var result = controller.Get(minutes,date) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest)); 
            Assert.That(result.Value, Contains.Substring("cannot be negative"));
        }

        [Test]
        public void Get_InvalidTimeRange_ReturnsBadRequest()
        {
            // Arrange
            var minutes = 120;
            var date = "2024-11-08 09:60";

            // Act
            var result = controller.Get(minutes, date) as ObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(result.Value, Contains.Substring("incorrect"));
        }
    }
}
