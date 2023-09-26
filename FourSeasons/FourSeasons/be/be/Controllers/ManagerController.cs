using be.Models;
using be.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace be.Controllers
{
    [Route("api/manager")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;
        private readonly IRoomService _roomService;
        private readonly ICategoryService _categoryService;
        private readonly IRoomImgService _roomImgService;

        public readonly DbFourSeasonHotelContext _context = new DbFourSeasonHotelContext();

        public ManagerController(IManagerService managerService, IRoomService roomService, ICategoryService categoryService, IRoomImgService roomImgService)
        {
            _managerService = managerService;
            _roomService = roomService;
            _categoryService = categoryService;
            _roomImgService = roomImgService;
        }

        [HttpGet("all")]
        public ActionResult GetAllManagers()
        {
            var result = _managerService.GetAllManagers();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet]
        public ActionResult GetManagers(int id)
        {
            var result = _managerService.GetManager(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteManager(int id)
        {
            _managerService.DeleteManager(id);
            return Ok();
        }

        [HttpPut("update")]
        public ActionResult UpdateManager(User manager)
        {
            User foundManger = _managerService.GetManager(manager.UserId);
            if (foundManger.FullName != manager.FullName)
            {
                manager.Email = _managerService.GenerateEmail(manager.FullName);
            }
            _managerService.UpdateManager(manager);
            return Ok();
        }

        [HttpPost]
        public ActionResult CreateManager(User manager)
        {
            manager.Email = _managerService.GenerateEmail(manager.FullName);

            _managerService.AddManager(manager);
            return Ok();
        }

        [HttpPut("updateStatus")]
        public ActionResult UpdateStatusManager(UserStatus userStatus)
        {
            _managerService.UpdateStatusManager(userStatus);
            return Ok();
        }

        [HttpGet("allIdCards")]
        public ActionResult GetIdCardList()
        {

            var result = _managerService.GetManagerIdCard();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        #region manage_Category
        [HttpGet("GetCategories")]
        public ActionResult GetCategories()
        {
            var categories = _categoryService.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost("AddCategory")]
        public ActionResult AddCategory(Category category)
        {
            Category newCategory = new Category();
            newCategory.CategoryName = category.CategoryName;
            newCategory.MaxPeople = category.MaxPeople;
            newCategory.Description = category.Description;
            newCategory.Size = category.Size;
            newCategory.Status = category.Status;
            newCategory.Image = category.Image;
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPatch("UpdateCategory/{id}")]
        public ActionResult UpdateCategory(Category category)
        {
            try
            {
                var result = _context.Categories.Where(x => x.CategoryId == category.CategoryId).FirstOrDefault();
                result.Size = category.Size;
                result.Status = category.Status;
                result.Description = category.Description;
                result.CategoryName = category.CategoryName;
                result.Image = category.Image;
                result.MaxPeople = category.MaxPeople;
                _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("DeleteCategory/{id}")]
        public bool DeleteCategory(int id)
        {
            var result = _context.Categories.Where(x => x.CategoryId == id).FirstOrDefault();
            if (result != null)
            {
                result.Status = false;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region manage_Room
        [HttpGet("GetRooms")]
        public ActionResult GetRooms()
        {
            var rooms = _roomService.GetAllRooms();
            return Ok(rooms);
        }

        [HttpPost("AddRoom")]
        public IActionResult AddRoom([FromBody] RoomInfo roomInfo)
        {
            _roomService.AddRoom(roomInfo.room);
            int id = roomInfo.room.RoomId;
            _roomImgService.AddRoomImg(roomInfo.images, id);

            return Ok();
        }

        [HttpPatch("DeleteRoom/{id}")]
        public ActionResult DeleteRoom(int id)
        {
            if (_roomService.DisableRoom(id))
            {
                return Ok();
            }
            return Ok(new
            {
                status = 404,
                message = "Room Id Not Found"
            });
        }

        [HttpPatch("UpdateRoom/{id}")]
        public ActionResult UpdateRoom([FromBody] RoomInfo roomInfo)
        {
            _roomImgService.UpdateRoomImg(roomInfo.images, roomInfo.room.RoomId);
            _roomService.UpdateRoom(roomInfo.room, roomInfo.room.RoomId);
            return Ok();
        }
        #endregion
    }

    public class RoomInfo
    {
        public Room room { get; set; }
        public List<string> images { get; set; }
    }
}
