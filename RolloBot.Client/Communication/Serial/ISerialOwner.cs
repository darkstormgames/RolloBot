using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RolloBot.Client.Communication.Serial
{
    public interface ISerialOwner
    {
        SerialCommunication SerialCommunication { get; }
        bool IsSerialEnabled { get; set; }
    }
}
