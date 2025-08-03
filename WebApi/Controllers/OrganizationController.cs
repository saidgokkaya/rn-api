using AdminPanel.Models.Auth;
using AdminPanel.Models.Organization.Role;
using AdminPanel.Models.Organization.User;
using Core.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations.User;
using System.Globalization;
using System.Xml;
using Utilities.Helper;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly ILogger<OrganizationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly DefaultValues _defaultValues;
        private readonly EmailHelper _emailHelper;

        public OrganizationController(ILogger<OrganizationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _userService = new UserService();
            _defaultValues = new DefaultValues();
            _emailHelper = new EmailHelper();
        }

        [HttpPost("check-mail-me")]
        public async Task<IActionResult> CheckMailMe([FromBody] CheckMailRequest request)
        {
            var userCheck = _userService.GetUserCheckMail(request.Mail, request.UserId).ToList();
            if (userCheck.Count != 0)
            {
                return Ok(0);
            }

            return Ok(1);
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<Users>> GetUsers()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var users = _userService.GetUsers(user.OrganizationId, userId);

            var userList = users.Select(user => new Users
            {
                Id = user.Id,
                Name = user.FirstName + " " + user.LastName,
                Mail = user.Mail,
                Phone = user.Phone,
                IsActive = user.IsActive ? "Aktif" : "Pasif",
            }).ToList();

            return Ok(userList);
        }

        [HttpGet("user")]
        public ActionResult<GetUserAndRole> GetUser(int userId = 0)
        {
            if (userId == 0)
            {
                userId = UserId();
            }

            var user = _userService.GetUserById(userId);
            var organization = _userService.GetOrganizationById(user.OrganizationId);
            var role = _userService.GetUserRole(userId);

            var data = new GetUserAndRole
            {
                Name = organization.Name,
                TaskNumber = organization.TaskNumber,
                OrgAddress = organization.Address,
                ZipCode = organization.ZipCode,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mail = user.Mail,
                Phone = user.Phone,
                Roles = role.Select(q => q.RoleId).ToList()
            };

            return Ok(data);
        }

        [HttpGet("admin-user")]
        public ActionResult<GetAdminUserAndRole> GetAdminUser(int userId = 0)
        {
            if (userId == 0)
            {
                userId = UserId();
            }

            var user = _userService.GetUserById(userId);
            var organization = _userService.GetOrganizationById(user.OrganizationId);
            var role = _userService.GetUserRole(userId);

            var data = new GetAdminUserAndRole
            {
                Name = organization.Name,
                TaskNumber = organization.TaskNumber,
                OrgAddress = organization.Address,
                ZipCode = organization.ZipCode,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mail = user.Mail,
                Phone = user.Phone,
                Roles = role.Select(q => q.RoleId).ToList()
            };

            return Ok(data);
        }

        [HttpGet("drawer")]
        public ActionResult<Drawer> GetDrawer()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);

            var data = new Drawer
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Mail = user.Mail
            };

            return Ok(data);
        }

        [HttpGet("workspace")]
        public ActionResult<GetOrganization> GetWorkspace()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var organization = _userService.GetOrganizationById(user.OrganizationId);

            var data = new GetOrganization
            {
                Id = organization.Id,
                Name = organization.Name,
            };

            return Ok(data);
        }

        [HttpGet("get-add-user")]
        public ActionResult<GetUserAndRole> GetAddUser()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var organization = _userService.GetOrganizationById(user.OrganizationId);

            var data = new GetUser
            {
                Name = organization.Name,
                TaskNumber = organization.TaskNumber,
                OrgAddress = organization.Address,
                ZipCode = organization.ZipCode,
                FirstName = "",
                LastName = "",
                Mail = "",
                Phone = "",
                Gender = "E",
                Address = ""
            };

            return Ok(data);
        }

        [HttpGet("roles")]
        public ActionResult<IEnumerable<Roles>> GetRoles()
        {
            var roles = _userService.GetRole();

            var roleList = roles.Select(role => new Roles
            {
                Id = role.Id,
                Name = role.Name
            }).ToList();

            return Ok(roleList);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add-user")]
        public IActionResult AddUser([FromBody] AddUser user)
        {
            var userId = UserId();
            var org = _userService.GetUserById(userId);

            DateTime? dateOfBirth = null;
            if (DateTime.TryParse(user.DateOfBirth, out var parsedDate))
            {
                dateOfBirth = parsedDate.ToUniversalTime();
            }

            var firstName = _defaultValues.RemoveDiacritics(user.FirstName.ToLower());
            var lastName = _defaultValues.RemoveDiacritics(user.LastName.ToLower());
            var username = firstName + "." + lastName;

            var password = _defaultValues.GenerateRandomPassword();

            var passwordHash = _defaultValues.HashPassword(password);

            var newUser = _userService.AddUser(org.OrganizationId, user.FirstName, user.LastName, user.Mail,
                user.Phone, user.Address, username, passwordHash);

            if (newUser == 0)
            {
                return BadRequest(new { success = false, message = "User could not be added." });
            }

            var signInUrl = _configuration["EmailSettings:SignInUrl"];

            _emailHelper.SendEmail(signInUrl, user.Mail, user.FirstName, username, password);

            var filteredRoles = user.Roles?.Where(role => role != 1 && role != 4).ToList();

            if (filteredRoles != null && filteredRoles.Count != 0)
            {
                foreach (var item in filteredRoles)
                {
                    _userService.AddUserRole(newUser, item);
                }
            }

            _userService.AddUserRole(newUser, 1);
            _userService.AddUserRole(newUser, 4);

            return Ok(new
            {
                id = newUser
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-admin-user")]
        public IActionResult UpdateAdminUser([FromBody] UpdateUser user)
        {
            var userId = UserId();
            var org = _userService.GetUserById(userId);
            var updateUser = _userService.UpdateUser(userId, user.FirstName, user.LastName, user.Mail, user.Phone);
            var updateOrganization = _userService.UpdateOrganization(org.OrganizationId, user.Name, user.OrgAddress, user.ZipCode, user.TaskNumber);
            if (updateUser == 0 && updateOrganization == 0)
            {
                return BadRequest(new { success = false, message = "User could not be added." });
            }
            return Ok(new { success = true });
        }

        [HttpPost("update-only-user")]
        public IActionResult UpdateOnlyUser([FromBody] UpdateOnlyUser user)
        {
            var userId = UserId();
            var org = _userService.GetUserById(userId);

            var updateUser = _userService.UpdateUser(userId, user.FirstName, user.LastName, user.Mail, user.Phone);

            if (updateUser == 0)
            {
                return BadRequest(new { success = false, message = "User could not be added." });
            }
            return Ok(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-user")]
        public IActionResult UpdateUser([FromBody] UpdateUser user)
        {
            var updateUser = _userService.UpdateUser(user.Id, user.FirstName, user.LastName, user.Mail, user.Phone);
            if (updateUser == 0)
            {
                return BadRequest(new { success = false, message = "User could not be added." });
            }

            _userService.RemoveUserRolesByUserId(updateUser);

            var filteredRoles = user.Roles?.Where(role => role != 1 && role != 4).ToList();

            if (filteredRoles != null && filteredRoles.Count != 0)
            {
                foreach (var item in filteredRoles)
                {
                    _userService.AddUserRole(updateUser, item);
                }
            }

            _userService.AddUserRole(updateUser, 1);
            _userService.AddUserRole(updateUser, 4);

            return Ok(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update-photo")]
        public IActionResult UpdatePhoto([FromForm] AddPhoto photo)
        {
            if (photo?.Photo != null)
            {
                var uploadsDirectory = _configuration["UserPhotoPath"];

                if (!Directory.Exists(uploadsDirectory))
                {
                    Directory.CreateDirectory(uploadsDirectory);
                }

                var fileExtension = Path.GetExtension(photo.Photo.FileName).ToLower();

                if (fileExtension != ".png")
                {
                    return BadRequest("Yalnızca .png dosya uzantıları kabul edilmektedir.");
                }

                var fileNameWithoutExtension = photo.UserId.ToString();

                var existingFiles = Directory.GetFiles(uploadsDirectory, fileNameWithoutExtension + ".*");
                foreach (var existingFile in existingFiles)
                {
                    var existingFileExtension = Path.GetExtension(existingFile).ToLower();
                    if (existingFileExtension == ".png")
                    {
                        System.IO.File.Delete(existingFile);
                    }
                }

                var newFileName = fileNameWithoutExtension + fileExtension;
                var newFilePath = Path.Combine(uploadsDirectory, newFileName);

                using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    photo.Photo.CopyTo(fileStream);
                }

                return Ok(new { success = true });
            }

            return BadRequest("Geçerli bir fotoğraf yüklenmedi.");
        }

        [HttpPost("update-user-password")]
        public IActionResult UpdatePassword([FromBody] UpdatePassword user)
        {
            var userId = UserId();
            var passwordHash = _defaultValues.HashPassword(user.NewPassword);
            var newUser = _userService.UpdatePassword(userId, passwordHash);
            if (newUser == 0)
            {
                return Ok(new { success = false });
            }
            return Ok(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete-user")]
        public IActionResult DeleteUser(int userId)
        {
            var user = _userService.IsDeletedUser(userId);
            if (user == 0)
            {
                return Ok(new { success = false });
            }
            return Ok(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("delete-users")]
        public IActionResult DeleteUsers([FromBody] DeleteUsers user)
        {
            foreach (var item in user.UserId)
            {
                var deleteUser = _userService.IsDeletedUser(item);
                if (deleteUser == 0)
                {
                    return Ok(new { success = false });
                }
            }
            return Ok(new { success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("status-user")]
        public IActionResult StatusUser(int userId)
        {
            var user = _userService.IsActiveUser(userId);
            if (user == 0)
            {
                return Ok(new { success = false });
            }
            return Ok(new { success = true });
        }

        [HttpGet("logo")]
        public IActionResult GetOrganizationLogo()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var organization = _userService.GetOrganizationById(user.OrganizationId);
            if (organization != null && !string.IsNullOrEmpty(organization.LogoUrl))
            {
                return Ok(new { logoUrl = organization.LogoUrl });
            }

            return NotFound();
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
