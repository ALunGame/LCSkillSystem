using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    public static class SkillLocate
    {
        private static SkillServer skillServer = new SkillServer();

        public static SkillServer SkillServer { get => skillServer;}
    }
}
