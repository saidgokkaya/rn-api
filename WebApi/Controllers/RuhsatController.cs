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

        [HttpGet("activities")]
        public async Task<ActionResult<IEnumerable<Activities>>> GetSchemas()
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
