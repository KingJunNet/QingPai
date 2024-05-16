using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;
using TaskManager.domain.repository;
using TaskManager.infrastructure.db;

namespace TaskManager.controller
{
    public sealed class UserHelper
    {
        private static readonly UserHelper instance = new UserHelper();
        /// <summary>
        /// 显式的静态构造函数⽤来告诉C#编译器在其内容实例化之前不要标记其类型
        /// </summary>
        static UserHelper() { }
        private UserHelper() {
            this.userRepository = new UserRepository();
        }
        public static UserHelper Instance { get { return instance; } }

        private bool isLoadUsers = false;
        private Dictionary<string, UserEntity> userMap;
        public Dictionary<string, UserEntity> UserMap { get { return userMap; } }

        private IUserRepository userRepository;

        public void loadUsers()
        {
            if (isLoadUsers)
            {
                return;
            }

            List<UserEntity> users = this.userRepository.selectAll();
            this.userMap = new Dictionary<string, UserEntity>();
            users.ForEach(user =>
            {
                if (!this.userMap.ContainsKey(user.UserName))
                {
                    this.userMap.Add(user.UserName, user);
                }
            });
            this.isLoadUsers = true;
        }

        public string calculateUserGroupName(string owner)
        {
            string groupName = "";

            if (string.IsNullOrWhiteSpace(owner))
            {
                return groupName;
            }
            //反推组别信息
            loadUsers();
            if (UserMap.ContainsKey(owner))
            {
                groupName = UserMap[owner].Department;
            }

            return groupName;
        }
    }
}
