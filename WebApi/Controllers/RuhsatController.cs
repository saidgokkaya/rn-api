using AdminPanel.Models.Organization.User;
using Core.Domain.Ruhsat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Implementations.Ruhsat;
using Service.Implementations.User;
using System.Net.Http;
using System.Text.Json;
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
        private readonly PdfHelper _pdfHelper;
        private readonly string _photoPath;
        private readonly string _scannedPath;

        public RuhsatController(ILogger<RuhsatController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _userService = new UserService();
            _ruhsatService = new RuhsatService();
            _defaultValues = new DefaultValues();
            _pdfHelper = new PdfHelper();
            _photoPath = configuration["PhotoPath"];
            _scannedPath = configuration["ScannedFilePath"];
        }

        [HttpGet("ruhsat-turu")]
        public async Task<ActionResult<IEnumerable<Models.Ruhsat.RuhsatTuru>>> GetRuhsatTurus()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var ruhsatTurus = await _ruhsatService.GetRuhsatTuru();

            var ruhsatTurusList = ruhsatTurus.Select(ruhsatTuru => new Models.Ruhsat.RuhsatTuru
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

        [HttpGet("classes-type")]
        public async Task<ActionResult<IEnumerable<Classes>>> GetClassesIsType(int ruhsatTuruId)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var classes = await _ruhsatService.GetRuhsatSinifi(user.OrganizationId, ruhsatTuruId);

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

        [HttpGet("warehouses-classes")]
        public async Task<ActionResult<IEnumerable<WareHouses>>> GetWareHousesIsClasses(int ruhsatSinifiId)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var warehouses = await _ruhsatService.GetDepo(user.OrganizationId, ruhsatSinifiId);

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

        [HttpGet("permits")]
        public async Task<ActionResult<IEnumerable<Permit>>> GetPermit()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var permits = await _ruhsatService.GetRuhsat(user.OrganizationId);

            var permitsList = permits.Select(permit => new Permit
            {
                Id = permit.Id,
                RuhsatNo = permit.RuhsatNo,
                TcKimlikNo = permit.TcKimlikNo,
                FullName = permit.Adi + " " + permit.Soyadi,
                IsyeriUnvani = permit.IsyeriUnvani,
                FaaliyetKonusuName = permit.FaaliyetKonusu.Name,
                RuhsatTuruName = permit.RuhsatTuru.Name,
                VerilisTarihi = permit.VerilisTarihi,
                IsActive = permit.IsActive ? "Aktif" : "Pasif",
                PhotoPath = permit.PhotoPath,
                ScannedFilePath = permit.ScannedFilePath,
            }).ToList();

            return Ok(permitsList);
        }

        [HttpPost("permit-status")]
        public async Task<IActionResult> PermitStatus(int id)
        {
            var status = _ruhsatService.IsActiveRuhsat(id);
            if (status == 0)
            {
                return BadRequest(); 
            }
            return Ok(1);
        }

        [HttpPost("delete-permit")]
        public IActionResult DeleteUser(int id)
        {
            var deletePermit = _ruhsatService.IsDeletedRuhsat(id);
            if (deletePermit == 0)
            {
                return Ok(new { success = false });
            }
            return Ok(new { success = true });
        }

        [HttpPost("delete-permits")]
        public IActionResult DeletePermits([FromBody] DeletePermits permits)
        {
            foreach (var permit in permits.PermitIds)
            {
                var deletePermit = _ruhsatService.IsDeletedRuhsat(permit);
                if (deletePermit == 0)
                {
                    return Ok(new { success = false });
                }
            }
            return Ok(new { success = true });
        }

        [HttpPost("add")]
        [RequestSizeLimit(20_000_000)]
        public async Task<IActionResult> Add([FromForm] AddRequest dto)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            if (dto.Image is null || dto.Image.Length == 0)
                return BadRequest("Görsel zorunludur.");

            Dictionary<string, string>? warehouses = null;
            if (!string.IsNullOrWhiteSpace(dto.Warehouses))
            {
                try
                {
                    warehouses = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(
                        dto.Warehouses,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }
                catch (System.Text.Json.JsonException)
                {
                    return BadRequest("Warehouses alanı geçerli bir JSON değil.");
                }
            }

            var uploadPath = _photoPath;
            Directory.CreateDirectory(uploadPath);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
            var savedPath = Path.Combine(uploadPath, fileName);

            await using (var fs = System.IO.File.Create(savedPath))
            {
                await dto.Image.CopyToAsync(fs);
            }

            var add = _ruhsatService.AddRuhsat(user.OrganizationId, dto.ActivityId, dto.TurId, dto.ClassId, dto.RuhsatNo, dto.TcKimlikNo, dto.Adi, dto.Soyadi, dto.IsyeriUnvani, dto.VerilisTarihi, dto.Ada, dto.Parsel, dto.Pafta, dto.Adres, dto.Not, fileName);

            if (warehouses is not null && warehouses.Any())
            {
                foreach (var entry in warehouses)
                {
                    var warehouseId = int.Parse(entry.Key);
                    var depo = _ruhsatService.GetDepoById(warehouseId);
                    var value = entry.Value;

                    _ruhsatService.AddDepoBilgi(user.OrganizationId, add, warehouseId, depo.Adi, value);
                }
            }

            return Ok(1);
        }

        [HttpGet("permit-by-id")]
        public IActionResult GetById(int id)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var ruhsat = _ruhsatService.GetRuhsatByIdFirst(user.OrganizationId, id);

            if (ruhsat == null)
                return NotFound("Ruhsat bulunamadı.");

            var dto = new RuhsatDetailDto
            {
                Id = ruhsat.Id,
                RuhsatTuruId = ruhsat.RuhsatTuruId,
                FaaliyetKonusuId = ruhsat.FaaliyetKonusuId,
                RuhsatSinifiId = ruhsat.RuhsatSinifiId,
                RuhsatNo = ruhsat.RuhsatNo,
                TcKimlikNo = ruhsat.TcKimlikNo,
                Adi = ruhsat.Adi,
                Soyadi = ruhsat.Soyadi,
                IsyeriUnvani = ruhsat.IsyeriUnvani,
                VerilisTarihi = ruhsat.VerilisTarihi,
                Ada = ruhsat.Ada,
                Parsel = ruhsat.Parsel,
                Pafta = ruhsat.Pafta,
                Adres = ruhsat.Adres,
                Not = ruhsat.Not,
                Warehouses = ruhsat.DepoBilgi?
                        .ToDictionary(
                            x => x.DepoId,
                            x => new WarehouseInfoDto { Bilgi = x.Bilgi, DepoAdi = x.DepoAdi }
                        ),
                ImageUrl = ruhsat.PhotoPath
            };

            return Ok(dto);
        }

        [HttpPost("update")]
        [RequestSizeLimit(20_000_000)]
        public async Task<IActionResult> Update([FromForm] UpdateRequest dto)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);

            var ruhsat = _ruhsatService.GetRuhsatById(dto.Id);
            var oldPhotoFileName = ruhsat?.PhotoPath;

            if (dto.Image != null && dto.Image.Length > 0)
            {
                if (!string.IsNullOrWhiteSpace(oldPhotoFileName))
                {
                    var oldPath = Path.Combine(_photoPath, oldPhotoFileName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        try
                        {
                            System.IO.File.Delete(oldPath);
                        }
                        catch (IOException ex)
                        {
                            _logger.LogWarning(ex, "Old photo couldn't be deleted: {Path}", oldPath);
                        }
                    }
                }

                Directory.CreateDirectory(_photoPath);
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Image.FileName)}";
                var savedPath = Path.Combine(_photoPath, fileName);

                await using (var fs = System.IO.File.Create(savedPath))
                {
                    await dto.Image.CopyToAsync(fs);
                }

                _ruhsatService.UpdatePhoto(dto.Id, fileName);
            }

            Dictionary<string, string>? warehouses = null;
            if (!string.IsNullOrWhiteSpace(dto.Warehouses))
            {
                try
                {
                    warehouses = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(
                        dto.Warehouses,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                }
                catch (System.Text.Json.JsonException)
                {
                    return BadRequest("Warehouses alanı geçerli bir JSON değil.");
                }
            }

            var update = _ruhsatService.UpdateRuhsat(dto.Id, user.OrganizationId, dto.ActivityId, dto.TurId, dto.ClassId, dto.RuhsatNo, dto.TcKimlikNo, dto.Adi, dto.Soyadi, dto.IsyeriUnvani, dto.VerilisTarihi, dto.Ada, dto.Parsel, dto.Pafta, dto.Adres, dto.Not);

            if (warehouses is not null && warehouses.Any())
            {
                var deleteDepoBilgi = _ruhsatService.GetDepoBilgi(dto.Id);
                foreach (var item in deleteDepoBilgi.ToList())
                {
                    _ruhsatService.IsDeletedDepoBilgi(item.Id);
                }
                foreach (var entry in warehouses)
                {
                    var warehouseId = int.Parse(entry.Key);
                    var depo = _ruhsatService.GetDepoById(warehouseId);
                    var value = entry.Value;

                    _ruhsatService.AddDepoBilgi(user.OrganizationId, dto.Id, warehouseId, depo.Adi, value);
                }
            }

            return Ok(1);
        }

        [HttpPost("upload-scanned-file")]
        [RequestSizeLimit(20_000_000)]
        public async Task<IActionResult> UploadScannedFile([FromForm] IFormFile file, [FromForm] int id)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Dosya seçilmedi.");

            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Yalnızca PDF dosyası yükleyebilirsiniz.");

            var uploadPath = _scannedPath;
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var ruhsat = _ruhsatService.UpdateScannedPdf(id,fileName);

            return Ok(new { scannedFilePath = fileName });
        }

        [HttpPost("delete-scanned-file")]
        public IActionResult DeleteScannedFile(int id)
        {
            var ruhsat = _ruhsatService.GetRuhsatById(id);

            if (ruhsat == null || string.IsNullOrEmpty(ruhsat.ScannedFilePath))
                return NotFound("Dosya bulunamadı.");

            var filePath = Path.Combine(_scannedPath, ruhsat.ScannedFilePath);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _ruhsatService.UpdateScannedPdf(id, null);

            return Ok("Silindi");
        }

        [HttpGet("download-certificate")]
        public IActionResult GetCertificateHtml(int id)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var org = _userService.GetOrganizationById(user.OrganizationId);
            var ruhsat = _ruhsatService.GetRuhsatByIdFirst(user.OrganizationId, id);

            if (ruhsat == null)
                return NotFound("Ruhsat bulunamadı.");

            var dto = new RuhsatDto
            {
                Id = ruhsat.Id,
                TcKimlikNo = ruhsat.TcKimlikNo,
                Adi = ruhsat.Adi,
                Soyadi = ruhsat.Soyadi,
                IsyeriUnvani = ruhsat.IsyeriUnvani,
                FaaliyetKonusu = new FaaliyetKonusuDto { Name = ruhsat.FaaliyetKonusu?.Name },
                Adres = ruhsat.Adres,
                Ada = ruhsat.Ada,
                Pafta = ruhsat.Pafta,
                Parsel = ruhsat.Parsel,
                RuhsatTuru = new RuhsatTuruDto { Name = ruhsat.RuhsatTuru?.Name },
                RuhsatSinifi = ruhsat.RuhsatSinifi != null ? new RuhsatSinifiDto { Name = ruhsat.RuhsatSinifi.Name } : null,
                VerilisTarihi = ruhsat.VerilisTarihi,
                RuhsatNo = ruhsat.RuhsatNo,
                PhotoPath = _photoPath + "/" + ruhsat.PhotoPath,
                DepoBilgileri = ruhsat.DepoBilgi.Count != 0 ? ruhsat.DepoBilgi.Select(d => new DepoBilgiDto { Bilgi = d.Bilgi }).ToList() : null,
                BelName = org?.BelName,
                LogoUrl = org?.LogoUrl,
                BelBaskan = org?.BelBaskan,
                BelTitle = org?.BelTitle,
                Content1 = org?.Content1,
                Content2 = org?.Content2,
                Content3 = org?.Content3,
                Content4 = org?.Content4,
                BelBaskanTitle = org?.BelBaskanTitle,
            };

            var htmlContent = _pdfHelper.GenerateCertificateHtml(dto);
            return Content(htmlContent, "text/html");
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
