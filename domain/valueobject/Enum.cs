using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.domain.valueobject
{
    public enum CreateTestTaskFrom
    {
        MAIN_FORM = 0,

        TEST_STATISTIC_LIST_FORM = 1,
    }

    public enum Role
    {
        超级管理员 = 0,

        管理员 = 1,

        普通成员 = 2,
    }

    public enum OperationType
    {
        ADD = 0,

        EDIT = 1
    }

    public enum EquipmentStateChn {
        使用中 = 1,
        待检定 = 2,
        未启用 = 4,
        停用 = 8,
        报废 = 16,
    }

   
}
