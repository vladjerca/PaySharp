using System;

namespace PaySharp.Exceptions
{
    public class PaymentConfigurationException : Exception
    {
        public PaymentConfigurationException(string missingField) : 
            base(string.Format("Configuration value for {0} could not be found. Please add it to the application configuration.", missingField)) { }
    }
}
