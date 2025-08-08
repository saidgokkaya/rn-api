using Core.Data;
using Microsoft.EntityFrameworkCore;
using Repository.Implementations;
using Service.Implementations.Numarataj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations.Log
{
    public class LogService
    {
        private readonly Repository<Context> _repository;

        public LogService()
        {
            _repository = new Repository<Context>(new Context());
        }

        #region Log
        public int AddLog(int organizationId, int userId, string moduleName, string processName, string baseModule, int? tableId)
        {
            var log = new Core.Domain.User.Log
            {
                ModuleName = moduleName,
                ProcessName = processName,
                BaseModule = baseModule,
                TableId = tableId,
                UserId = userId,
                OrganizationId = organizationId,
                InsertedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(log);
            return log.Id;
        }

        public async Task<IEnumerable<Core.Domain.User.Log>> GetLogs(int organizationId)
        {
            var data = _repository.FilterAsQueryable<Core.Domain.User.Log>(p => !p.IsDeleted && p.Organization.Id.Equals(organizationId)).IncludeLog();
            return data;
        }

        public async Task<IEnumerable<Core.Domain.User.Log>> GetLogExcel(List<int> logIds, int organizationId)
        {
            var data = _repository
                .FilterAsQueryable<Core.Domain.User.Log>(
                    p => !p.IsDeleted &&
                         logIds.Contains(p.Id) &&
                         p.Organization.Id.Equals(organizationId)
                )
                .IncludeLog();

            return await data.ToListAsync();
        }
        #endregion
    }

    public static class LogExtensions
    {
        public static IQueryable<Core.Domain.User.Log> IncludeLog(this IQueryable<Core.Domain.User.Log> query)
        {
            return query
                .Include(ma => ma.Organization)
                .Include(ma => ma.User);
        }
    }
}
