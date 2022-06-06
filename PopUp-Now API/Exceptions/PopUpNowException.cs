using System;

namespace PopUp_Now_API.Exceptions
{
    public class PopUpNowException : Exception
    {
        public PopUpNowException(string message) : base(message)
        {
        }
    }
}