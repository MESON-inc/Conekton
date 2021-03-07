using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.AvatarBody.Application
{
    public class AvatarBodyFactory : MonoBehaviour, IAvatarBodyFactory
    {
        [Inject] private DiContainer _container = null;

        [SerializeField] private GameObject _bodyPrefabA = null;
        [SerializeField] private GameObject _bodyPrefabB = null;
        
        public IAvatarBody Create(CreateAvatarBodyArgs args)
        {
            DiContainer subContainer = _container.CreateSubContainer();
            subContainer.BindInstance(args);
            
            GameObject obj = null;
            
            switch (args.BodyType)
            {
                case (byte)'A':
                    obj = subContainer.InstantiatePrefab(_bodyPrefabA);
                    break;
                
                case (byte)'B':
                    obj = subContainer.InstantiatePrefab(_bodyPrefabB);
                    break;
                
                default:
                    Debug.LogError($"The passed body type is not found. Received body type is {args.BodyType}");
                    break;
            }

            return obj?.GetComponent<IAvatarBody>();
        }
    }
}