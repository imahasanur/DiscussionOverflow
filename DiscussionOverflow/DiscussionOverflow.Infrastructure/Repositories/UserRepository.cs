using DiscussionOverflow.Domain;
using DiscussionOverflow.Domain.Entities;
using DiscussionOverflow.Domain.Repositories;
using DiscussionOverflow.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DiscussionOverflow.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(IApplicationDbContext context, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public  async Task<JsonElement> GetUserByEmailAsync(string email)
        {
           var user = await _userManager.FindByEmailAsync(email);
            // Convert the user object to JSON
            var userJson = JsonSerializer.Serialize(user);

            // Parse the JSON string into a JsonElement
            var jsonElement = JsonDocument.Parse(userJson).RootElement;

            return jsonElement;

        }

        public async Task<JsonElement> GetProfileUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            // Convert the user object to JSON
            var userJson = JsonSerializer.Serialize(user);

            // Parse the JSON string into a JsonElement
            var jsonElement = JsonDocument.Parse(userJson).RootElement;

            return jsonElement;

        }

        public async Task<IdentityResult> UpdateUserAsync(JsonElement user)
        {
            var deserializedUser = JsonSerializer.Deserialize<ApplicationUser>(user.GetRawText());
            var response = await _userManager.UpdateAsync(deserializedUser);
            return response;
        }


    }
}
