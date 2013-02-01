using System.IO;
using System.Web;
using Logfox.Domain.Entities;

namespace Logfox.Web.UI.Extensions
{
    public static class HttpPostedFileBaseExtensions
    {
        public static byte[] ToByteArray(this HttpPostedFileBase postFile)
        {
            byte[] photoContent = null;

            if (null != postFile)
            {
                using (Stream inputStream = postFile.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    photoContent = memoryStream.ToArray();
                }
            }

            if (null != photoContent && 0 == photoContent.Length)
            {
                photoContent = null;
            }

            return photoContent;
        }

		public static Image ToImage(this HttpPostedFileBase postFile)
		{
			Image image = null;
			byte[] imageContent = postFile.ToByteArray();
			if (imageContent != null)
			{
				image = new Image();
				image.Content = imageContent;
				image.ContentType = postFile.ContentType;
			}
			return image;
		}
    }
}