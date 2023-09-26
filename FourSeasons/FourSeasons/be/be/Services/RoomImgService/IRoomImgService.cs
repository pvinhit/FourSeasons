using be.Models;

namespace be.Services
{
    public interface IRoomImgService
    {
        void AddRoomImg(IList<string> imgs, int roomId);
        void DeleteRoomImg(int roomId);
        void UpdateRoomImg(IList<string> imgs, int roomId);
        IEnumerable<RoomImg> GetAllRoomImgByRoomId(int roomId);
    }
}
