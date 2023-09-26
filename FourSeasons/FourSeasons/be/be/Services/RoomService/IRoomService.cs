using be.Models;

namespace be.Services
{
    public interface IRoomService
    {
        Room GetRoom(int roomId);
        void AddRoom(Room room);
        bool UpdateRoom(Room room, int id);
        void DeleteRoom(int roomId);
        dynamic GetAllRooms();
        dynamic GetEnableRooms();
        dynamic GetRoomsByRoomType(string categoryName);
        dynamic GetRooms(int[] roomIds);
        bool DisableRoom(int roomId);
        IEnumerable<dynamic> GetAllRoomCategories(int id);

        dynamic GetAvailableRoomsByType(string categoryName, DateTime sd, DateTime ed);
        IEnumerable<Room> GetAllRoom();
    }
}
