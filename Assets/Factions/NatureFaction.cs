using Assets.Structures.Cards;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assets.Factions
{
    public class NatureFaction : FactionBase
    {
        public NatureFaction(string name, ServiceLocator.IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
        }

        public override void TakeTurn()
        {
            Task.Run(() => DoStuff());
        }

        public void DoStuff()
        {
            EndTurn();
        }
    }
}