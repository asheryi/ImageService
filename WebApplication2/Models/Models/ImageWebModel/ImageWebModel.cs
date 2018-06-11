using System.IO;

namespace WebApplication2.Models.Models.ImageWebModel
{
    public class ImageWebModel
    {
        public string OutputPath { get; set; }

        public ImageWebModel()
        {

        }

        private int ScanForPhotos(string sDir)
        {
            int count = 0;
            foreach (string f in Directory.GetFiles(sDir))
            {
                    ++count;
            }
            foreach (string d in Directory.GetDirectories(sDir))
            {
                count += ScanForPhotos(d);
            }
            return count;
        }


        public int GetPhotosCount()
        {
            if(OutputPath == null)
            {
                return 0;
            }
            int count = 0;
            foreach(string d in Directory.GetDirectories(OutputPath))
            {
                if (!d.EndsWith("ThumbsNail"))
                {
                    count += ScanForPhotos(d);
                }
            }
            return count;
        }





    }
}