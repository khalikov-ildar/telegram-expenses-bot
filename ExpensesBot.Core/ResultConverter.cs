using System.Text;
using ErrorOr;

namespace ExpensesBot.Core;

public static class ResultConverter
{
    public static string Problem(List<Error> errors)
    {
        if (errors.Count == 1)
        {
            return ResultConverter.Problem(errors[0]);
        }
        var toReturn = new StringBuilder();
        for (var i = 0; i < errors.Count; i++)
        {
            var errorMessage = $"{i + 1}) {errors[i].Description}";
            toReturn.Append(errorMessage);
        }

        return toReturn.ToString();
    }

    public static string Problem(Error error)
    {
        return error.Description;
    }
}