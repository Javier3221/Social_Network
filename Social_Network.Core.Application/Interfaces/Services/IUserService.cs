﻿using Social_Network.Core.Application.ViewModels.User;
using Social_Network.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social_Network.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<SaveUserViewModel, UserViewModel, User>
    {
        Task<UserViewModel> Login(LoginViewModel loginVm);
        Task<bool> FindUserNameAvailabilty(string userName);
        Task<List<UserViewModel>> GetAllViewModelWithInclude();
        Task<UserViewModel> FindUserByUserName(string userName);
        Task<UserViewModel> GetByIdWithInclude(int id);
        Task<SaveUserViewModel> FindUserByEmail(string email);
        Task ActivateAccount(SaveUserViewModel user);
        Task ChangePassword(UserViewModel user);
    }
}
