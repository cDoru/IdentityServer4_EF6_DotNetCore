﻿using DataAccess.IdentiyModels;
using DataAccess.IdentiyModels.Managers;
using DataAccess.IdentiyModels.Utilities;
using Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class UserAccess : IUserAccess
    {
        private readonly TAD _context;

        ApplicationUserManager _appUserManager;
        ApplicationRoleManager _appRoleManager;

        public UserAccess(TAD context)
        {
            _context = context;
            _appUserManager = ApplicationUserManager.Create(_context);
            _appRoleManager = ApplicationRoleManager.Create(_context);
        }

        public async Task<Tuple<bool, string[]>> CreateRoleAsync(RoleDTO role, IEnumerable<string> claims)
        {
            try
            {
                ObjectMapper mapper = new ObjectMapper();
                ApplicationRole appRole = mapper.ConvertRoleToIdentityRole(role);

                var result = await _appRoleManager.CreateAsync(appRole);
                if (!result.Succeeded)
                    return Tuple.Create(false, result.Errors.ToArray());


                appRole = await _appRoleManager.FindByNameAsync(appRole.Name);

                if (claims != null)
                {
                    //foreach (string claim in claims.Distinct())
                    //{
                    //    result = await this._roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

                    //    if (!result.Succeeded)
                    //    {
                    //        await DeleteRoleAsync(role);
                    //        return Tuple.Create(false, result.Errors.Select(e => e.Description).ToArray());
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Tuple.Create(true, new string[] { role.Id });
        }

        public async Task<Tuple<bool, string[]>> CreateUserAsync(UserDTO user, IEnumerable<string> roles, string password)
        {

            try
            {
                ObjectMapper mapper = new ObjectMapper();
                ApplicationUser appUser = mapper.ConvertUserToIdentityUser(user);

                var result = await _appUserManager.CreateAsync(appUser, password); // for the first time , password is hard coded

                if (!result.Succeeded)
                {
                    return Tuple.Create(false, result.Errors.ToArray());

                }

                appUser = await _appUserManager.FindByNameAsync(appUser.UserName);

                try
                {
                    if (roles != null)
                    {
                        result = await _appUserManager.AddToRolesAsync(user.Id, roles.ToArray());
                    }
                }
                catch
                {
                    await DeleteUserAsync(appUser);
                    throw;
                }

                if (!result.Succeeded)
                {
                    await DeleteUserAsync(appUser);
                    return Tuple.Create(false, result.Errors.ToArray());
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return Tuple.Create(true, new string[] { user.Id });


        }

        public async Task<Tuple<bool, string[]>> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _appUserManager.DeleteAsync(user);
            return Tuple.Create(result.Succeeded, result.Errors.ToArray());
        }

        public async Task<Tuple<bool, string[]>> DeleteRoleAsync(ApplicationRole role)
        {
            var result = await _appRoleManager.DeleteAsync(role);
            return Tuple.Create(result.Succeeded, result.Errors.ToArray());
        }

        public async Task<Tuple<bool, string[]>> DeleteRoleAsync(string roleName)
        {
            var role = await _appRoleManager.FindByNameAsync(roleName);

            if (role != null)
                return await DeleteRoleAsync(role);

            return Tuple.Create(true, new string[] { });
        }

        public async Task<Tuple<bool, string[]>> DeleteUserAsync(string userId)
        {
            var user = await _appUserManager.FindByIdAsync(userId);

            if (user != null)
                return await DeleteUserAsync(user);

            return Tuple.Create(true, new string[] { });
        }
    }
}
