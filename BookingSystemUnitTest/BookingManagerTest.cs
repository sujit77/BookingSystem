using BookingSystem;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystemUnitTest
{
    [TestFixture]
    public class BookingManagerTest
    {

        [Test]
        public void RoomAvailable_Returns_True_False_Exception_Test()
        {
            var rooms = Enumerable.Range(100, 210).OrderBy(t => new Random().Next()).Take(4);            

            IBookingManager bm = new BookingManager(rooms.ToList());
            var dt1 = new DateTime(2022,01,31);

            var room101Available = bm.IsRoomAvailable(101, dt1);// outputs true
            Assert.AreEqual(true, room101Available);

            bm.AddBooking("Patel", 101, dt1);
            var room101AvailablkeAfterBooking = bm.IsRoomAvailable(101, dt1);// outputs false
            Assert.AreEqual(false, room101AvailablkeAfterBooking);
           
            Assert.Throws<Exception>(()=>bm.AddBooking("Li", 101, dt1), "Room No 101 is not available on 31/01/2022 00:00:00"); // throws an exception

        }

        [Test]
        public void RoomAvailable_MultiThread_Booking_Test()
        {
            var rooms = Enumerable.Range(100, 210).OrderBy(t => new Random().Next()).Take(4);

            IBookingManager bm = new BookingManager(rooms.ToList());
            var dt1 = new DateTime(2022, 01, 31);

            Parallel.For(100, 104, (i) => bm.AddBooking("Patel", i, dt1));            
            var keys = (bm as BookingManager).Reservations.Keys.Count;

            Assert.AreEqual(4, keys);

           
        }

        [Test]
        public void GetRoomsAvailable_On_A_Date_Test()
        {
            var rooms = Enumerable.Range(100, 210).OrderBy(t => new Random().Next()).Take(4);

            IBookingManager bm = new BookingManager(rooms.ToList());
            var dt1 = new DateTime(2022, 01, 31);

            Parallel.For(100, 104, (i) => bm.AddBooking("Patel", i, dt1));
            var keys = (bm as BookingManager).Reservations.Keys.Count;

            Assert.AreEqual(4, keys);

            var availableRooms = bm.GetAvailableRooms(dt1);
            Assert.AreEqual(0, availableRooms.Count());


        }
    }
}
