﻿using Assets.Structures;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Factions
{
    public class AIFaction : FactionBase
    {
        public AIFaction(string name, ServiceLocator.IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
        }

        public override void TakeTurn()
        {
            Task.Run(() => DoStuff());
        }

        public void DoStuff()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(100);
                Debug.Log($"Working {i}...");
            }
            Debug.Log($"Turn over: {Name}");

            EndTurn();
        }
    }
}