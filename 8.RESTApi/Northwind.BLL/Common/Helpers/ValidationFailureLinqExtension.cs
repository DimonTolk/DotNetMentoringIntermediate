using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL.Common.Helpers
{
    public static class ValidationFailureLinqExtension
    {
        public static IDictionary<string, string[]> ToPropGroupedValidationError(this IEnumerable<ValidationFailure> failures)
        {
            var errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

            return errors;
        }
    }
}
