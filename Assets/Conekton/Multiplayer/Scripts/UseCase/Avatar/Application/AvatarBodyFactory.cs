using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.AvatarBody.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARMultiplayer.AvatarBody.Application
{
    public class AvatarBodyFactory : MonoBehaviour, IAvatarBodyFactory<AvatarBodyTypeArgs>
    {
        [Inject] private DiContainer _container = null;

        [SerializeField] private GameObject _bodyPrefabA = null;
        [SerializeField] private GameObject _bodyPrefabB = null;
        
        public IAvatarBody Create(AvatarBodyTypeArgs args)
        {
            DiContainer subContainer = _container.CreateSubContainer();
            subContainer.BindInstance(args);
            
            GameObject obj = null;
            
            switch (args.BodyType)
            {
                case AvatarBodyType.A:
                    obj = subContainer.InstantiatePrefab(_bodyPrefabA);
                    break;
                
                case AvatarBodyType.B:
                    obj = subContainer.InstantiatePrefab(_bodyPrefabB);
                    break;
            }

            return obj?.GetComponent<IAvatarBody>();
        }
    }
}