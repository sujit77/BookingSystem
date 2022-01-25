using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem
{
    public class BookingManager : IBookingManager
    {
        // thread safe data structure that maintains room id and reservationinfo against this room
        public ConcurrentDictionary<int, ReservationInfo> Reservations;
        List<int> rooms; 
        
        public BookingManager(List<int> roomNos)
        {
            // initialise rooms 
            rooms = roomNos;            
            // intialise empty reservations concurrent dict
            Reservations = new ConcurrentDictionary<int, ReservationInfo>();
        }

        public void AddBooking(string guest, int room, DateTime date)
        {

            if (IsRoomAvailable(room, date))
            {
                var reservation = new ReservationInfo() { StartDate = date, EndDate = date, RoomId = room };
                Reservations.TryAdd(room, reservation);
            }
            else
            {
                throw new Exception($"Room No {room} is not available on {date}");
            }

        }

        public IEnumerable<int> GetAvailableRooms(DateTime date)
        {
         
            foreach(KeyValuePair<int,ReservationInfo> item in Reservations)
            {
                if (item.Value.StartDate == date )
                {
                   // item.Key room is not available on date
                }
                else
                {
                    // room 
                    yield return item.Key;
                }
            }
            
        }

        public bool IsRoomAvailable(int room, DateTime date)
        {
            return !Reservations.Any(x => x.Key == room && x.Value.StartDate == date);
        }
    }
}
