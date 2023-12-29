using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.domain.entity;
using TaskManager.domain.valueobject;

namespace TaskManager.domain.repository
{
    /// <summary>
    /// 用户信息实体仓库
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// 查询所有用户信息
        /// </summary>     
        /// <returns>用户信息集合</returns>
        List<UserEntity> selectAll();
    }
}
