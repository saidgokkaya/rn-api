using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helper
{
    public class PdfHelper
    {
        public string GenerateCertificateHtml(RuhsatDto model)
        {
            var logoUrl = model.LogoUrl;

            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html><head><meta charset='utf-8' />");
            sb.AppendLine("<title>Ruhsat Belgesi</title>");
            sb.AppendLine("<style>");
            sb.AppendLine(@"
                body { font-family: Arial, sans-serif; font-size: 12px; margin: 0; padding: 0; }
                .outer-border {
                    width: 100%;
                    max-width: 790px;
                    min-height: 1000px;
                    height: 1000px;
                    margin: 0 auto;
                    border: 1px solid #1D4EA4;
                    box-sizing: border-box;
                    display: flex;
                    flex-direction: column;
                }
                .main-border {
                    flex: 1;
                    padding: 2px;
                    border: 9px solid #1D4EA4;
                    box-sizing: border-box;
                }
                .inner-border {
                    flex: 1;
                    padding: 25px;
                    box-sizing: border-box;
                    position: relative;
                }
                .logo {
                    position: absolute;
                    top: -35px;
                    left: -35px;
                    width: 300px;
                    height: 200px;
                }
                .header-text {
                    font-weight: bold;
                    font-size: 14pt;
                    text-align: center;
                    margin-top: 30px;
                    margin-bottom: 60px;
                    line-height: 1.5;
                }
                .main-title {
                    text-align: center;
                    font-size: 20pt;
                    font-weight: 680;
                    margin-bottom: 30px;
                }
                .content-table td {
                    padding: 2px 5px;
                    font-size: 14px;
                }
                .label { font-weight: bold; white-space: nowrap; }
                .signature-block {
                    text-align: right;
                    margin-top: 130px;
                    margin-right: 40px;
                }
                .photo {
                    position: absolute;
                    top: 20px;
                    right: 15px;
                    width: 150px;
                    height: 180px;
                    border: 1px solid #000;
                    object-fit: cover;
                }
            ");
            sb.AppendLine("</style>");
            sb.AppendLine("</head><body>");

            sb.AppendLine("<div class='outer-border'><div class='main-border'><div class='inner-border'>");

            sb.AppendLine($"<img src='{logoUrl}' class='logo' alt='Logo' />");

            if (!string.IsNullOrEmpty(model.PhotoPath))
                sb.AppendLine($"<img class='photo' src='{model.PhotoPath}' alt='Fotoğraf' />");

            sb.AppendLine($"<div class='header-text'>{model.BelName}</div>");
            sb.AppendLine("<div class='main-title'>İŞYERİ AÇMA VE ÇALIŞMA RUHSATI</div>");

            sb.AppendLine("<table class='content-table'>");
            sb.AppendLine($"<tr><td class='label'>T.C. Kimlik No</td><td>: {model.TcKimlikNo}</td></tr>");
            sb.AppendLine($"<tr><td class='label'>Adı Soyadı</td><td>: {model.Adi} {model.Soyadi}</td></tr>");
            sb.AppendLine($"<tr><td class='label'>İşyeri Ünvanı</td><td>: {model.IsyeriUnvani}</td></tr>");
            sb.AppendLine($"<tr><td class='label'>Faaliyet Konusu</td><td>: {model.FaaliyetKonusu?.Name}</td></tr>");
            sb.AppendLine($"<tr><td class='label'>İşyerinin Adresi</td><td>: {model.Adres}</td></tr>");
            sb.AppendLine("</table><br/>");

            sb.AppendLine("<table class='content-table'>");
            sb.AppendLine($"<tr><td class='label'>Ada</td><td>: {model.Ada}</td></tr>");
            sb.AppendLine($"<tr><td class='label'>Pafta</td><td>: {model.Pafta}</td></tr>");
            sb.AppendLine($"<tr><td class='label'>Parsel</td><td>: {model.Parsel}</td></tr>");
            sb.AppendLine("</table><br/>");

            sb.AppendLine("<table class='content-table'>");
            sb.Append("<tr><td class='label'>İşyeri Sınıfı</td><td>: ");
            if (model.RuhsatTuru?.Name == "Gayrisıhhi Müessese")
                sb.Append($"{model.RuhsatSinifi?.Name}");
            else
                sb.Append("....sınıf");

            sb.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
            if (model.RuhsatTuru?.Name == "Sıhhi Müessese")
                sb.Append("Umuma Açık Müessese ( ) &nbsp;&nbsp;&nbsp;&nbsp; Sıhhi Müessese (X)");
            else
                sb.Append($"{model.RuhsatTuru?.Name} (X) &nbsp;&nbsp;&nbsp;&nbsp; Sıhhi Müessese ( )");

            sb.AppendLine("</td></tr></table><br/>");

            if (model.RuhsatTuru?.Name == "Gayrisıhhi Müessese" && model.DepoBilgileri?.Any() == true)
            {
                sb.AppendLine($"<div style='font-weight:bold;'>{model.Content4}</div>");
                for (int i = 0; i < model.DepoBilgileri.Count; i++)
                {
                    sb.AppendLine($"{i + 1}. Depo: {model.DepoBilgileri[i].Bilgi} m³<br/>");
                }

                sb.AppendLine($"<br/><div>{model.Content3}</div>");
            }

            sb.AppendLine("<br/><br/>");
            sb.AppendLine($"<div><strong>Ruhsat Tarihi:</strong> {model.VerilisTarihi:dd.MM.yyyy} &nbsp;&nbsp;&nbsp; <strong>Ruhsat No:</strong> {model.RuhsatNo}</div>");

            sb.AppendLine("<div class='signature-block'>");
            sb.AppendLine($"<div><strong>{model.BelBaskan}</strong></div>");
            sb.AppendLine($"<div><strong>{model.BelBaskanTitle}</strong></div>");
            sb.AppendLine($"<div>{model.BelTitle}</div><br/><br/>");
            sb.AppendLine("</div>");

            sb.AppendLine("<div style='font-size:10px;'>");
            sb.AppendLine($"{model.Content1}");
            if (model.RuhsatTuru?.Name == "Gayrisıhhi Müessese")
            {
                sb.AppendLine($"<br/>{model.Content2}");
            }
            sb.AppendLine("</div>");

            sb.AppendLine("</div></div></div></body></html>");

            return sb.ToString();
        }

        public string GenerateCertificateNumaratajHtml(NumaratajDto model)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"tr\">");
            sb.AppendLine("<head>");
            sb.AppendLine("  <meta charset=\"UTF-8\">");
            sb.AppendLine("  <title></title>");
            sb.AppendLine("  <style>");
            sb.AppendLine("    body { font-family: Arial, sans-serif; margin: 40px; }");
            sb.AppendLine("    .center { text-align: center; }");
            sb.AppendLine("    .header { font-size: 24px; font-weight: bold; }");
            sb.AppendLine("    .subheader { font-size: 20px; font-weight: bold; }");
            sb.AppendLine("    .section-title { background: #f0f0f0; padding: 8px; font-weight: bold; }");
            sb.AppendLine("    .table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
            sb.AppendLine("    .table td { padding: 8px; border: 1px solid #000; }");
            sb.AppendLine("    .table td:first-child {width: 30%; font-weight: bold;}");
            sb.AppendLine("    .watermark {");
            sb.AppendLine("      position: fixed;");
            sb.AppendLine("      top: 50%;");
            sb.AppendLine("      left: 50%;");
            sb.AppendLine("      transform: translate(-50%, -50%) rotate(-50deg);");
            sb.AppendLine("      font-size: 100px;");
            sb.AppendLine("      color: rgba(0, 0, 0, 0.05);");
            sb.AppendLine("      white-space: nowrap;");
            sb.AppendLine("      z-index: 0;");
            sb.AppendLine("      pointer-events: none;");
            sb.AppendLine("      user-select: none;");
            sb.AppendLine("    }");
            sb.AppendLine("    .main-title { font-size: 25px; }");
            sb.AppendLine("    .no-border td { border: none !important; }");
            sb.AppendLine("    .footer { font-size: 12px; margin-top: 30px; border: 1px solid #000; padding: 10px; }");
            sb.AppendLine("    .top-header { display: flex; justify-content: space-between; align-items: center; }");
            sb.AppendLine("    .logo { width: 110px; }");
            sb.AppendLine("    .title-block { flex-grow: 1; text-align: center; margin-left: -110px; }");
            sb.AppendLine("  </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("  <div class=\"watermark\">NUMARATAJ</div>");
            sb.AppendLine("  <div class=\"top-header\">");
            sb.AppendLine($"    <img src=\"{model.LogoUrl}\" class=\"logo\" style=\"float: left;\" />");
            sb.AppendLine("    <div class=\"title-block\">");
            sb.AppendLine($"{model.BelediyeAdi}");
            sb.AppendLine($"      <div>{model.BirimAdi}</div>");
            sb.AppendLine("    </div>");
            sb.AppendLine("  </div>");

            sb.AppendLine("  <br><br>");

            sb.AppendLine("  <table class=\"table no-border\">");
            sb.AppendLine("    <tr>");
            sb.AppendLine($"      <td style=\"text-align: left;\">Belge No: {model.BelgeNo}</td>");
            sb.AppendLine("      <td style=\"text-align: center; background-color: #d3d3d3; font-weight: bold;\">NUMARATAJ BELGESİ</td>");
            sb.AppendLine($"      <td style=\"text-align: right;\">Tarih: {model.Tarih:dd.MM.yyyy}</td>");
            sb.AppendLine("    </tr>");
            sb.AppendLine("  </table>");

            sb.AppendLine($"  <div class=\"section-title center main-title\" style=\"border: 1px solid #333; background-color: #d3d3d3; padding: 8px; border-radius: 4px; margin-bottom: 8px;\">{model.Baslik}</div>");

            sb.AppendLine("  <div class=\"section-title\">KİMLİK BİLGİLERİ</div>");
            sb.AppendLine("  <table class=\"table\">");
            if (model.Visibility && !string.IsNullOrWhiteSpace(model.TcKimlikNo))
            {
                sb.AppendLine($"    <tr><td>TC Kimlik No</td><td>{model.TcKimlikNo}</td></tr>");
            }
            sb.AppendLine($"    <tr><td>Ad/Soyad</td><td>{model.AdSoyad}</td></tr>");
            if (model.Visibility && !string.IsNullOrWhiteSpace(model.Telefon))
            {
                sb.AppendLine($"    <tr><td>Telefon</td><td>{model.Telefon}</td></tr>");
            }
            sb.AppendLine("  </table>");

            sb.AppendLine("  <div class=\"section-title\">ADRES BİLGİLERİ</div>");
            sb.AppendLine("  <table class=\"table\">");
            sb.AppendLine($"    <tr><td>Adres No (UAVT)</td><td>{model.AdresNo}</td></tr>");
            sb.AppendLine($"    <tr><td>Mahalle</td><td>{model.Mahalle}</td></tr>");
            sb.AppendLine($"    <tr><td>Cadde/Sokak/Bulvar</td><td>{model.CaddeSokak}</td></tr>");
            sb.AppendLine($"    <tr><td>Dış Kapı</td><td>{model.DisKapi}</td></tr>");
            sb.AppendLine($"    <tr><td>İç Kapı No</td><td>{model.IcKapiNo}</td></tr>");
            sb.AppendLine($"    <tr><td>Yapı/Site Adı</td><td>{model.SiteAdi}</td></tr>");
            sb.AppendLine("  </table>");

            sb.AppendLine("  <div class=\"section-title\">TAPU BİLGİLERİ</div>");
            sb.AppendLine("  <table class=\"table\">");
            sb.AppendLine($"    <tr><td>Ada/Parsel</td><td>{model.Ada} / {model.Parsel}</td></tr>");
            sb.AppendLine("  </table>");

            if (!string.IsNullOrWhiteSpace(model.EskiAdres))
            {
                sb.AppendLine("  <div class=\"section-title\">ADRES DEĞİŞİKLİĞİ BİLGİLERİ</div>");
                sb.AppendLine("  <table class=\"table\">");
                sb.AppendLine($"    <tr><td>Eski Adres</td><td>{model.EskiAdres}</td></tr>");
                sb.AppendLine("  </table>");
            }
            else
            {
                sb.AppendLine("  <br><br>");
                sb.AppendLine("  <br><br>");
            }

            sb.AppendLine("  <br><br>");
            sb.AppendLine("  <div class=\"center\">");
            sb.AppendLine($"    <strong>{model.PersonelAdi}</strong><br>");
            sb.AppendLine("    NUMARATAJ PERSONELİ");
            sb.AppendLine("  </div>");

            sb.AppendLine("  <div class=\"footer\">");
            sb.AppendLine($"    {model.Adres}");
            sb.AppendLine("  </div>");
            var guid = Guid.NewGuid().ToString();
            sb.AppendLine($"  <div style=\"position: fixed; bottom: 2px; left: 2px; font-size: 8px; color: #999; user-select: none;\">{guid}</div>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }

    public class RuhsatDto
    {
        public int Id { get; set; }

        public string TcKimlikNo { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }

        public string IsyeriUnvani { get; set; }

        public FaaliyetKonusuDto FaaliyetKonusu { get; set; }

        public string Adres { get; set; }

        public string Ada { get; set; }
        public string Pafta { get; set; }
        public string Parsel { get; set; }

        public RuhsatTuruDto RuhsatTuru { get; set; }
        public RuhsatSinifiDto RuhsatSinifi { get; set; }

        public DateTime VerilisTarihi { get; set; }
        public string RuhsatNo { get; set; }

        public string PhotoPath { get; set; }

        public List<DepoBilgiDto> DepoBilgileri { get; set; } = new();

        public string BelName { get; set; }
        public string LogoUrl { get; set; }
        public string BelBaskan { get; set; }
        public string BelBaskanTitle { get; set; }
        public string BelTitle { get; set; }
        public string Content1 { get; set; }
        public string Content2 { get; set; }
        public string Content3 { get; set; }
        public string Content4 { get; set; }
    }

    public class FaaliyetKonusuDto
    {
        public string Name { get; set; }
    }

    public class RuhsatTuruDto
    {
        public string Name { get; set; }
    }

    public class RuhsatSinifiDto
    {
        public string Name { get; set; }
    }

    public class DepoBilgiDto
    {
        public string Bilgi
        {
            get; set;
        }
    }

    public class NumaratajDto
    {
        public string LogoUrl { get; set; }
        public string BelediyeAdi { get; set; }
        public string BirimAdi { get; set; }
        public string BelgeNo { get; set; }
        public DateTime Tarih { get; set; }
        public string Baslik { get; set; }
        public string TcKimlikNo { get; set; }
        public string AdSoyad { get; set; }
        public string Telefon { get; set; }
        public string AdresNo { get; set; }
        public string Mahalle { get; set; }
        public string CaddeSokak { get; set; }
        public string DisKapi { get; set; }
        public string IcKapiNo { get; set; }
        public string SiteAdi { get; set; }
        public int Type { get; set; }
        public string IsYeriUnvani { get; set; }
        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string EskiAdres { get; set; }
        public string PersonelAdi { get; set; }
        public string Adres { get; set; }
        public bool Visibility { get; set; }
    }
}