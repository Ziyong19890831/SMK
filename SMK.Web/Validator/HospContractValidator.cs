using FluentValidation;
using SMK.Web.Models;

namespace SMK.Web.Validator
{
    public class HospContractValidator : AbstractValidator<HospContractViewModel>
	{
		public HospContractValidator()
		{
	 		RuleFor(x => x.HospId).NotEmpty().WithMessage("機構代碼不可為空白。");
	 		RuleFor(x => x.HospSeqNo).NotEmpty().WithMessage("機構子代碼不可為空白。");
			RuleFor(x => x.SmkcontractType).NotEmpty().WithMessage("合約類型不可為空白。");
        }
    }
}
