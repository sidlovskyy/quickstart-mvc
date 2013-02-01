using System.Web.Mvc;

namespace QuickStartProject.Web.UI.Filters
{
    public class ModelStateMergeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            object tempDataModelState = filterContext.Controller.TempData[Constants.MODEL_STATE];
            ModelStateDictionary controllerModelState = filterContext.Controller.ViewData.ModelState;

            if ((null != tempDataModelState) && (null != controllerModelState)
                && !controllerModelState.Equals(tempDataModelState))
            {
                controllerModelState.Merge((ModelStateDictionary) tempDataModelState);
            }
        }
    }
}