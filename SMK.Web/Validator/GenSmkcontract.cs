using FluentValidation;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Validator
{
    public class GenSmkcontractValidator : AbstractValidator<GenSmkcontract>
    {
        public GenSmkcontractValidator()
        {
            RuleFor(x => x.SmkcontractType).NotNull().NotEmpty();
            RuleFor(x => x.SmkcontractName).NotNull().NotEmpty();
        }
    }
}
