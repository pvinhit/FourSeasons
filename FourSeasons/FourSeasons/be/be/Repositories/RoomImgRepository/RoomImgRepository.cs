using be.Models;

namespace be.Repositories.RoomImgRepository
{
    public class RoomImgRepository : IRoomImgRepository
    {
        private readonly DbFourSeasonHotelContext _context;

        public RoomImgRepository()
        {
            _context = new DbFourSeasonHotelContext();
        }

        public void AddRoomImg(IList<string> imgs, int roomId)
        {
            foreach (var img in imgs)
            {
                RoomImg roomImg = new RoomImg
                {
                    RoomId = roomId,
                    Image = img
                };
                _context.RoomImgs.Add(roomImg);
                _context.SaveChanges();
            }
        }

        public void DeleteRoomImg(int roomId)
        {
            var imgs = _context.RoomImgs.Where(rI => rI.RoomId == roomId).ToList();
            foreach (var img in imgs)
            {
                _context.RoomImgs.Remove(img);
                _context.SaveChanges();
            }
        }

        public IEnumerable<RoomImg> GetAllRoomImgByRoomId(int roomId)
        {
            var roomImgList = _context.RoomImgs.Where(rI => rI.RoomId == roomId).ToList();
            return roomImgList;
        }

        public void UpdateRoomImg(IList<string> imgs, int roomId)
        {
            DeleteRoomImg(roomId);
            AddRoomImg(imgs, roomId);
        }
    }
}
