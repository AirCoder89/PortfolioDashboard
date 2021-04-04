using System.Collections.Generic;

namespace Models
{
    public abstract class RootModel
    {
        public abstract List<T> GetModels<T>() where T : Model;
    }
}