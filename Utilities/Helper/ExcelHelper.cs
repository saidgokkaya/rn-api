using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helper
{
    public class ExcelHelper
    {
        public byte[] GenerateExcelFile(List<NumaratajExcelDto> numbering)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Numarataj Verileri");

                worksheet.Cell(1, 1).Value = "Belge No";
                worksheet.Cell(1, 2).Value = "TC Kimlik No";
                worksheet.Cell(1, 3).Value = "Tarih";
                worksheet.Cell(1, 4).Value = "Ad Soyad";
                worksheet.Cell(1, 5).Value = "Telefon";
                worksheet.Cell(1, 6).Value = "Cadde / Sokak";
                worksheet.Cell(1, 7).Value = "Dış Kapı";
                worksheet.Cell(1, 8).Value = "İç Kapı No";
                worksheet.Cell(1, 9).Value = "Site Adı";
                worksheet.Cell(1, 10).Value = "Eski Adres";
                worksheet.Cell(1, 11).Value = "Blok Adı";
                worksheet.Cell(1, 12).Value = "Adres No";
                worksheet.Cell(1, 13).Value = "İşyeri Ünvanı";
                worksheet.Cell(1, 14).Value = "Ada";
                worksheet.Cell(1, 15).Value = "Parsel";
                worksheet.Cell(1, 16).Value = "Numarataj Tipi";
                worksheet.Cell(1, 17).Value = "Mahalle";

                for (int i = 0; i < numbering.Count; i++)
                {
                    var row = i + 2;
                    var item = numbering[i];

                    worksheet.Cell(row, 1).Value = item.BelgeNo;
                    worksheet.Cell(row, 2).Value = item.TcKimlikNo;
                    worksheet.Cell(row, 3).Value = item.Tarih.ToString("dd.MM.yyyy");
                    worksheet.Cell(row, 4).Value = item.AdSoyad;
                    worksheet.Cell(row, 5).Value = item.Telefon;
                    worksheet.Cell(row, 6).Value = item.CaddeSokak;
                    worksheet.Cell(row, 7).Value = item.DisKapi;
                    worksheet.Cell(row, 8).Value = item.IcKapiNo;
                    worksheet.Cell(row, 9).Value = item.SiteAdi;
                    worksheet.Cell(row, 10).Value = item.EskiAdres;
                    worksheet.Cell(row, 11).Value = item.BlokAdi;
                    worksheet.Cell(row, 12).Value = item.AdresNo;
                    worksheet.Cell(row, 13).Value = item.IsYeriUnvani;
                    worksheet.Cell(row, 14).Value = item.Ada;
                    worksheet.Cell(row, 15).Value = item.Parsel;
                    worksheet.Cell(row, 16).Value = item.NumaratajType;
                    worksheet.Cell(row, 17).Value = item.Mahalle;
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public byte[] GenerateExcelNumTypeFile(List<NumaratajExcelDto> numbering)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Numarataj Verileri");

                if (numbering == null || !numbering.Any())
                    return Array.Empty<byte>();

                var numType = numbering.First().numType;

                var columnMap = GetColumnMap(numType);

                for (int col = 0; col < columnMap.Count; col++)
                {
                    worksheet.Cell(1, col + 1).Value = columnMap[col].Header;
                }

                for (int i = 0; i < numbering.Count; i++)
                {
                    var item = numbering[i];
                    for (int j = 0; j < columnMap.Count; j++)
                    {
                        var cellValue = columnMap[j].Selector(item);

                        if (cellValue == null)
                            worksheet.Cell(i + 2, j + 1).Value = "";
                        else if (cellValue is int || cellValue is double || cellValue is decimal || cellValue is float)
                            worksheet.Cell(i + 2, j + 1).Value = Convert.ToDouble(cellValue);
                        else if (cellValue is DateTime)
                            worksheet.Cell(i + 2, j + 1).Value = (DateTime)cellValue;
                        else if (cellValue is bool)
                            worksheet.Cell(i + 2, j + 1).Value = (bool)cellValue;
                        else
                            worksheet.Cell(i + 2, j + 1).Value = cellValue.ToString();
                    }
                }

                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        private List<(string Header, Func<NumaratajExcelDto, object> Selector)> GetColumnMap(int numType)
        {
            switch (numType)
            {
                case 0:
                    return new List<(string, Func<NumaratajExcelDto, object>)> {
                    ("Belge No", x => x.BelgeNo),
                    ("TC Kimlik No", x => x.TcKimlikNo),
                    ("Tarih", x => x.Tarih.ToString("dd.MM.yyyy")),
                    ("Ad Soyad", x => x.AdSoyad),
                    ("Telefon", x => x.Telefon),
                    ("Mahalle", x => x.Mahalle),
                    ("Cadde / Sokak", x => x.CaddeSokak),
                    ("Dış Kapı", x => x.DisKapi),
                    ("İç Kapı No", x => x.IcKapiNo),
                    ("Site Adı", x => x.SiteAdi),
                    ("Ada", x => x.Ada),
                    ("Parsel", x => x.Parsel),
                    ("Adres No", x => x.AdresNo),
                    ("İşyeri Ünvanı", x => x.IsYeriUnvani),
                    ("Blok Adı", x => x.BlokAdi),
                };
                case 1:
                    return new List<(string, Func<NumaratajExcelDto, object>)> {
                    ("Belge No", x => x.BelgeNo),
                    ("TC Kimlik No", x => x.TcKimlikNo),
                    ("Tarih", x => x.Tarih.ToString("dd.MM.yyyy")),
                    ("Ad Soyad", x => x.AdSoyad),
                    ("Telefon", x => x.Telefon),
                    ("Mahalle", x => x.Mahalle),
                    ("Cadde / Sokak", x => x.CaddeSokak),
                    ("Dış Kapı", x => x.DisKapi),
                    ("İç Kapı No", x => x.IcKapiNo),
                    ("Ada", x => x.Ada),
                    ("Parsel", x => x.Parsel),
                    ("Adres No", x => x.AdresNo),
                };
                case 2:
                    return new List<(string, Func<NumaratajExcelDto, object>)> {
                    ("Belge No", x => x.BelgeNo),
                    ("TC Kimlik No", x => x.TcKimlikNo),
                    ("Tarih", x => x.Tarih.ToString("dd.MM.yyyy")),
                    ("Ad Soyad", x => x.AdSoyad),
                    ("Telefon", x => x.Telefon),
                    ("Mahalle", x => x.Mahalle),
                    ("Cadde / Sokak", x => x.CaddeSokak),
                    ("Dış Kapı", x => x.DisKapi),
                    ("Ada", x => x.Ada),
                    ("Parsel", x => x.Parsel),
                    ("Blok Adı", x => x.BlokAdi),
                };
                case 3:
                    return new List<(string, Func<NumaratajExcelDto, object>)> {
                    ("Belge No", x => x.BelgeNo),
                    ("Tarih", x => x.Tarih.ToString("dd.MM.yyyy")),
                    ("Ad Soyad", x => x.AdSoyad),
                    ("Mahalle", x => x.Mahalle),
                    ("Cadde / Sokak", x => x.CaddeSokak),
                    ("Dış Kapı", x => x.DisKapi),
                    ("İç Kapı No", x => x.IcKapiNo),
                    ("Ada", x => x.Ada),
                    ("Parsel", x => x.Parsel),
                };
                case 4:
                    return new List<(string, Func<NumaratajExcelDto, object>)> {
                    ("Belge No", x => x.BelgeNo),
                    ("TC Kimlik No", x => x.TcKimlikNo),
                    ("Tarih", x => x.Tarih.ToString("dd.MM.yyyy")),
                    ("Ad Soyad", x => x.AdSoyad),
                    ("Telefon", x => x.Telefon),
                    ("Mahalle", x => x.Mahalle),
                    ("Cadde / Sokak", x => x.CaddeSokak),
                    ("Dış Kapı", x => x.DisKapi),
                    ("İç Kapı No", x => x.IcKapiNo),
                    ("Site Adı", x => x.SiteAdi),
                    ("Eski Adres", x => x.EskiAdres),
                    ("Ada", x => x.Ada),
                    ("Parsel", x => x.Parsel),
                    ("Adres No", x => x.AdresNo),
                };
                default:
                    return new List<(string, Func<NumaratajExcelDto, object>)> {
                    ("Ad Soyad", x => x.AdSoyad)
                };
            }
        }
    }

    public class NumaratajExcelDto
    {
        public string BelgeNo { get; set; }
        public DateTime Tarih { get; set; }
        public string TcKimlikNo { get; set; }
        public string AdSoyad { get; set; }
        public string Telefon { get; set; }
        public string AdresNo { get; set; }
        public string Mahalle { get; set; }
        public string CaddeSokak { get; set; }
        public string DisKapi { get; set; }
        public string IcKapiNo { get; set; }
        public string SiteAdi { get; set; }
        public string IsYeriUnvani { get; set; }
        public string Ada { get; set; }
        public string Parsel { get; set; }
        public string EskiAdres { get; set; }
        public string BlokAdi { get; set; }
        public string NumaratajType { get; set; }
        public int numType { get; set; }
    }
}
