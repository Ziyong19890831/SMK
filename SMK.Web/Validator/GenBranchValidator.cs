using FluentValidation;
using SMK.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMK.Web.Validator
{
    public class GenBranchValidator: AbstractValidator<GenBranch>
	{
		public GenBranchValidator()
		{
	 		RuleFor(x => x.BranchNo).NotNull().NotEmpty();
	 		RuleFor(x => x.BranchName).NotNull().NotEmpty();
        }
    }
}
