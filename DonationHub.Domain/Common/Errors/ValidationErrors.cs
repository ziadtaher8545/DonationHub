using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Common.Errors;

public static class ValidationErrors
{
    public static readonly Error Failed = new("Validation.Failed", "One or more validation errors occurred.", ErrorType.Validation);

}
