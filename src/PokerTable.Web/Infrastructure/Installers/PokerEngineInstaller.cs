using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using PokerTable.Game;
using PokerTable.Game.Data;
using PokerTable.Game.Data.Interfaces;

namespace PokerTable.Web.Infrastructure.Installers
{
    public class PokerEngineInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IEngine>().ImplementedBy<Engine>().LifestyleTransient(),
                Component.For<IDealer>().ImplementedBy<Dealer>().LifestyleTransient(),
                Component.For<IDeckBuilder>().ImplementedBy<DeckBuilder>().LifestyleTransient(),
                Component.For<ISeatManager>().ImplementedBy<SeatManager>().LifestyleTransient(),
                Component.For<IRepository>().ImplementedBy<AzureRepository>().LifestyleTransient()
                );
        }
    }
}