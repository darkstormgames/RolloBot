using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RolloBot.Client.Communication;

namespace RolloBot.Client.Tools.MK8DX
{
    public class ToolPanel : ToolWindowBase
    {
        public ToolPanel(ICommunicationsOwner communications) : base(communications)
        {
            this.LoadTools(new List<ToolBase>
            {
                new AutoToad(this),
                //new AutoTable(this),

            });
        }
    }
}
