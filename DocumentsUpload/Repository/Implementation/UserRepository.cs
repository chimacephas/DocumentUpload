using DocumentsUpload.Data;
using DocumentsUpload.Entities;
using DocumentsUpload.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Repository.Implementation
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<User> GetUserWithDocuments(string emailorTransactionId)
        {
            return await _context.Users.Where(x => x.Email == emailorTransactionId || x.TransactionNumber == emailorTransactionId)
                                 .Include(x => x.Documents)
                                 .SingleOrDefaultAsync();
        }
    }
}
