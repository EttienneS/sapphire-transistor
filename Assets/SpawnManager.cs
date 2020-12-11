using Assets.ServiceLocator;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets
{
    public class SpawnManager : LocatableMonoBehaviorBase
    {
        private const string StructureGroup = "Structures";

        public override void Initialize()
        {
            LoadStructures();
        }

        private Dictionary<string, GameObject> _structures;

        private bool _structuresLoaded;

        private void LoadStructures()
        {
            _structures = new Dictionary<string, GameObject>();

            var load = Addressables.LoadAssetsAsync<GameObject>(StructureGroup, op => _structures.Add(op.name, op));
            load.Completed += (_) => _structuresLoaded = true;
        }


        public GameObject SpawnStructure(string name)
        {
            while (!_structuresLoaded)
            {
                Thread.Sleep(1);
            }

            return Instantiate(_structures[name], transform);
        }
    }
}