using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.DTO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class BaseController<U> : Controller where U : BaseEntity
    {
        protected async Task<IActionResult> GetSingleToken<T>(Func<Task<T>> getFunc)
        {           
            var obj = await getFunc.Invoke();

            return obj != null ? (IActionResult)Ok(obj) : NotFound();
        }
    }
}
