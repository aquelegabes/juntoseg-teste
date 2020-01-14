using JuntoSeg.Application.ViewModel.Requests;
using JuntoSeg.Application.ViewModel.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuntoSeg.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResp> FindAsync(int id);
        UserResp Add(UserReq user);
        Task<bool> RemoveAsync(UserReq user);
        Task<UserResp> UpdateAsync(UserReq user);
        Task<string> GenerateValidationToken(int userId);
        bool ResetPassword(string validationToken, string newPassword);
    }
}
