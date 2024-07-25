using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.DAL;

namespace WindowsFormsApp1.BLL
{
    internal class ControlUserBLL
    {
        ControlUserDAL userDAL = new ControlUserDAL();

        public List<string> GetUserPermissions(int userID)
        {
            return userDAL.GetUserPermissions(userID);
        }

        public void SetUserPermissions(int userID, List<string> permissions)
        {
            userDAL.SetUserPermissions(userID, permissions);
        }
    }
}
