using System.Linq;
using System.Web.Mvc;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Web.UI.Filters;

namespace QuickStartProject.Web.UI.Controllers
{
    public class PricingController : BaseController
    {
        private readonly IRepository<Pricing, int> _pricingRepository;
        private readonly IRepository<StorageUnit, int> _storageUnitRepository;
        private readonly IRepository<TimeUnit, int> _timeUnitRepository;

        public PricingController(IRepository<Pricing, int> pricingRepo, IRepository<TimeUnit, int> timeUnitRepo,
                                 IRepository<StorageUnit, int> storageUnitRepo)
        {
            _pricingRepository = pricingRepo;
            _timeUnitRepository = timeUnitRepo;
            _storageUnitRepository = storageUnitRepo;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AjaxOnly]
        public JsonResult GetPricing()
        {
            var timeUnits = _timeUnitRepository.All();
            var storageUnits = _storageUnitRepository.All();
            var priceItems = _pricingRepository.All();

            var result = new
                             {
                                 timeItems = timeUnits.Select(tu => new {index = tu.Id, value = tu.Value}),
                                 storageItems = storageUnits.Select(su => new {index = su.Id, value = su.Value}),
                                 priceItems =
                                     priceItems.Select(
                                         pi =>
                                         new
                                             {
                                                 storageIndex = pi.StorageUnit.Id,
                                                 timeIndex = pi.TimeUnit.Id,
                                                 value = pi.Value
                                             })
                             };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}