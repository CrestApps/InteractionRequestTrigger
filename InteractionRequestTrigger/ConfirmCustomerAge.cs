using System;
using System.ComponentModel.DataAnnotations;

namespace InteractionRequestTrigger
{
    public class ConfirmCustomerAge : ObservableConfirmation
    {
        private DateTime? _birthDate;

        [Required]
        public DateTime? BirthDate
        {
            get
            {
                return _birthDate;
            }
            set
            {
                _birthDate = value;
                RaisePropertyChanged();
            }
        }
    }
}
