using System.Collections;
using System.Collections.Generic;
using Conekton.ARUtility.UseCase.WorldOrigin.Domain;
using Conekton.Utility;

namespace Conekton.ARUtility.UseCase.WorldOrigin.Infrastructure
{
    public class WorldOriginService : IWorldOriginService
    {
        private IWorldOrigin _origin = null;

        public void UnRegister(IWorldOrigin origin)
        {
            if (_origin == origin)
            {
                _origin = null;
            }
        }

        public void Register(IWorldOrigin origin)
        {
            _origin = origin;
        }

        public IWorldOrigin GetWorldOrigin()
        {
            return _origin;
        }
    }
}