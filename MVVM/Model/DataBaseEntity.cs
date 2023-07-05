using Filian.Core;

namespace Filian.MVVM.Model
{
    public abstract class DataBaseEntity : ObservableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Translation { get; set; }
        public string Picture_Path { get; set; }
    }
}
