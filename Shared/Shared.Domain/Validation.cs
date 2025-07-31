using System.ComponentModel.DataAnnotations;

namespace Shared.Domain;

public static class Validation
{
    public static void BasedOn(Action<IList<string>> validator)
    {
        List<string> source = [];
        validator(source);
        if (source.Count != 0)
        {
            throw new ValidationException(string.Join<string>(", ", source));
        }
    }
}