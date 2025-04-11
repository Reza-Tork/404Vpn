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
        MainMenu,
        //End Message Commands
        // Callback Commands
        MyServiceDetails,
        BuyBandwidth,
        RenewMyService,
        ChargeWallet,
        PaymentMethod,
        CardToCard,
        Factor
        // End Callback Commands
    }
}
