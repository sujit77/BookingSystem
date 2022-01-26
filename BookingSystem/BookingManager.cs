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
        public ConcurrentDictionary<int, List<ReservationInfo>> Reservations;
        List<int> rooms; 
        
        public BookingManager(List<int> roomNos)
        {
            // initialise rooms 
            rooms = roomNos;            
            // intialise empty reservations concurrent dict
            Reservations = new ConcurrentDictionary<int, List<ReservationInfo>>();
        }

        public void AddBooking(string guest, int room, DateTime date)
        {

            if (IsRoomAvailable(room, date))
            {
                var reservation = new ReservationInfo() { BookedDate = date,RoomId = room };
                if (Reservations.ContainsKey(room))
                {
                    Reservations[room].Add(reservation);
                }
                Reservations.TryAdd(room, new List<ReservationInfo>() { reservation });
            }
            else
            {
                throw new Exception($"Room No {room} is not available on {date}");
            }

        }

        public IEnumerable<int> GetAvailableRooms(DateTime date)
        {
            var nonBookedRooms = rooms.Except(Reservations.Keys);// start with rooms that are not in Reservations dictionary i.e. not booked for anu dates

            foreach(KeyValuePair<int,List<ReservationInfo>> item in Reservations)
            {
                
                if (item.Value.Any(x => x.BookedDate == date))
                {
                   // no rooms on this date
                }
                else
                {
                    // room 
                    nonBookedRooms.ToList().Add(item.Key);
                }
            }
            return nonBookedRooms;


        }

        public bool IsRoomAvailable(int room, DateTime date)
        {
            return !Reservations.Any(x => x.Key == room && x.Value.Any(r=>r.BookedDate == date));
        }
    }
}
