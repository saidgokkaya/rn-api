using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations.Ruhsat;
using Service.Implementations.User;
using Utilities.Helper;
using WebApi.Models.Ruhsat;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RuhsatController : ControllerBase
    {
        private readonly ILogger<RuhsatController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly RuhsatService _ruhsatService;
        private readonly DefaultValues _defaultValues;

        public RuhsatController(ILogger<RuhsatController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _userService = new UserService();
            _ruhsatService = new RuhsatService();
            _defaultValues = new DefaultValues();
        }

        [HttpGet("ruhsat-turu")]
        public async Task<ActionResult<IEnumerable<RuhsatTuru>>> GetRuhsatTurus()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var ruhsatTurus = await _ruhsatService.GetRuhsatTuru();

            var ruhsatTurusList = ruhsatTurus.Select(ruhsatTuru => new RuhsatTuru
            {
                Id = ruhsatTuru.Id,
                Name = ruhsatTuru.Name
            }).ToList();

            return Ok(ruhsatTurusList);
        }

        [HttpGet("activities")]
        public async Task<ActionResult<IEnumerable<Activities>>> GetActivities()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var activities = await _ruhsatService.GetFaaliyetKonusu(user.OrganizationId);

            var activitiesList = activities.Select(activity => new Activities
            {
                Id = activity.Id,
                Name = activity.Name
            }).ToList();

            return Ok(activitiesList);
        }

        [HttpPost]
        [Route("add-activities")]
        public async Task<IActionResult> AddActivities([FromBody] AddActivitiesRequest request)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);

            var existingActivities = await _ruhsatService.GetFaaliyetKonusu(user.OrganizationId);

            var incomingActivities = request.Name;

            var templatesToUpdate = new List<ActivityDto>();
            var templatesToAdd = new List<ActivityDto>();

            foreach (var incoming in incomingActivities)
            {
                if (incoming.Id.HasValue)
                {
                    var match = existingActivities.FirstOrDefault(e => e.Id == incoming.Id.Value);
                    if (match != null)
                    {
                        if (match.Name != incoming.Name)
                        {
                            templatesToUpdate.Add(incoming);
                        }
                    }
                }
                else
                {
                    templatesToAdd.Add(incoming);
                }
            }

            var incomingIds = incomingActivities.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();
            var templatesToRemove = existingActivities
                .Where(e => !incomingIds.Contains(e.Id))
                .ToList();

            foreach (var add in templatesToAdd)
            {
                _ruhsatService.AddFaaliyetKonusu(user.OrganizationId, add.Name);
            }

            foreach (var update in templatesToUpdate)
            {
                _ruhsatService.UpdateFaaliyetKonusu(update.Id.Value, update.Name);
            }

            foreach (var delete in templatesToRemove)
            {
                _ruhsatService.IsDeletedFaaliyetKonusu(delete.Id);
            }

            return Ok(1);
        }

        [HttpGet("classes")]
        public async Task<ActionResult<IEnumerable<Classes>>> GetClasses()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var classes = await _ruhsatService.GetRuhsatSinifi(user.OrganizationId);

            var classesList = classes.Select(classs => new Classes
            {
                Id = classs.Id,
                Name = classs.Name,
                RuhsatTuruId = classs.RuhsatTuruId,
            }).ToList();

            return Ok(classesList);
        }

        [HttpPost]
        [Route("add-classes")]
        public async Task<IActionResult> AddClasses([FromBody] AddClassesRequest request)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);

            var existingClasses = await _ruhsatService.GetRuhsatSinifi(user.OrganizationId);

            var incomingClasses = request.Name;

            var templatesToUpdate = new List<ClassDto>();
            var templatesToAdd = new List<ClassDto>();

            foreach (var incoming in incomingClasses)
            {
                if (incoming.Id.HasValue)
                {
                    var match = existingClasses.FirstOrDefault(e => e.Id == incoming.Id.Value);
                    if (match != null)
                    {
                        var ruhsatTuruChanged = match.RuhsatTuruId != incoming.RuhsatTuruId;
                        var nameChanged = !string.Equals(match.Name, incoming.Name, StringComparison.Ordinal);

                        if (nameChanged || ruhsatTuruChanged)
                        {
                            templatesToUpdate.Add(incoming);
                        }
                    }
                }
                else
                {
                    templatesToAdd.Add(incoming);
                }
            }

            var incomingIds = incomingClasses.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();
            var templatesToRemove = existingClasses
                .Where(e => !incomingIds.Contains(e.Id))
                .ToList();

            foreach (var add in templatesToAdd)
            {
                _ruhsatService.AddRuhsatSinifi(user.OrganizationId, add.Name, add.RuhsatTuruId);
            }

            foreach (var update in templatesToUpdate)
            {
                _ruhsatService.UpdateRuhsatSinifi(update.Id.Value, update.Name, update.RuhsatTuruId);
            }

            foreach (var delete in templatesToRemove)
            {
                _ruhsatService.IsDeletedRuhsatSinifi(delete.Id);
            }

            return Ok(1);
        }

        [HttpGet("warehouses")]
        public async Task<ActionResult<IEnumerable<WareHouses>>> GetWareHouses()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var warehouses = await _ruhsatService.GetDepo(user.OrganizationId);

            var warehousesList = warehouses.Select(warehouse => new WareHouses
            {
                Id = warehouse.Id,
                Name = warehouse.Adi,
                RuhsatSinifiId = warehouse.RuhsatSinifiId,
            }).ToList();

            return Ok(warehousesList);
        }

        [HttpPost]
        [Route("add-warehouse")]
        public async Task<IActionResult> AddWareHouses([FromBody] AddWareHousesRequest request)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);

            var existingWarehouses = await _ruhsatService.GetDepo(user.OrganizationId);

            var incomingWarehouses = request.Name;

            var templatesToUpdate = new List<WareHouseDto>();
            var templatesToAdd = new List<WareHouseDto>();

            foreach (var incoming in incomingWarehouses)
            {
                if (incoming.Id.HasValue)
                {
                    var match = existingWarehouses.FirstOrDefault(e => e.Id == incoming.Id.Value);
                    if (match != null)
                    {
                        var ruhsatSinifiChanged = match.RuhsatSinifiId != incoming.RuhsatSinifiId;
                        var nameChanged = !string.Equals(match.Adi, incoming.Name, StringComparison.Ordinal);

                        if (nameChanged || ruhsatSinifiChanged)
                        {
                            templatesToUpdate.Add(incoming);
                        }
                    }
                }
                else
                {
                    templatesToAdd.Add(incoming);
                }
            }

            var incomingIds = incomingWarehouses.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();
            var templatesToRemove = existingWarehouses
                .Where(e => !incomingIds.Contains(e.Id))
                .ToList();

            foreach (var add in templatesToAdd)
            {
                _ruhsatService.AddDepo(user.OrganizationId, add.Name, add.RuhsatSinifiId);
            }

            foreach (var update in templatesToUpdate)
            {
                _ruhsatService.UpdateDepo(update.Id.Value, update.Name, update.RuhsatSinifiId);
            }

            foreach (var delete in templatesToRemove)
            {
                _ruhsatService.IsDeletedDepo(delete.Id);
            }

            return Ok(1);
        }

        private int UserId()
        {
            var userIdClaim = HttpContext.User.FindFirst("userId");
            if (userIdClaim == null)
            {
                return 0;
            }

            int userId = int.Parse(userIdClaim.Value);
            return userId;
        }
    }
}
