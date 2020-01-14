using AutoMapper;
using JuntoSeg.Application.Interfaces;
using JuntoSeg.Application.ViewModel;
using JuntoSeg.Application.ViewModel.Requests;
using JuntoSeg.Application.ViewModel.Response;
using JuntoSeg.Domain.Entities;
using JuntoSeg.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static JuntoSeg.Common.HashString;

namespace JuntoSeg.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserService(
            IUnitOfWork uow,
            IMapper mapper)
        {
            this._uow = uow;
            this._mapper = mapper;
        }

        public async Task<string> GenerateValidationToken(int userId)
        {
            try
            {
                var userModel = await _uow.Users.FindAsync(userId);
                var token = ValidationToken.GenerateToken();
                token.User = userModel ?? throw new ArgumentNullException();

                _uow.Tokens.Add(token);
                _uow.Commit();

                return token.Token;
            }
            catch { throw; }
        }

        public bool ResetPassword(string validationToken, string newPassword)
        {
            try
            {
                var token = _uow.Tokens.Query.First(f => f.Token.Equals(validationToken));
                if (token.CreatedAt < DateTime.UtcNow.Add(token.ExpirationSpan) 
                    || !token.IsUsed)
                    return false;

                var user = token.User;
                user.Passw = CreateHashString(newPassword);
                token.IsUsed = true;

                _uow.Tokens.Update(token);
                _uow.Users.Update(user);
                _uow.Commit();
                return true;
            }
            catch { return false; }
        }

        public UserResp Add(UserReq user)
        {
            var userModel = _mapper.Map<User>(user);
            userModel.Passw = CreateHashString(user.NotHashedPassword);

            _uow.Users.Add(userModel);
            _uow.Commit();

            return _mapper.Map<UserResp>(userModel);
        }

        public async Task<UserResp> FindAsync(int id)
        {
            var user = await _uow.Users.FindAsync(id);
            return _mapper.Map<UserResp>(user);
        }

        public async Task<bool> RemoveAsync(UserReq user)
        {
            var userModel = await _uow.Users.FindAsync(user.Id);
            if (!string.IsNullOrWhiteSpace(user.LoginPassword))
            {
                if (VerifyHashString(userModel.Passw,user.LoginPassword))
                {
                    _uow.Users.Remove(userModel);
                    _uow.Commit();
                    return true;
                }
                return false;
            }
            else return false;
        }

        public async Task<UserResp> UpdateAsync(UserReq user)
        {
            var userModel = await _uow.Users.FindAsync(user.Id);

            if (!string.IsNullOrWhiteSpace(user.Name)
                && user.Name != userModel.Name)
            {
                userModel.Name = user.Name;
            }

            if (!string.IsNullOrWhiteSpace(user.NotHashedPassword)
                && !string.IsNullOrWhiteSpace(user.LoginPassword)
                && CreateHashString(user.LoginPassword) != userModel.Passw)
            {
                userModel.Passw = CreateHashString(user.NotHashedPassword);
            }

            _uow.Users.Update(userModel);
            _uow.Commit();

            return _mapper.Map<UserResp>(userModel);
        }
    }
}
