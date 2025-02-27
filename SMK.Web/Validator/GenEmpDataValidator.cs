using FluentValidation;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Validator
{
    public class GenEmpDataValidator : AbstractValidator<GenEmpData>
	{
		public GenEmpDataValidator()
		{
			RuleFor(x => x.Account).NotNull().NotEmpty();
			RuleFor(x => x.Name).NotNull().NotEmpty();
		}
		public GenEmpDataValidator(bool pwd)
		{
	 		RuleFor(x => x.Account).NotNull().NotEmpty();
	 		RuleFor(x => x.Name).NotNull().NotEmpty();
			if (pwd) {
				RuleFor(x => x.Pwd).NotNull().NotEmpty();
			}
        }
    }
}
