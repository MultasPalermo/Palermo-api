using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.NameSplitter
{
    public static class NameSplitter
    {
        public static (string firstName, string lastName) Split(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return ("", "");
            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 1) return (parts[0], "");
            var lastName = parts[^1];
            var firstName = string.Join(" ", parts[..^1]);
            return (firstName, lastName);
        }
    }

}
