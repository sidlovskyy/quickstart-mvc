using System.Web.Mvc;
using Logfox.Domain.Entities;
using Logfox.Domain.Repository;

namespace Logfox.Web.UI.Controllers
{
	public class ImageController : BaseController
    {       
		private readonly IRepository<Image, int> _imageRepository;

		public ImageController(IRepository<Image, int> imageRepository) 
		{
			_imageRepository = imageRepository;
		}

		[HttpGet]
		public ActionResult Get(int id) {
			Image image = _imageRepository.GetById(id);
			return (image != null)
				? File(image.Content, image.ContentType)
				: (ActionResult) HttpNotFound();
		}
    }
}
