using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NOV.ES.TAT.Common.FeatureToggle;
using NOV.ES.TAT.Common.FeatureToggle.Models;

namespace NOV.ES.TAT.Job.API.Filters
{
    [AttributeUsage(AttributeTargets.All)]
    public class FeatureToggleAttribute : Attribute,IAuthorizationFilter
    {
        public string Feature { get; }
        public FeatureToggleAttribute(string feature)
        {
            Feature = feature;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ToggleFeatures toggleFeatures = (ToggleFeatures)context.HttpContext.Items["ToggleFeatures"];
            if (toggleFeatures == null || toggleFeatures.toggleFeatures_ == null
                || toggleFeatures.toggleFeatures_.FirstOrDefault(x => x.FeatureCode.Equals(Feature)) == null)
            {
                context.Result = new JsonResult(new { message = string.Format(Constants.Features_Toggle_Unauthorized_Error, Feature) })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }      
    }
}
