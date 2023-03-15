using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication
{
    internal class qq
    {
        public qq ()
        {
            // TODO: Get Roles, Set Claims

            //var appUser = _financeAppDbContext.ApplicationUsers
            //    .Include(x => x.ApplicationUserRoles)
            //    .Where(x => x.AuthenticationUserId == authenticationUser.Id)
            //    .FirstOrDefault();

            //if (appUser == null) {
            //    // TODO: Fix Response to avoid givving information to a user;
            //    var errorMessage = new ErrorResponseMessage(
            //            new LogginInActionMessage(loginRequest.Username),
            //            new ResourceWithPropertyMessage(authenticationUser, "Username", loginRequest.Username),
            //            new UnauthorizedToAccessResourceReason());
            //    return new ApiResponse<AuthenticationUserDto>(ApiResponseCodesEnum.Unauthorized, errorMessage);
            //}


            //var claimsIdentity = new ClaimsIdentity(appUser.GetClaims(authenticationUser), CookieAuthenticationDefaults.AuthenticationScheme);

            //await _microsfotAuthService.SignInAsync(httpContext, null, new ClaimsPrincipal(claimsIdentity), null);

            //// Create Cooke to tell SPA that it is logged in
            //_cookieManager.Set("LoggedIn", true.ToString(), new CookieOptions {
            //    Secure = true,
            //    HttpOnly = false,
            //    SameSite = SameSiteMode.None
            //});

            //var ret = new ApiResponse<AuthenticationUserDto>(_mapper.Map<AuthenticationUserDto>(authenticationUser));
            //return ret;
        }
    }
}
