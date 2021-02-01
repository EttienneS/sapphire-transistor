using Assets.Cards;
using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.StrategyCamera;
using Assets.Structures;
using Assets.UI;
using UnityEngine;

namespace Assets
{
    /// <summary>
    ///   <para>Required controller monobehavior for the Service Locator.  This class initializes the ServiceLocator and keeps it alive.</para>
    ///   <para>Anything that manages the ServiceLocator lifecycle should live here.</para>
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        private IServiceLocator _locator;

        /// <summary>
        /// Creates the ServiceLocator and registers MonoBehaviors that already exist in the scene.
        /// Does an optional call to ProcessInitializationQueue to resovle the references in the loded monobehaviors.
        /// </summary>
        public void Awake()
        {
            _locator = Locator.Create();

            _locator.Register<ISpawnManager>(FindObjectOfType<SpawnManager>());
            _locator.Register<ICameraController>(FindObjectOfType<CameraController>());
            _locator.Register<IMapManager>(FindObjectOfType<MapManager>());

            _locator.Register<IStructureFactory>(new StructureFactory());
            _locator.Register<IFactionManager>(new FactionManager());

            _locator.Register<ICardManager>(new CardManager());

            _locator.Register<NewGameManager>(new NewGameManager());
            
            _locator.Register<IUIManager>(FindObjectOfType<UIManager>());
        }

        private bool _start = true;

        public void Update()
        {
            if (_start)
            {
                StartCoroutine(_locator.ProcessServiceList());
                _start = false;
            }
        }
    }
}