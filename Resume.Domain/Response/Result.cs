using System.Collections.Generic;
using System.Linq;

namespace Resume.Domain.Response
{
    public class Result
    {
        public static string GeneralErrorMessage => "An unexpected error occurred";
        public bool Succeeded { get; set; }
        public List<string> Messages { get; set; } = new List<string>();

        public void SetError(List<string> errorMessages = null)
        {
            if (!errorMessages?.Any() ?? true)
            {
                errorMessages = GeneralErrorMessage.ToListOfSelf();
            }
            Messages = errorMessages;
            Succeeded = false;
        }

        public void SetSuccess(List<string> noticeMessages = null)
        {
            Messages = noticeMessages ?? new List<string>();
            Succeeded = true;
        }


        public static Result Error(List<string> errorMessages = null)
        {
            var result = new Result();
            result.SetError(errorMessages);
            return result;
        }

        public static Result Ok(List<string> noticeMessages = null)
        {
            var result = new Result();
            result.SetSuccess(noticeMessages);
            return result;
        }
    }

    public class Result<T> : Result
    {
        public T ReturnedObject { get; set; }
        public void SetSuccessObject(T objectToReturn, List<string> notices)
        {
            ReturnedObject = objectToReturn;
            SetSuccess(notices);
        }

    }
}
