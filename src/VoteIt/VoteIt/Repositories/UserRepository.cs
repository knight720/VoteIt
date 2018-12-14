using System.Collections.Generic;
using System.Linq;
using VoteIt.Data;

namespace VoteIt.Repositories
{
    public class UserRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        /// <summary>
        /// 以 email 取得使用者名稱
        /// </summary>
        /// <param name="emailList"></param>
        /// <returns></returns>
        public List<string> GetUserName(List<string> emailList)
        {
            var result = emailList.GroupJoin
                (
                    this._applicationDbContext.Users,
                    e => e,
                    u => u.Email,
                    (e, u) => new { e, u }
                )
                .SelectMany
                (
                    eu => eu.u.DefaultIfEmpty(),
                    (eu, u) => u == null ? eu.e : u.UserName
                )
                .ToList();

            return result;
        }
    }
}