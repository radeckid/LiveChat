using LiveChatRegisterLogin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LiveChatRegisterLogin.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        

        public UserRepository(DataContext context)
        {
            _context = context;

            context.Database.EnsureCreated();
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(true);
            var passwordUser = user.Password;

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, passwordUser.PasswordHash, passwordUser.PasswordSalt))
                return null;

            return user;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHashSalt(password, out passwordHash, out passwordSalt);

            user.Password = new Password
            {
                User = user,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync().ConfigureAwait(true);

            return user;
        }

        public async Task<IEnumerable<User>> GetAllFriend(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId).ConfigureAwait(true);

            if(user == null)
            {
                return null;
            }
            
            var friendId =  user.Friends.Select(x => x.FriendId);
            var friends = new List<User>();

            foreach (int id in friendId)
            {
                friends.Add(await _context.Users.FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(true));
            }

            return friends;
        }

        public async Task<bool> UserExists(int userId)
        {
            if (await _context.Users.AnyAsync(x => x.Id == userId).ConfigureAwait(true))
                return true;

            return false;
        }

        public async Task<bool> IsRelation(int requesterId, int newFriendId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == requesterId);

            return user.Friends.Any(x => x.FriendId == newFriendId);
        }

        private void CreatePasswordHashSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
                return true;
            }
        }

        private async Task<IEnumerable<User>> ProcessRelations(int[] friendId)
        {
            
        }
    }

}
