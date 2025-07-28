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

        public async Task<IEnumerable<RuhsatSinifi>> GetRuhsatSinifi(int organizationId, int ruhsatTuruId)
        {
            var data = _repository.FilterAsQueryable<RuhsatSinifi>(p => !p.IsDeleted && p.Organization.Id.Equals(organizationId) && p.RuhsatTuru.Id.Equals(ruhsatTuruId)).IncludeRuhsatSinifi();
            return data;
        }
        #endregion

        #region Depo
        public int AddDepo(int organizationId, string name, int ruhsatSinifiId)
        {
            var depo = new Depo
            {
                Adi = name,
                OrganizationId = organizationId,
                RuhsatSinifiId = ruhsatSinifiId,
                InsertedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(depo);
            return depo.Id;
        }

        public int UpdateDepo(int id, string name, int ruhsatSinifiId)
        {
            var depo = GetDepoById(id);
            if (depo != null)
            {
                depo.Adi = name;
                depo.RuhsatSinifiId = ruhsatSinifiId;
                depo.UpdateDate = DateTime.UtcNow;

                _repository.Update(depo);
                return depo.Id;
            }
            return 0;
        }

        public int IsDeletedDepo(int id)
        {
            var depo = GetDepoById(id);
            if (depo != null)
            {
                depo.IsDeleted = !depo.IsDeleted;
                depo.UpdateDate = DateTime.UtcNow;

                _repository.Update(depo);
                return depo.Id;
            }
            return 0;
        }

        public Depo GetDepoById(int id)
        {
            return _repository.GetById<Depo>(id);
        }

        public async Task<IEnumerable<Depo>> GetDepo(int organizationId)
        {
            var data = _repository.FilterAsQueryable<Depo>(p => !p.IsDeleted && !p.RuhsatSinifi.IsDeleted && p.Organization.Id.Equals(organizationId)).IncludeDepo();
            return data;
        }

        public async Task<IEnumerable<Depo>> GetDepo(int organizationId, int ruhsatSinifiId)
        {
            var data = _repository.FilterAsQueryable<Depo>(p => !p.IsDeleted && !p.RuhsatSinifi.IsDeleted && p.Organization.Id.Equals(organizationId) && p.RuhsatSinifi.Id.Equals(ruhsatSinifiId)).IncludeDepo();
            return data;
        }
        #endregion

        #region Ruhsat
        public int AddRuhsat(
            int organizationId, 
            int faaliyetKonusuId, 
            int ruhsatTuruId, 
            int? ruhsatSinifiId, 
            string ruhsatNo, 
            string tcKimlikNo, 
            string adi, 
            string soyadi, 
            string isyeriUnvani, 
            DateTime verilisTarihi, 
            string ada, 
            string parsel, 
            string pafta, 
            string adres, 
            string not, 
            string photoPath)
        {
            var ruhsat = new Core.Domain.Ruhsat.Ruhsat
            {
                OrganizationId = organizationId,
                FaaliyetKonusuId = faaliyetKonusuId,
                RuhsatTuruId = ruhsatTuruId,
                RuhsatSinifiId = ruhsatSinifiId,
                RuhsatNo = ruhsatNo,
                TcKimlikNo = tcKimlikNo,
                Adi = adi,
                Soyadi = soyadi,
                IsyeriUnvani = isyeriUnvani,
                VerilisTarihi = verilisTarihi,
                Ada = ada,
                Parsel = parsel,
                Pafta = pafta,
                Adres = adres,
                Not = not,
                PhotoPath = photoPath,
                InsertedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(ruhsat);
            return ruhsat.Id;
        }

        public int UpdateRuhsat(
            int id,
            int organizationId,
            int faaliyetKonusuId,
            int ruhsatTuruId,
            int? ruhsatSinifiId,
            string ruhsatNo,
            string tcKimlikNo,
            string adi,
            string soyadi,
            string isyeriUnvani,
            DateTime verilisTarihi,
            string ada,
            string parsel,
            string pafta,
            string adres,
            string not)
        {
            var ruhsat = GetRuhsatById(id);
            if (ruhsat != null)
            {
                ruhsat.FaaliyetKonusuId = faaliyetKonusuId;
                ruhsat.RuhsatTuruId = ruhsatTuruId;
                ruhsat.RuhsatSinifiId = ruhsatSinifiId;
                ruhsat.RuhsatNo = ruhsatNo;
                ruhsat.TcKimlikNo = tcKimlikNo;
                ruhsat.Adi = adi;
                ruhsat.Soyadi = soyadi;
                ruhsat.IsyeriUnvani = isyeriUnvani;
                ruhsat.VerilisTarihi = verilisTarihi;
                ruhsat.Ada = ada;
                ruhsat.Parsel = parsel;
                ruhsat.Pafta = pafta;
                ruhsat.Adres = adres;
                ruhsat.Not = not;
                ruhsat.UpdateDate = DateTime.UtcNow;

                _repository.Update(ruhsat);
                return ruhsat.Id;
            }
            return 0;
        }

        public int UpdatePhoto(int id, string fileName)
        {
            var ruhsat = GetRuhsatById(id);
            if (ruhsat != null)
            {
                ruhsat.PhotoPath = fileName;

                _repository.Update(ruhsat);
                return ruhsat.Id;
            }
            return 0;
        }

        public int IsActiveRuhsat(int id)
        {
            var ruhsat = GetRuhsatById(id);
            if (ruhsat != null)
            {
                ruhsat.IsActive = !ruhsat.IsActive;
                ruhsat.UpdateDate = DateTime.UtcNow;

                _repository.Update(ruhsat);
                return ruhsat.Id;
            }
            return 0;
        }

        public int IsDeletedRuhsat(int id)
        {
            var ruhsat = GetRuhsatById(id);
            if (ruhsat != null)
            {
                ruhsat.IsDeleted = !ruhsat.IsDeleted;
                ruhsat.UpdateDate = DateTime.UtcNow;

                _repository.Update(ruhsat);
                return ruhsat.Id;
            }
            return 0;
        }

        public Core.Domain.Ruhsat.Ruhsat GetRuhsatById(int id)
        {
            return _repository.GetById<Core.Domain.Ruhsat.Ruhsat>(id);
        }

        public async Task<IEnumerable<Core.Domain.Ruhsat.Ruhsat>> GetRuhsat(int organizationId)
        {
            var data = _repository.FilterAsQueryable<Core.Domain.Ruhsat.Ruhsat>(p => !p.IsDeleted 
                                && (p.RuhsatSinifi == null || !p.RuhsatSinifi.IsDeleted)
                                && !p.RuhsatTuru.IsDeleted 
                                && !p.FaaliyetKonusu.IsDeleted 
                                && p.Organization.Id.Equals(organizationId)).IncludeRuhsat();
            return data;
        }

        public Core.Domain.Ruhsat.Ruhsat GetRuhsatByIdFirst(int organizationId, int id)
        {
            return _repository
                .FilterAsQueryable<Core.Domain.Ruhsat.Ruhsat>(p =>
                    !p.IsDeleted &&
                    (p.RuhsatSinifi == null || !p.RuhsatSinifi.IsDeleted) &&
                    !p.RuhsatTuru.IsDeleted &&
                    !p.FaaliyetKonusu.IsDeleted &&
                    p.Organization.Id == organizationId &&
                    p.Id.Equals(id))
                .IncludeRuhsat()
                .FirstOrDefault();
        }
        #endregion

        #region DepoBilgi
        public int AddDepoBilgi(int organizationId, int ruhsatId, int depoId, string adi, string bilgi)
        {
            var ruhsat = new Core.Domain.Ruhsat.DepoBilgi
            {
                OrganizationId = organizationId,
                RuhsatId = ruhsatId,
                DepoId = depoId,
                DepoAdi = adi,
                Bilgi = bilgi,
                InsertedDate = DateTime.UtcNow,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(ruhsat);
            return ruhsat.Id;
        }

        public int IsDeletedDepoBilgi(int id)
        {
            var depoBilgi = GetDepoBilgiById(id);
            if (depoBilgi != null)
            {
                depoBilgi.IsDeleted = !depoBilgi.IsDeleted;
                depoBilgi.UpdateDate = DateTime.UtcNow;

                _repository.Update(depoBilgi);
                return depoBilgi.Id;
            }
            return 0;
        }

        public Core.Domain.Ruhsat.DepoBilgi GetDepoBilgiById(int id)
        {
            return _repository.GetById<Core.Domain.Ruhsat.DepoBilgi>(id);
        }

        public IEnumerable<Core.Domain.Ruhsat.DepoBilgi> GetDepoBilgi(int ruhsatId)
        {
            var data = _repository.FilterAsQueryable<Core.Domain.Ruhsat.DepoBilgi>(p => !p.IsDeleted && p.Ruhsat.Id.Equals(ruhsatId)).IncludeDepoBilgi();
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

        public static IQueryable<Depo> IncludeDepo(this IQueryable<Depo> query)
        {
            return query
                .Include(ma => ma.Organization)
                .Include(ma => ma.RuhsatSinifi);
        }

        public static IQueryable<DepoBilgi> IncludeDepoBilgi(this IQueryable<DepoBilgi> query)
        {
            return query
                .Include(ma => ma.Organization)
                .Include(ma => ma.Ruhsat);
        }

        public static IQueryable<Core.Domain.Ruhsat.Ruhsat> IncludeRuhsat(this IQueryable<Core.Domain.Ruhsat.Ruhsat> query)
        {
            return query
                .Include(ma => ma.Organization)
                .Include(ma => ma.RuhsatTuru)
                .Include(ma => ma.RuhsatSinifi)
                .Include(ma => ma.FaaliyetKonusu)
                .Include(x => x.DepoBilgi.Where(db => !db.IsDeleted));
        }
    }
}
