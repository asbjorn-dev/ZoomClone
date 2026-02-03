using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared
{
    // Base response wrapper for API operations with status, message, and data payload
    // I am using generics to eliminate duplication 

    // IsSuccess - Indicates whether the operation succeeded
    // Message - Optional message providing additional context (error details or success info)
    // Data - The actual data payload returned by the operation, or default if unsuccessful
    public abstract record ServiceResponse<T>(bool IsSuccess = false, string? Message = null, T? Data = default);
}
