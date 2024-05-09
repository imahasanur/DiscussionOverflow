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
    public interface IUserManagementService
    {
        Task<JsonElement> GetUserByEmailAsync(string? email);
        Task<JsonElement> GetProfileUserByEmailAsync(string? email);
        Task<IdentityResult> UpdateUserAsync(JsonElement user);
    }
}
