using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.StrategyCamera;
using Assets.Structures;
using UnityEngine;

namespace Assets
{
    /// <summary>
    ///   <para>Required controller monobehavior for the Service Locator.  This class initializes the ServiceLocator and keeps it alive.</para>
    ///   <para>Anything that manages the ServiceLocator lifecycle should live here.</para>
    /// </summary>
    public class Bootstrapper : MonoBehaviour
    {
        private Locator _serviceLocator;

        /// <summary>
        /// Creates the ServiceLocator and registers MonoBehaviors that already exist in the scene.
        /// Does an optional call to ProcessInitializationQueue to resovle the references in the loded monobehaviors.
        /// </summary>
        public void Awake()
        {
            Locator.Instaniate();

            var locator = Locator.Instance;

            locator.Register<ISpawnManager>(FindObjectOfType<SpawnManager>());
            locator.Register<ICameraController>(FindObjectOfType<CameraController>());
            locator.Register<IMapManager>(FindObjectOfType<MapManager>());

            locator.Register<IStructureFactory>(new StructureFactory());
            locator.Register<IFactionManager>(new FactionManager());
            locator.Register<NewGameManager>(new NewGameManager());

            locator.ProcessInitializationQueue();
            locator.LogServices();
        }
    }
}