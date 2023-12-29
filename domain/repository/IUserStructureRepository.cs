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
    /// 用户组织实体仓库
    /// </summary>
    public interface IUserStructureRepository
    {
        /// <summary>
        /// 查询指定用户的>用户组织简要信息
        /// </summary>     
        /// <param name="userName">用户名称</param>
        /// <returns>用户组织简要信息集合</returns>
        List<UserStructureLite> selectByUser(string userName);
    }
}
