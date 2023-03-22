// Photo Sender
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

public partial class UploadPhotos : System.Web.UI.Page
{
    private Dictionary<string, byte[]> photos = new Dictionary<string, byte[]>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            HttpPostedFile photo = Request.Files["photo"];
            if (photo != null && photo.ContentLength > 0 && IsImage(photo))
            {
                byte[] data = new byte[photo.ContentLength];
                photo.InputStream.Read(data, 0, photo.ContentLength);
                photos.Add(photo.FileName, data);
            }
        }
        else
        {
            // Return the dictionary of uploaded photos to the client
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(photos);
            Response.Clear();
            Response.ContentType = "application/json";
            Response.Write(json);
            Response.End();
        }
    }

    private bool IsImage(HttpPostedFile file)
    {
        return file.ContentType.StartsWith("image/");
    }
}