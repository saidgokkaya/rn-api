using Core.Data;
using Core.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service.Implementations.User
{
    public class UserService
    {
        private readonly Repository<Context> _repository;

        public UserService()
        {
            _repository = new Repository<Context>(new Context());
        }

        #region Organization
        public int AddOrganization(string name, string hashCode, int userCount = 5, int accountCount = 2, string accountType = "", string address = "", string zipCode = "", string taskNumber = "", string phone = "")
        {
            var organization = new Organization
            {
                Name = name,
                Address = address,
                ZipCode = zipCode,
                TaskNumber = taskNumber,
                Phone = phone,
                InsertedDate = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };

            _repository.Save(organization);
            return organization.Id;
        }

        public int UpdateOrganization(int id, string name, string address, string zipCode, string taskNumber)
        {
            var organization = GetOrganizationById(id);
            if (organization != null)
            {
                organization.Name = name;
                organization.Address = address;
                organization.ZipCode = zipCode;
                organization.TaskNumber = taskNumber;
                organization.UpdateDate = DateTime.Now;

                _repository.Update(organization);
                return organization.Id;
            }
            return 0;
        }

        public int UpdateCerceveOrganization(int id, int cerceve)
        {
            var organization = GetOrganizationById(id);
            if (organization != null)
            {
                organization.Cerceve = cerceve;
                organization.UpdateDate = DateTime.Now;

                _repository.Update(organization);
                return organization.Id;
            }
            return 0;
        }

		public int IsActiveOrganization(int id)
        {
            var organization = GetOrganizationById(id);
            if (organization != null)
            {
                organization.IsActive = !organization.IsActive;
                organization.UpdateDate = DateTime.Now;

                _repository.Update(organization);
                return organization.Id;
            }
            return 0;
        }

        public int IsDeletedOrganization(int id)
        {
            var organization = GetOrganizationById(id);
            if (organization != null)
            {
                organization.IsDeleted = !organization.IsDeleted;
                organization.UpdateDate = DateTime.Now;

                _repository.Update(organization);
                return organization.Id;
            }
            return 0;
        }

        public Organization GetOrganizationById(int id)
        {
            return _repository.GetById<Organization>(id);
        }

        public IEnumerable<Organization> GetOrganization()
        {
            var data = _repository.Filter<Organization>(p => p.IsActive && !p.IsDeleted);
            return data;
        }
		#endregion

		#region User
		public int AddUser(int organizationId, string firstName, string lastName, string mail, string phone, string userName, string password)
        {
            var organization = GetOrganizationById(organizationId);
            if (organization != null)
            {
                var user = new Core.Domain.User.User
                {
                    OrganizationId = organizationId,
                    FirstName = firstName,
                    LastName = lastName,
                    Mail = mail,
                    Phone = phone,
                    UserName = userName,
                    Password = password,
                    InsertedDate = DateTime.Now,
					IsActive = true,
                    IsDeleted = false
                };

                _repository.Save(user);
                return user.Id;
            }
            return 0;
        }

        public int UpdatePassword(int id, string password)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.Password = password;
                user.UpdateDate = DateTime.Now;

                _repository.Update(user);
                return user.Id;
            }
            return 0;
        }

        public int UpdatePhotoUrl(int id, string photoUrl)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.PhotoPath = photoUrl;
                user.UpdateDate = DateTime.Now;

                _repository.Update(user);
                return user.Id;
            }
            return 0;
        }

        public int UpdateUser(int id, string firstName, string lastName, string mail, string phone)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Mail = mail;
                user.Phone = phone;
                user.UpdateDate = DateTime.Now;

                _repository.Update(user);
                return user.Id;
            }
            return 0;
        }

		public int IsActiveUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                user.UpdateDate = DateTime.Now;

                _repository.Update(user);
                return user.Id;
            }
            return 0;
        }

        public int IsDeletedUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.IsDeleted = !user.IsDeleted;
                user.UpdateDate = DateTime.Now;

                _repository.Update(user);
                return user.Id;
            }
            return 0;
        }

        public Core.Domain.User.User GetUserById(int id)
        {
            return _repository.GetById<Core.Domain.User.User>(id);
        }

        public IEnumerable<Core.Domain.User.User> GetUser(int organizationId)
        {
            var data = _repository.FilterAsQueryable<Core.Domain.User.User>(p => !p.IsDeleted && p.Organization.Id.Equals(organizationId)).IncludeUser();
            return data;
        }

        public IEnumerable<Core.Domain.User.User> GetUsers(int organizationId, int userId)
        {
            var data = _repository
                .FilterAsQueryable<Core.Domain.User.User>(
                    p => !p.IsDeleted
                         && p.Organization.Id.Equals(organizationId)
                         && !p.Id.Equals(userId))
                .IncludeUser();
            return data;
        }

        public Core.Domain.User.User GetUserLogin(string mail, string password)
        {
            var data = _repository.FilterAsQueryable<Core.Domain.User.User>(p => p.IsActive && !p.IsDeleted)
                .IncludeUser()
                .FirstOrDefault(u => u.Mail == mail && u.Password == password);
            return data;
        }

		public IEnumerable<Core.Domain.User.User> GetUserCheckMail(string mail, int id)
		{
			var data = _repository
				.FilterAsQueryable<Core.Domain.User.User>(
					p => !p.IsDeleted && p.Mail == mail && p.Id != id)
				.IncludeUser();
			return data;
		}

		public IEnumerable<Core.Domain.User.User> GetUserCheckMailN(string mail)
		{
			var data = _repository
				.FilterAsQueryable<Core.Domain.User.User>(
					p => !p.IsDeleted && p.Mail == mail)
				.IncludeUser();
			return data;
		}
		#endregion

		#region Role
		public IEnumerable<Role> GetRole()
        {
            var data = _repository.FilterAsQueryable<Role>(x => true);
            return data;
        }

        public Role GetRoleById(int id)
        {
            return _repository.GetById<Role>(id);
        }
        #endregion

        #region UserRole
        public int AddUserRole(int userId, int roleId)
        {
            var user = GetUserById(userId);
            if (user != null)
            {
                var userRole = new Core.Domain.User.UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };

                _repository.Save(userRole);
                return 1;
            }
            return 0;
        }

        public int RemoveUserRolesByUserId(int userId)
        {
            var userRoles = _repository.FilterAsQueryable<UserRole>(ur => ur.UserId == userId).ToList();

            if (userRoles.Any())
            {
                foreach (var userRole in userRoles)
                {
                    _repository.Delete(userRole);
                }

                return 1;
            }

            return 0;
        }

        public IEnumerable<UserRole> GetUserRole(int userId)
        {
            var data = _repository.FilterAsQueryable<UserRole>(p => p.UserId.Equals(userId))
                .IncludeUserRole();
            return data;
        }
        #endregion
    }

    public static class UserExtensions
    {
        public static IQueryable<Core.Domain.User.User> IncludeUser(this IQueryable<Core.Domain.User.User> query)
        {
            return query
                .Include(ma => ma.UserRole)
                .Include(ma => ma.Organization);
        }

        public static IQueryable<UserRole> IncludeUserRole(this IQueryable<UserRole> query)
        {
            return query
                .Include(ma => ma.User)
                .Include(ma => ma.Role);
        }

        public static IQueryable<Role> IncludeRole(this IQueryable<Role> query)
        {
            return query
                .Include(ma => ma.UserRole);
        }
    }
}
