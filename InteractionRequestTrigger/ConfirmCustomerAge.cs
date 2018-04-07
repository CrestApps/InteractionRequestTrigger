using System;
using System.ComponentModel.DataAnnotations;

namespace InteractionRequestTrigger
{
    public class ConfirmCustomerAge : ObservableConfirmation
    {
        public DateTime? _BirthDate { get; set; }
        [Required]
        public DateTime? BirthDate
        {
            get
            {
                return _BirthDate;
            }
            set
            {
                _BirthDate = value;
                RaisePropertyChanged();
            }
        }
    }
}
