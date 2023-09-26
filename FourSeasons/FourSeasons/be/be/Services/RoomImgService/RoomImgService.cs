using be.Models;
using be.Repositories.RoomImgRepository;

namespace be.Services
{
    public class RoomImgService : IRoomImgService
    {
        public IRoomImgRepository _roomImgRepo;

        public RoomImgService(IRoomImgRepository roomImgRepository)
        {
            _roomImgRepo = roomImgRepository;
        }

        public void AddRoomImg(IList<string> imgs, int roomId)
        {
            _roomImgRepo.AddRoomImg(imgs, roomId);
        }

        public void DeleteRoomImg(int roomId)
        {
            _roomImgRepo.DeleteRoomImg(roomId);
        }

        public IEnumerable<RoomImg> GetAllRoomImgByRoomId(int roomId)
        {
            return _roomImgRepo.GetAllRoomImgByRoomId(roomId);
        }

        public void UpdateRoomImg(IList<string> imgs, int roomId)
        {
            _roomImgRepo.UpdateRoomImg(imgs, roomId);
        }
    }
}
