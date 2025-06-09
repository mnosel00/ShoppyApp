using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Main.Common
{
    public class BasketState : INotifyPropertyChanged
    {
        private static BasketState? _instance;
        public static BasketState Instance => _instance ??= new BasketState();

        private Guid? _basketId;
        public Guid? BasketId
        {
            get => _basketId;
            set
            {
                if (_basketId != value)
                {
                    _basketId = value;
                    OnPropertyChanged();
                }
            }
        }

        public Guid UserId { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
