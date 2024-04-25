using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.valueobject;

namespace TaskManager.domain.service
{
    public class RoleManager
    {
        public static bool isAdmin(string userRole) {
            return userRole.Equals(Role.超级管理员.ToString())
                || userRole.Equals(Role.管理员.ToString());
        }
    }
}
