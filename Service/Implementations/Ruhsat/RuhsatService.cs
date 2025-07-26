using Core.Data;
using Core.Domain.Ruhsat;
using Core.Domain.User;
using Microsoft.EntityFrameworkCore;
using Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations.Ruhsat
{
    public class RuhsatService
    {
        private readonly Repository<Context> _repository;

        public RuhsatService()
        {
            _repository = new Repository<Context>(new Context());
        }

        #region Faaliyet
        public int AddFaaliyetKonusu(int organizationId, string name)
        {
            var faaliyetKonusu = new FaaliyetKonusu
            {
                Name = name,
                OrganizationId = organizationId,
                InsertedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(faaliyetKonusu);
            return faaliyetKonusu.Id;
        }

        public int UpdateFaaliyetKonusu(int id, string name)
        {
            var faaliyetKonusu = GetFaaliyetKonusuById(id);
            if (faaliyetKonusu != null)
            {
                faaliyetKonusu.Name = name;
                faaliyetKonusu.UpdateDate = DateTime.UtcNow;

                _repository.Update(faaliyetKonusu);
                return faaliyetKonusu.Id;
            }
            return 0;
        }

        public int IsDeletedFaaliyetKonusu(int id)
        {
            var faaliyetKonusu = GetFaaliyetKonusuById(id);
            if (faaliyetKonusu != null)
            {
                faaliyetKonusu.IsDeleted = !faaliyetKonusu.IsDeleted;
                faaliyetKonusu.UpdateDate = DateTime.UtcNow;

                _repository.Update(faaliyetKonusu);
                return faaliyetKonusu.Id;
            }
            return 0;
        }

        public FaaliyetKonusu GetFaaliyetKonusuById(int id)
        {
            return _repository.GetById<FaaliyetKonusu>(id);
        }

        public async Task<IEnumerable<FaaliyetKonusu>> GetFaaliyetKonusu(int organizationId)
        {
            var data = _repository.FilterAsQueryable<FaaliyetKonusu>(p => !p.IsDeleted && p.Organization.Id.Equals(organizationId)).IncludeFaaliyetKonusu();
            return data;
        }
        #endregion

        #region RuhsatTürü
        public async Task<IEnumerable<RuhsatTuru>> GetRuhsatTuru()
        {
            var data = _repository.FilterAsQueryable<RuhsatTuru>(p => !p.IsDeleted);
            return data;
        }
        #endregion

        #region RuhsatSınıfı
        public int AddRuhsatSinifi(int organizationId, string name, int ruhsatTuruId)
        {
            var ruhsatSinifi = new RuhsatSinifi
            {
                Name = name,
                OrganizationId = organizationId,
                RuhsatTuruId = ruhsatTuruId,
                InsertedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(ruhsatSinifi);
            return ruhsatSinifi.Id;
        }

        public int UpdateRuhsatSinifi(int id, string name, int ruhsatTuruId)
        {
            var ruhsatSinifi = GetRuhsatSinifiById(id);
            if (ruhsatSinifi != null)
            {
                ruhsatSinifi.Name = name;
                ruhsatSinifi.RuhsatTuruId = ruhsatTuruId;
                ruhsatSinifi.UpdateDate = DateTime.UtcNow;

                _repository.Update(ruhsatSinifi);
                return ruhsatSinifi.Id;
            }
            return 0;
        }

        public int IsDeletedRuhsatSinifi(int id)
        {
            var ruhsatSinifi = GetRuhsatSinifiById(id);
            if (ruhsatSinifi != null)
            {
                ruhsatSinifi.IsDeleted = !ruhsatSinifi.IsDeleted;
                ruhsatSinifi.UpdateDate = DateTime.UtcNow;

                _repository.Update(ruhsatSinifi);
                return ruhsatSinifi.Id;
            }
            return 0;
        }

        public RuhsatSinifi GetRuhsatSinifiById(int id)
        {
            return _repository.GetById<RuhsatSinifi>(id);
        }

        public async Task<IEnumerable<RuhsatSinifi>> GetRuhsatSinifi(int organizationId)
        {
            var data = _repository.FilterAsQueryable<RuhsatSinifi>(p => !p.IsDeleted && p.Organization.Id.Equals(organizationId)).IncludeRuhsatSinifi();
            return data;
        }
        #endregion
    }

    public static class RuhsatExtensions
    {
        public static IQueryable<FaaliyetKonusu> IncludeFaaliyetKonusu(this IQueryable<FaaliyetKonusu> query)
        {
            return query
                .Include(ma => ma.Organization);
        }

        public static IQueryable<RuhsatSinifi> IncludeRuhsatSinifi(this IQueryable<RuhsatSinifi> query)
        {
            return query
                .Include(ma => ma.Organization)
                .Include(ma => ma.RuhsatTuru);
        }
    }
}
