using System.Text.RegularExpressions;

namespace Application.Common.Requests;

public abstract class BotCallbackRequest
{
    public abstract string CallbackPattern { get; }
    
    public virtual bool Contains(string callbackData) =>
        new Regex(CallbackPattern).Match(callbackData).Success;
}