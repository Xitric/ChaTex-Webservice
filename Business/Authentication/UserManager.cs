using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Authentication
{
    class UserManager : IUserManager
    {
        private readonly IDataAccess dal;

        public UserManager(IDataAccess dal)
        {
            this.dal = dal;
        }

        public string Login(string email)
        {
            string token = dal.GetSessionToken(email);

            if (token == null)
            {
                //From:
                //https://stackoverflow.com/questions/14643735/how-to-generate-a-unique-token-which-expires-after-24-hours
                token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                //Token expires after 24 hours
                DateTime expiration = DateTime.Now.AddDays(1);

                if (dal.SaveUserToken(email, token, expiration))
                {
                    return token;
                }

                return null;
            }

            return token;
        }
    }
}
