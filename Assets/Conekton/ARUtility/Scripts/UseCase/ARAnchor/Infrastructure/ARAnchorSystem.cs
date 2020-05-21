using Zenject;

using Conekton.ARUtility.UseCase.ARAnchor.Domain;

namespace Conekton.ARUtility.UseCase.ARAnchor.Infrastructure
{
    public class ARAnchorSystem : IARAnchorSystem
    {
        [Inject] private IARAnchorRepository _repository = null;

        IARAnchor IARAnchorSystem.Find(AnchorID anchorID) => _repository.Find(anchorID);

        private int _index = 0;

        IARAnchor IARAnchorSystem.Create()
        {
            AnchorID id = CreateNewID();
            return _repository.Create(id);
        }

        private AnchorID CreateNewID()
        {
            return new AnchorID
            {
                ID = (_index++).GetHashCode(),
            };
        }
    }
}

