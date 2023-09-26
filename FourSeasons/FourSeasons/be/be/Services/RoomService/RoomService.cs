using be.Models;
using be.Repositories.RoomRepository;
using System.Dynamic;

namespace be.Services
{
    public class RoomService : IRoomService
    {
        public IRoomRepository _roomRepo;

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepo = roomRepository;
        }

        public void AddRoom(Room room)
        {
            _roomRepo.AddRoom(room);
        }

        public void DeleteRoom(int roomId)
        {
            _roomRepo.DeleteRoom(roomId);
        }

        public bool DisableRoom(int roomId)
        {
            return _roomRepo.DisableRoom(roomId);
        }

        public dynamic GetAllRooms()
        {
            return _roomRepo.GetAllRooms();
        }

        public Room GetRoom(int roomId)
        {
            return _roomRepo.GetRoom(roomId);
        }

        public bool UpdateRoom(Room room, int id)
        {
            return _roomRepo.UpdateRoom(room, id);
        }

        public IEnumerable<dynamic> GetAllRoomCategories(int id)
        {
            return _roomRepo.GetAllRoomCategories(id);
        }

        public dynamic GetEnableRooms()
        {
            return _roomRepo.GetEnableRooms();
        }

        public IEnumerable<Room> GetAllRoom()
        {
            return _roomRepo.GetAllRoom();
        }

        public dynamic GetRoomsByRoomType(string categoryName)
        {
            return _roomRepo.GetRoomsByRoomType(categoryName);
        }

        public dynamic GetAvailableRoomsByType(string categoryName, DateTime sd, DateTime ed)
        {
            return _roomRepo.GetAvailableRoomsByType(categoryName, sd, ed);
        }

        public dynamic GetRooms(int[] roomIds)
        {
            return _roomRepo.GetRooms(roomIds);
        }
    }
}
