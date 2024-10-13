using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication1.Validators
{
    public class CustomActionFilters
    {
        public class ValidateModelAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {
                // Kiểm tra tính hợp lệ của ModelState
                if (!context.ModelState.IsValid)
                {
                    // Nếu không hợp lệ, trả về BadRequest
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
            }
        }
    }
}
