using Assets.Factions;
using Assets.Map;
using Assets.Resources;
using Assets.StrategyCamera;
using Assets.Structures;
using UnityEngine;

namespace Assets.ServiceLocator
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
            var locator = new Locator();

            locator.Register(FindObjectOfType<CameraController>());
            locator.Register(FindObjectOfType<MapManager>());
            locator.Register(new ResourceManager());
            locator.Register(new FactionManager());
            locator.Register(new StructureFactory());
            locator.Register(FindObjectOfType<MapGenerator>());

            locator.ProcessInitializationQueue();
            locator.LogServices();

            _serviceLocator = locator;
        }

        /// <summary>Gets the service locator.</summary>
        /// <returns>
        ///   The current ServiceLocator
        /// </returns>
        public Locator GetServiceLocator()
        {
            return _serviceLocator;
        }
    }
}