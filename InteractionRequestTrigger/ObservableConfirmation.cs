using Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace InteractionRequestTrigger
{
    public class ObservableConfirmation : Confirmation, INotifyPropertyChanged, IDataErrorInfo
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="args">The PropertyChangedEventArgs</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }

        [Obsolete]
        public string Error
        {
            get
            {
                throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");
            }
        }

        /// <summary>
        /// Collection of all error
        /// </summary>
        protected readonly Dictionary<string, IEnumerable<string>> _Errors = new Dictionary<string, IEnumerable<string>>();

        /// <summary>
        /// This method is called after the property looses focus.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string this[string propertyName]
        {
            get
            {
                return OnValidate(propertyName);
            }
        }

        /// <summary>
        /// Validates current instance properties using Data Annotations.
        /// </summary>
        /// <param name="propertyName">This instance property to validate.</param>
        /// <returns>Relevant error string on validation failure or <see cref="System.String.Empty"/> on validation success.</returns>
        protected virtual string OnValidate(string propertyName)
        {
            var value = GetValue(propertyName);

            var validationContext = new ValidationContext(this, null, null)
            {
                MemberName = propertyName
            };

            if (_Errors.ContainsKey(propertyName))
            {
                _Errors.Remove(propertyName);
            }

            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateProperty(value, validationContext, results);

            if (!isValid)
            {
                var validationResult = results.First();

                _Errors[propertyName] = results.Select(x => x.ErrorMessage).ToList();

                return validationResult.ErrorMessage;
            }

            RaisePropertyChanged(nameof(IsValidModel));

            return string.Empty;
        }

        /// <summary>
        /// Gets the value of the given object
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private object GetValue(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            var propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);
            if (propertyDescriptor == null)
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            return propertyDescriptor.GetValue(this);
        }

        /// <summary>
        /// Checks the model state if it is valid or not
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValidModel
        {
            get
            {
                return !(_Errors.Any());
            }
        }

    }
}
