using FluentValidation;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Validator
{
    public class GenEndReasonValidator : AbstractValidator<GenEndReason>
    {
        public GenEndReasonValidator()
        {
            RuleFor(x => x.EndReasonNo).NotNull().NotEmpty();
            RuleFor(x => x.EndReasonName).NotNull().NotEmpty();
        }
    }
}

