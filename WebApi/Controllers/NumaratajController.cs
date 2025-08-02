using Core.Domain.Numarataj;
using Core.Domain.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Implementations.Numarataj;
using Service.Implementations.Ruhsat;
using Service.Implementations.User;
using System.Drawing;
using System.Reflection.Metadata;
using System.Xml.Linq;
using Utilities.Helper;
using WebApi.Models.Numarataj;
using WebApi.Models.Ruhsat;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NumaratajController : ControllerBase
    {
        private readonly ILogger<NumaratajController> _logger;
        private readonly UserService _userService;
        private readonly NumaratajService _numaratajService;
        private readonly DefaultValues _defaultValues;
        private readonly PdfHelper _pdfHelper;

        public NumaratajController(ILogger<NumaratajController> logger)
        {
            _logger = logger;
            _userService = new UserService();
            _numaratajService = new NumaratajService();
            _defaultValues = new DefaultValues();
            _pdfHelper = new PdfHelper();
        }

        [HttpGet("mahalles")]
        public async Task<ActionResult<IEnumerable<MahalleDto>>> GetMahalles()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var mahalles = await _numaratajService.GetMahalle(user.OrganizationId);

            var mahallesList = mahalles.Select(mahalle => new MahalleDto
            {
                Id = mahalle.Id,
                Name = mahalle.Name
            }).ToList();

            return Ok(mahallesList);
        }

        [HttpPost]
        [Route("add-mahalles")]
        public async Task<IActionResult> AddMahalles([FromBody] AddMahallesRequest request)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);

            var existingMahalles = await _numaratajService.GetMahalle(user.OrganizationId);

            var incomingMahalles = request.Name;

            var templatesToUpdate = new List<MahalleDto>();
            var templatesToAdd = new List<MahalleDto>();

            foreach (var incoming in incomingMahalles)
            {
                if (incoming.Id.HasValue)
                {
                    var match = existingMahalles.FirstOrDefault(e => e.Id == incoming.Id.Value);
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

            var incomingIds = incomingMahalles.Where(x => x.Id.HasValue).Select(x => x.Id.Value).ToList();
            var templatesToRemove = existingMahalles
                .Where(e => !incomingIds.Contains(e.Id))
                .ToList();

            foreach (var add in templatesToAdd)
            {
                _numaratajService.AddMahalle(user.OrganizationId, add.Name);
            }

            foreach (var update in templatesToUpdate)
            {
                _numaratajService.UpdateMahalle(update.Id.Value, update.Name);
            }

            foreach (var delete in templatesToRemove)
            {
                _numaratajService.IsDeletedMahalle(delete.Id);
            }

            return Ok(1);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddNumarataj([FromBody] AddNumarataj model)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);

            _numaratajService.AddNumarataj(user.OrganizationId, model.TcKimlikNo, model.AdSoyad, model.Telefon, model.CaddeSokak, model.DisKapi,
                model.IcKapiNo, model.SiteAdi, model.EskiAdres, model.BlokAdi, model.AdresNo, model.IsYeriUnvani, model.Ada, model.Parsel, model.NumaratajType, model.MahalleId);

            return Ok(1);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateNumarataj([FromBody] UpdateNumarataj model)
        {
            _numaratajService.UpdateNumarataj(model.Id, model.TcKimlikNo, model.AdSoyad, model.Telefon, model.CaddeSokak, model.DisKapi,
                model.IcKapiNo, model.SiteAdi, model.EskiAdres, model.BlokAdi, model.AdresNo, model.IsYeriUnvani, model.Ada, model.Parsel, model.NumaratajType, model.MahalleId);

            return Ok(1);
        }

        [HttpGet("numberings")]
        public async Task<ActionResult<IEnumerable<Models.Numarataj.Numarataj>>> GetNumbering()
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var numberings = await _numaratajService.GetNumarataj(user.OrganizationId);

            var numberingsList = numberings.Select(numbering => new Models.Numarataj.Numarataj
            {
                Id = numbering.Id,
                TcKimlikNo = numbering.TcKimlikNo,
                IsActive = numbering.IsActive ? "Aktif" : "Pasif",
                AdSoyad = numbering.AdSoyad,
                Telefon = numbering.Telefon,
                InsertedDate = numbering.InsertedDate,
                CaddeSokak = numbering.CaddeSokak,
                DisKapi = numbering.DisKapi,
                IcKapiNo = numbering.IcKapiNo,
                Mahalle = numbering.Mahalle.Name,
                NumaratajType = numbering.NumaratajType switch
                {
                    0 => "Özel İşyeri",
                    1 => "Resmi Kurum",
                    2 => "Yeni Bina",
                    3 => "Saha Çalışması",
                    4 => "Adres Tespit",
                    _ => "Bilinmeyen"
                }
            }).ToList();

            return Ok(numberingsList);
        }

        [HttpGet("numberings-for-type")]
        public async Task<ActionResult<IEnumerable<Models.Numarataj.Numarataj>>> GetNumberingForType(int numType)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var numberings = await _numaratajService.GetNumaratajForType(user.OrganizationId, numType);

            var numberingsList = numberings.Select(numbering => new Models.Numarataj.Numarataj
            {
                Id = numbering.Id,
                TcKimlikNo = numbering.TcKimlikNo,
                IsActive = numbering.IsActive ? "Aktif" : "Pasif",
                AdSoyad = numbering.AdSoyad,
                Telefon = numbering.Telefon,
                InsertedDate = numbering.InsertedDate,
                CaddeSokak = numbering.CaddeSokak,
                DisKapi = numbering.DisKapi,
                IcKapiNo = numbering.IcKapiNo,
                Mahalle = numbering.Mahalle.Name,
            }).ToList();

            return Ok(numberingsList);
        }

        [HttpPost("numbering-status")]
        public async Task<IActionResult> NumberingStatus(int id)
        {
            var status = _numaratajService.IsActiveNumarataj(id);
            if (status == 0)
            {
                return BadRequest();
            }
            return Ok(1);
        }

        [HttpPost("delete-numbering")]
        public IActionResult DeleteNumbering(int id)
        {
            var deleteNumbering = _numaratajService.IsDeletedNumarataj(id);
            if (deleteNumbering == 0)
            {
                return Ok(new { success = false });
            }
            return Ok(new { success = true });
        }

        [HttpPost("delete-numberings")]
        public IActionResult DeleteNumberings([FromBody] DeleteNumberings numberings)
        {
            foreach (var numbering in numberings.NumberingIds)
            {
                var deleteNumbering = _numaratajService.IsDeletedNumarataj(numbering);
                if (deleteNumbering == 0)
                {
                    return Ok(new { success = false });
                }
            }
            return Ok(new { success = true });
        }

        [HttpGet("numbering-by-id")]
        public IActionResult GetById(int id)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var numarataj = _numaratajService.GetNumaratajByIdFirst(user.OrganizationId, id);

            if (numarataj == null)
                return NotFound("Numarataj bulunamadı.");

            var dto = new NumaratajDetailDto
            {
                Id = numarataj.Id,
                TcKimlikNo = numarataj.TcKimlikNo,
                AdSoyad = numarataj.AdSoyad,
                Telefon = numarataj.Telefon,
                CaddeSokak = numarataj.CaddeSokak,
                DisKapi = numarataj.DisKapi,
                IcKapiNo = numarataj.IcKapiNo,
                SiteAdi = numarataj.SiteAdi,
                EskiAdres = numarataj.EskiAdres,
                BlokAdi = numarataj.BlokAdi,
                AdresNo = numarataj.AdresNo,
                IsYeriUnvani = numarataj.IsYeriUnvani,
                Ada = numarataj.Ada,
                Parsel = numarataj.Parsel,
                NumaratajType = numarataj.NumaratajType,
                MahalleId = numarataj.MahalleId
            };

            return Ok(dto);
        }

        [HttpGet("download-certificate")]
        public IActionResult GetCertificateHtml(int id)
        {
            var userId = UserId();
            var user = _userService.GetUserById(userId);
            var org = _userService.GetOrganizationById(user.OrganizationId);
            var numarataj = _numaratajService.GetNumaratajByIdFirst(user.OrganizationId, id);

            if (numarataj == null)
                return NotFound("Numarataj bulunamadı.");

            var dto = new NumaratajDto
            {
                LogoUrl = org.LogoUrl,
                BelediyeAdi = org.NumaratajBelName,
                Baslik = numarataj.NumaratajType switch
                {
                    0 => "Özel İşyeri",
                    1 => "Resmi Kurum",
                    2 => "Yeni Bina",
                    3 => "Saha Çalışması",
                    4 => "Adres Tespit",
                    _ => "Bilinmeyen"
                },
                BelgeNo = numarataj.Id.ToString(),
                Tarih = numarataj.InsertedDate.Value,
                TcKimlikNo = numarataj.TcKimlikNo,
                AdSoyad = numarataj.AdSoyad,
                Telefon = numarataj.Telefon,
                AdresNo = numarataj.AdresNo,
                Mahalle = numarataj.Mahalle.Name,
                CaddeSokak = numarataj.CaddeSokak,
                DisKapi = numarataj.DisKapi,
                IcKapiNo = numarataj.IcKapiNo,
                SiteAdi = numarataj.SiteAdi,
                Type = numarataj.NumaratajType,
                IsYeriUnvani = numarataj.IsYeriUnvani,
                Ada = numarataj.Ada,
                Parsel = numarataj.Parsel,
                EskiAdres = numarataj.EskiAdres,
                PersonelAdi = user.FirstName + " " + user.LastName,
                BirimAdi = org.NumaratajPersonName,
                Adres = org.Address
            };

            var htmlContent = _pdfHelper.GenerateCertificateNumaratajHtml(dto);
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
