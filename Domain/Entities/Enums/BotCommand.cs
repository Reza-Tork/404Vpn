using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum BotCommand
    {
        // Message Commands
        None,
        Start,
        BuyService,
        RenewService,
        MyServices,
        ExtraBandwidth,
        Plans,
        Wallet,
        Support,
        Help,
        AdminPanel,
        //End Message Commands


        // Callback Commands

        // End Callback Commands

        // Callback Admin Commands
        AdminGetServices,
        AdminAddService,
        AdminRemoveService
        // End Callback Admin Commands
    }
}
