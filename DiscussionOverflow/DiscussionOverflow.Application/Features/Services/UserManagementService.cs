
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DiscussionOverflow.Application.Features.Services
{
    public class UserManagementService:IUserManagementService
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public UserManagementService( IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<JsonElement> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
        }

        public async Task<JsonElement> GetProfileUserByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetProfileUserByEmailAsync(email);
        }

        public async Task<IdentityResult> UpdateUserAsync(JsonElement user)
        {
            return await _unitOfWork.UserRepository.UpdateUserAsync(user);
        }


    }
}
