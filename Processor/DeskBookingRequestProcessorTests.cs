using DeskBooker.Core.DataInterface;
using DeskBooker.Core.Dormain;
using DeskBooker.Core.Processor;
using Moq;

namespace DeskBooker.Core.Tests.Processor
{
    public class DeskBookingRequestProcessorTests
    {
        private readonly DeskBookingRequest _request;
        private readonly Mock<IDeskBookingRepository> _deskBookingRepositoryMock;
        private readonly DeskBookingRequestProcessor _processor;

        public DeskBookingRequestProcessorTests()
        {
            _request = new DeskBookingRequest
            {
                FirstName = "Kenneth",                                  /* setting up the  request data for the desk booking class*/
                LastName = "Karani",
                Email = "kenneth@KennethMwangiKarani.com",
                Date = new DateTime(2023, 8, 24)
            };
            _deskBookingRepositoryMock = new Mock<IDeskBookingRepository>();
            _processor = new DeskBookingRequestProcessor(_deskBookingRepositoryMock.Object);
        }
        [Fact]
        public void ShouldReturnDeskBookingResultWithRequestValues()
        {
            //Arrange
           
            
            //Act
            DeskBookingResult result = _processor.BookDesk(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_request.FirstName, result.FirstName); //if the FirstName from request in the name from the results FirstName also checks the other details like lastname,email and Date
            Assert.Equal(_request.LastName, result.LastName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);
        }

        [Fact]
        public void ShouldThrowExceptionIfRequestIsNull()
        {
            
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookDesk(null));
            Assert.Equal("request", exception.ParamName);
        }
        [Fact]
        public void ShouldSaveDeskBooking()
        {
            DeskBooking savedDeskBooking = null;
            _deskBookingRepositoryMock.Setup(x => x.Save(It.IsAny<DeskBooking>()))
                 .Callback<DeskBooking>(deskBooking =>
                 {
                     savedDeskBooking = deskBooking;
                 });
            _processor.BookDesk(_request);
            _deskBookingRepositoryMock.Verify(x => x.Save(It.IsAny<DeskBooking>()), Times.Once);
            Assert.NotNull(savedDeskBooking);
            Assert.Equal(_request.FirstName, savedDeskBooking.FirstName);
            Assert.Equal(_request.LastName, savedDeskBooking.LastName);
            Assert.Equal(_request.Email, savedDeskBooking.Email);
            Assert.Equal(_request.Date, savedDeskBooking.Date);
        }
    }
}
