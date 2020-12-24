using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Repository
{
    public enum JobStatus
    {
        [Description("在职")]
        InJob = 0,
        [Description("离职")]
        OutJob =1,
        [Description("审核中")]
        Audit =2,
        [Description("拒绝")]
        Refuse = 3
    }
}
