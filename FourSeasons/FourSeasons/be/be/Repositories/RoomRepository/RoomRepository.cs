using be.Models;
using be.Services;
using System.Dynamic;

namespace be.Repositories.RoomRepository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DbFourSeasonHotelContext _context;
        public IRoomImgService _roomImgService;

        public RoomRepository(IRoomImgService roomImgService, DbFourSeasonHotelContext context)
        {
            _context = context;
            _roomImgService = roomImgService;
        }

        public void AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public void DeleteRoom(int roomId)
        {
            var item = _context.Rooms.Find(roomId);
            _context.Rooms.Remove(item);
            _context.SaveChanges();
        }

        public bool DisableRoom(int roomId)
        {
            var item = _context.Rooms.Find(roomId);
            if (item != null)
            {
                item.Status = false;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public dynamic GetAllRooms()
        {
            var rooms = from Room in _context.Rooms
                        join Category in _context.Categories on Room.CategoryId equals Category.CategoryId
                        orderby Room.RoomId
                        select new
                        {
                            Room.RoomId,
                            Room.RoomName,
                            Room.Description,
                            Room.CategoryId,
                            Category.CategoryName,
                            Room.Price,
                            Room.Status
                        };

            dynamic result = new List<ExpandoObject>();

            var roomImgs = new List<RoomImg>();

            foreach (var room in rooms)
            {
                roomImgs = _roomImgService.GetAllRoomImgByRoomId(room.RoomId).ToList();
                dynamic temp = new ExpandoObject();
                temp.roomInfo = room;
                temp.images = roomImgs;
                result.Add(temp);
            }

            return result;
        }

        public Room GetRoom(int roomId)
        {
            return _context.Rooms.Find(roomId);
        }

        public bool UpdateRoom(Room room, int id)
        {
            var updateRoom = _context.Rooms.Find(id);
            if (updateRoom != null)
            {
                updateRoom.RoomName = room.RoomName;
                updateRoom.Description = room.Description;
                updateRoom.CategoryId = room.CategoryId;
                updateRoom.Price = room.Price;
                updateRoom.Status = room.Status;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<dynamic> GetAllRoomCategories(int id)
        {
            var result = (from r in _context.Rooms
                          join c in _context.Categories on r.CategoryId equals c.CategoryId
                          join i in _context.RoomImgs on r.RoomId equals i.RoomId
                          where r.CategoryId == id
                          group r by new { c.CategoryId, r.RoomId } into k
                          select new
                          {
                              k.Key.RoomId,
                              RoomName = (from rr in _context.Rooms where rr.RoomId == k.Key.RoomId select rr.RoomName).FirstOrDefault(),
                              Description = (from rr in _context.Rooms where rr.RoomId == k.Key.RoomId select rr.Description).FirstOrDefault(),
                              Price = (from rr in _context.Rooms where rr.RoomId == k.Key.RoomId select rr.Price).FirstOrDefault(),
                              Image = (from rr in _context.Rooms
                                       join ii in _context.RoomImgs on rr.RoomId equals ii.RoomId
                                       where rr.RoomId == k.Key.RoomId
                                       select ii.Image).FirstOrDefault(),
                          }).ToList();

            if (result.Count() > 0)
            {
                return result;
            }

            return null;
        }

        public dynamic GetEnableRooms()
        {
            var rooms = from Room in _context.Rooms
                        join Category in _context.Categories on Room.CategoryId equals Category.CategoryId
                        where Room.Status == true
                        orderby Room.RoomId
                        select new
                        {
                            Room.RoomId,
                            Room.RoomName,
                            Room.Description,
                            Room.CategoryId,
                            Category.CategoryName,
                            Room.Price,
                            Room.Status
                        };

            dynamic result = new List<ExpandoObject>();

            var roomImgs = new List<RoomImg>();

            foreach (var room in rooms)
            {
                roomImgs = _roomImgService.GetAllRoomImgByRoomId(room.RoomId).ToList();
                dynamic temp = new ExpandoObject();
                temp.roomInfo = room;
                temp.images = roomImgs;
                result.Add(temp);
            }

            return result;
        }

        public IEnumerable<Room> GetAllRoom()
        {
            if (_context.Rooms == null)
            {
                return null;
            }
            return _context.Rooms.ToList();
        }

        public dynamic GetRoomsByRoomType(string categoryName)
        {
            var rooms = from Room in _context.Rooms
                        join Category in _context.Categories on Room.CategoryId equals Category.CategoryId
                        where Category.CategoryName == categoryName
                        orderby Room.RoomId
                        select new
                        {
                            Room.RoomId,
                            Room.RoomName,
                            Room.Description,
                            Room.CategoryId,
                            Category.CategoryName,
                            Room.Price,
                            Room.Status
                        };

            dynamic result = new List<ExpandoObject>();

            var roomImgs = new List<RoomImg>();

            foreach (var room in rooms)
            {
                roomImgs = _roomImgService.GetAllRoomImgByRoomId(room.RoomId).ToList();
                dynamic temp = new ExpandoObject();
                temp.roomInfo = room;
                temp.images = roomImgs;
                result.Add(temp);
            }

            return result;
        }

        public dynamic GetAvailableRoomsByType(string categoryName, DateTime sd, DateTime ed)
        {
            var bookings_roomId = from Booking in _context.Bookings
                                  join BookingDetail in _context.BookingDetails on Booking.BookingId equals BookingDetail.BookingId
                                  where Booking.CheckOut > sd && Booking.CheckIn < ed
                                  select BookingDetail.RoomId;



            var rooms = _context.Rooms.Where(r => r.Category.CategoryName == categoryName && r.Status == true)
            .Select(r => r.RoomId).ToList();




            var availableRoomIds = rooms.Where(r => !bookings_roomId.Any(b => r.Equals(b))).ToArray();
            dynamic availableRooms = GetRoomsByRoomType(categoryName);
            dynamic result = new List<ExpandoObject>();
            foreach (int roomId in availableRoomIds)
            {
                foreach (var room in availableRooms)
                {
                    if (room.roomInfo.RoomId == roomId)
                    {
                        result.Add(room);
                    }
                }
            }
            return result;
        }

        public dynamic GetRooms(int[] roomIds)
        {
            dynamic rooms = GetAllRooms();
            dynamic result = new List<ExpandoObject>();
            foreach (int roomId in roomIds)
            {
                foreach (var room in rooms)
                {
                    if (room.roomInfo.RoomId == roomId)
                    {
                        result.Add(room);
                    }
                }
            }
            return result;
        }
    }
}
