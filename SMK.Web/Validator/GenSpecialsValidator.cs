

using FluentValidation;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Validator
{
    public class GenSpecialsValidator : AbstractValidator<GenSpecial>
    {
        public GenSpecialsValidator()
        {
            RuleFor(x => x.SpecialistNo).NotNull().NotEmpty();
            RuleFor(x => x.SpecialistName).NotNull().NotEmpty();
        }
    }
}
