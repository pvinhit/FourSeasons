using be.Models;

namespace be.Repositories.RoomImgRepository
{
    public interface IRoomImgRepository
    {
        void AddRoomImg(IList<string> imgs, int roomId);
        void DeleteRoomImg(int roomId);
        void UpdateRoomImg(IList<string> imgs, int roomId);
        IEnumerable<RoomImg> GetAllRoomImgByRoomId(int roomId);
    }
}
