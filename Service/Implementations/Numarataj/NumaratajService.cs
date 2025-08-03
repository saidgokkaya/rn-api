using Core.Data;
using Core.Domain.Numarataj;
using Core.Domain.Ruhsat;
using Core.Domain.User;
using Microsoft.EntityFrameworkCore;
using Repository.Implementations;
using Service.Implementations.Ruhsat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations.Numarataj
{
    public class NumaratajService
    {
        private readonly Repository<Context> _repository;

        public NumaratajService()
        {
            _repository = new Repository<Context>(new Context());
        }

        #region Mahalle
        public int AddMahalle(int organizationId, string name)
        {
            var mahalle = new Mahalle
            {
                Name = name,
                OrganizationId = organizationId,
                InsertedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(mahalle);
            return mahalle.Id;
        }

        public int UpdateMahalle(int id, string name)
        {
            var mahalle = GetMahalleById(id);
            if (mahalle != null)
            {
                mahalle.Name = name;
                mahalle.UpdateDate = DateTime.UtcNow;

                _repository.Update(mahalle);
                return mahalle.Id;
            }
            return 0;
        }

        public int IsDeletedMahalle(int id)
        {
            var mahalle = GetMahalleById(id);
            if (mahalle != null)
            {
                mahalle.IsDeleted = !mahalle.IsDeleted;
                mahalle.UpdateDate = DateTime.UtcNow;

                _repository.Update(mahalle);
                return mahalle.Id;
            }
            return 0;
        }

        public Mahalle GetMahalleById(int id)
        {
            return _repository.GetById<Mahalle>(id);
        }

        public async Task<IEnumerable<Mahalle>> GetMahalle(int organizationId)
        {
            var data = _repository.FilterAsQueryable<Mahalle>(p => !p.IsDeleted && p.Organization.Id.Equals(organizationId)).IncludeMahalle();
            return data;
        }
        #endregion

        #region Numarataj
        public int AddNumarataj(int organizationId, string tcKimlikNo, string adSoyad, string telefon, string cadde, string disKapi, string icKapi, string siteAdi, 
            string eskiAdres, string blokAdi, string adresNo, string isYeriUnvani, string ada, string parsel, int type, int mahalleId)
        {
            var numarataj = new Core.Domain.Numarataj.Numarataj
            {
                TcKimlikNo = tcKimlikNo,
                AdSoyad = adSoyad,
                Telefon = telefon,
                CaddeSokak = cadde,
                DisKapi = disKapi,
                IcKapiNo = icKapi,
                SiteAdi = siteAdi,
                EskiAdres = eskiAdres,
                BlokAdi = blokAdi,
                AdresNo = adresNo,
                IsYeriUnvani = isYeriUnvani,
                Ada = ada,
                Parsel = parsel,
                NumaratajType = type,
                MahalleId = mahalleId,
                OrganizationId = organizationId,
                InsertedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(numarataj);
            return numarataj.Id;
        }

        public int UpdateNumarataj(int id, string tcKimlikNo, string adSoyad, string telefon, string cadde, string disKapi, string icKapi, string siteAdi,
            string eskiAdres, string blokAdi, string adresNo, string isYeriUnvani, string ada, string parsel, int type, int mahalleId)
        {
            var numarataj = GetNumaratajById(id);
            if (numarataj != null)
            {
                numarataj.TcKimlikNo = tcKimlikNo;
                numarataj.AdSoyad = adSoyad;
                numarataj.Telefon = telefon;
                numarataj.CaddeSokak = cadde;
                numarataj.DisKapi = disKapi;
                numarataj.IcKapiNo = icKapi;
                numarataj.SiteAdi = siteAdi;
                numarataj.EskiAdres = eskiAdres;
                numarataj.BlokAdi = blokAdi;
                numarataj.AdresNo = adresNo;
                numarataj.IsYeriUnvani = isYeriUnvani;
                numarataj.Ada = ada;
                numarataj.Parsel = parsel;
                numarataj.NumaratajType = type;
                numarataj.MahalleId = mahalleId;
                numarataj.UpdateDate = DateTime.UtcNow;

                _repository.Update(numarataj);
                return numarataj.Id;
            }
            return 0;
        }

        public int IsActiveNumarataj(int id)
        {
            var numarataj = GetNumaratajById(id);
            if (numarataj != null)
            {
                numarataj.IsActive = !numarataj.IsActive;
                numarataj.UpdateDate = DateTime.UtcNow;

                _repository.Update(numarataj);
                return numarataj.Id;
            }
            return 0;
        }

        public int IsDeletedNumarataj(int id)
        {
            var numarataj = GetNumaratajById(id);
            if (numarataj != null)
            {
                numarataj.IsDeleted = !numarataj.IsDeleted;
                numarataj.UpdateDate = DateTime.UtcNow;

                _repository.Update(numarataj);
                return numarataj.Id;
            }
            return 0;
        }

        public Core.Domain.Numarataj.Numarataj GetNumaratajById(int id)
        {
            return _repository.GetById<Core.Domain.Numarataj.Numarataj>(id);
        }

        public async Task<IEnumerable<Core.Domain.Numarataj.Numarataj>> GetNumarataj(int organizationId)
        {
            var data = _repository.FilterAsQueryable<Core.Domain.Numarataj.Numarataj>(p => !p.IsDeleted && p.Organization.Id.Equals(organizationId) && !p.Mahalle.IsDeleted).IncludeNumarataj();
            return data;
        }

        public async Task<IEnumerable<Core.Domain.Numarataj.Numarataj>> GetNumaratajForType(int organizationId, int type)
        {
            var data = _repository.FilterAsQueryable<Core.Domain.Numarataj.Numarataj>(p => !p.IsDeleted && p.Organization.Id.Equals(organizationId) && !p.Mahalle.IsDeleted && p.NumaratajType == type).IncludeNumarataj();
            return data;
        }

        public async Task<IEnumerable<Core.Domain.Numarataj.Numarataj>> GetNumaratajExcel(List<int> numberingIds, int organizationId)
        {
            var data = _repository
                .FilterAsQueryable<Core.Domain.Numarataj.Numarataj>(
                    p => !p.IsDeleted &&
                         numberingIds.Contains(p.Id) &&
                         !p.Mahalle.IsDeleted &&
                         p.Organization.Id.Equals(organizationId)
                )
                .IncludeNumarataj();

            return await data.ToListAsync();
        }

        public Core.Domain.Numarataj.Numarataj GetNumaratajByIdFirst(int organizationId, int id)
        {
            return _repository
                .FilterAsQueryable<Core.Domain.Numarataj.Numarataj>(p =>
                    !p.IsDeleted &&
                    !p.Mahalle.IsDeleted && 
                    p.Organization.Id == organizationId &&
                    p.Id.Equals(id))
                .IncludeNumarataj()
                .FirstOrDefault();
        }
        #endregion
    }

    public static class NumaratajExtensions
    {
        public static IQueryable<Mahalle> IncludeMahalle(this IQueryable<Mahalle> query)
        {
            return query
                .Include(ma => ma.Organization);
        }

        public static IQueryable<Core.Domain.Numarataj.Numarataj> IncludeNumarataj(this IQueryable<Core.Domain.Numarataj.Numarataj> query)
        {
            return query
                .Include(ma => ma.Organization)
                .Include(ma => ma.Mahalle);
        }
    }
}
