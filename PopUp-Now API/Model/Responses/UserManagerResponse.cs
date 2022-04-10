using System.Collections.Generic;

namespace PopUp_Now_API.Model.Responses
{
    public class UserManagerResponse
    {
        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}