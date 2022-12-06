using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NOV.ES.TAT.Common.FeatureToggle.Service;
using NOV.ES.TAT.Common.UserPermissions;
using NOV.ES.TAT.Common.UserPermissions.Models;
using NOV.ES.TAT.Common.UserPermissions.Service;

namespace NOV.ES.TAT.Job.API.Filters
{
    public class GlobalAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IUserProfileService UserProfileService;
        private readonly IFeatureToggleService FeatureToggleService;

        public GlobalAuthorizationFilter(IUserProfileService userProfileService, IFeatureToggleService featureToggleService)
        {
            this.UserProfileService = userProfileService;
            this.FeatureToggleService = featureToggleService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            #region FeatureToggle
            //Task<ToggleFeatures> toggleFeatures = FeatureToggleService.GetUserToggleFeatures();
            //if (toggleFeatures.Result != null)
            //    context.HttpContext.Items["ToggleFeatures"] = toggleFeatures.Result;
            #endregion
            if (context.HttpContext.Request.Method.Equals(HttpMethod.Post.Method))
                return;

            #region Authorization
            var viewRequest = context.HttpContext.Request.Method.Equals(HttpMethod.Get.Method);
            if (viewRequest)
                return;

            Task<UserProfile> userProfile = UserProfileService.GetUserProfile();

            if (userProfile.Result != null && userProfile.Result.UserSystemFeaturePermissions != null)
            {
                if (userProfile.Result.IsSuperAdmin)
                    return;

                bool addRequest = context.HttpContext.Request.Method.Equals(HttpMethod.Post.Method);
                if (addRequest && userProfile.Result.UserSystemFeaturePermissions
                        .Where(x => x.Code.Equals(Constants.MAINT_PKG_SLIP)
                        && x.PermissionName.Equals(Constants.ADD)).ToList().Count > 0)
                {
                    return;
                }

                bool editRequest = context.HttpContext.Request.Method.Equals(HttpMethod.Put.Method);
                if (editRequest && userProfile.Result.UserSystemFeaturePermissions
                        .Where(x => x.Code.Equals(Constants.MAINT_PKG_SLIP)
                        && x.PermissionName.Equals(Constants.EDIT)).ToList().Count > 0)
                {
                    return;
                }

                bool deleteRequest = context.HttpContext.Request.Method.Equals(HttpMethod.Delete.Method);
                if (deleteRequest && userProfile.Result.UserSystemFeaturePermissions
                        .Where(x => x.Code.Equals(Constants.MAINT_PKG_SLIP)
                        && x.PermissionName.Equals(Constants.DELETE)).ToList().Count > 0)
                {
                    return;
                }
            }

            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            #endregion
        }
    }
}
