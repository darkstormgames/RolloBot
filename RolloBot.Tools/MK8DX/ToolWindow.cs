using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RolloBot.Client.Communication;

namespace RolloBot.Tools.MK8DX
{
    public class ToolWindow : ToolWindowBase
    {
        public ToolWindow(ICommunicationsOwner communications) : base(communications)
        {
            this.LoadTools(new List<ToolBase>
            {
                new AutoToad.AutoToad(this),
                //new AutoTable(this),

            });
        }
    }
}
