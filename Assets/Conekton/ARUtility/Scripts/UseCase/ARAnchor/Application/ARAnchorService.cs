using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Zenject;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;

namespace Conekton.ARUtility.UseCase.ARAnchor.Application
{
    public class ARAnchorService : IARAnchorService
    {
        [Inject] private IARAnchorSystem _system = null;

        IARAnchor IARAnchorService.Create() => _system.Create();

        IARAnchor IARAnchorService.Find(AnchorID anchorID) => _system.Find(anchorID);
    }
}

