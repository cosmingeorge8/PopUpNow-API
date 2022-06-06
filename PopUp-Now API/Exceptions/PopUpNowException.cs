using System;

namespace PopUp_Now_API.Exceptions
{
    /**
     * Custom exception class used by PopUpNow application
     */
    public class PopUpNowException : Exception
    {
        public PopUpNowException(string message) : base(message)
        {
        }
    }
}