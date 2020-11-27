using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RolloBot.Client.Communication;

namespace RolloBot.Tools.MK8DX
{
    internal class AutoTable : ToolBase
    {
        public AutoTable(ToolWindowBase mainWindow) : base(mainWindow)
        {
        }

        public override string Name => "AutoTable";
        public override string DisplayName => "Auto Table";
        public override string ToolTip => "";
        public override string RunButtonText => "Create Table";
        public override string StopButtonText => "Cancel Creation";
        public override bool NeedsVideo => true;
        public override bool NeedsSerial => false;
        public override bool NeedsXInput => false;
        public override CaptureExecutionSpeed CaptureExecutionSpeed => CaptureExecutionSpeed.Once;
        public override ExecutionTrigger ExecutionTrigger => ExecutionTrigger.None;
        public override ToolConfigBase Config => null;

        public override void Execute()
        {
            throw new NotImplementedException();
        }

        protected override void Cleanup()
        {
            throw new NotImplementedException();
        }
    }
}
