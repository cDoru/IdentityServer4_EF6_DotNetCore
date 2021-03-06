﻿using Domain;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Business
{
    public interface IUserManager
    {
        Task<Tuple<bool, string[]>> CreateRoleAsync(RoleDTO roleObj, IEnumerable<int> claims);
        Task<Tuple<bool, string[]>> CreateUserAsync(UserDTO userObj, IEnumerable<string> roles);
        Task<Tuple<bool, string[]>> LoginAsync(UserDTO user);
        UserDTO FindUserByID(UserDTO user);
        Task<Tuple<UserDTO, string[], List<string>>> FindUserRolesPermissions(UserDTO user);
    }
}
