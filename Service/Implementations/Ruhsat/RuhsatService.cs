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
    }

    public static class RuhsatExtensions
    {
        public static IQueryable<FaaliyetKonusu> IncludeFaaliyetKonusu(this IQueryable<FaaliyetKonusu> query)
        {
            return query
                .Include(ma => ma.Organization);
        }
    }
}
